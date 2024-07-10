using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;



/*
몬스터 기본 로직 
*/
public class Enemy : MonoBehaviour
{

    // 몬스터 기본 정보 설정 
    [SerializeField] private float moveSpeed; // 몬스터의 스피드

    [SerializeField] private float attackDamage; // 몬스터의 공격력

    [SerializeField] GameObject coinPrefab; // 몬스터가 죽을 경우 드랍할 코인 프리팹

    private int maxHp = 20;        // 몬스터의 총 체력 
    private int currentHp = 20;    // 몬스터의 현재 체력 


    // 몬스터를 따라다니는 체력바 지정
    [SerializeField] private GameObject monsterHpBar;
    // [SerializeField] private GameObject canvas;

    Slider hpSlider;

    RectTransform hpBar;

    // HudText
    [SerializeField] private GameObject hudDamageText;
    [SerializeField] Transform hudPosition;


    // 타격 이펙트 
    [SerializeField] private GameObject hitEffect;      // 효과 프리팹

    [SerializeField] private AudioClip hitSound;     // 효과 재생음 
    private float knockbackForce = 1f;  // 플레이어로 부터 피격당했을때 몬스터가 밀려나는 힘 

    // 행동 지표를 결정할 변수 
    public int nextMove;

    bool isDamaged;     // 데미지를 입었는지

    Rigidbody2D rigid2D;

    Animator animator;

    SpriteRenderer spriteRenderer;

    BoxCollider2D boxCollider2D;
    AudioSource audioSource;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Invoke("Think", 5);
    }
    // Start is called before the first frame update
    void Start()
    {
        // 몬스터 생성시 몬스터 상단에 체력바 붙이기
        hpBar = Instantiate(monsterHpBar, GameObject.Find("Canvas").transform).GetComponent<RectTransform>();
        hpSlider = hpBar.GetComponentInChildren<Slider>();
        hpSlider.value = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        // 체력바 붙이기 
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.2f, 0));
        hpBar.position = _hpBarPos;
    }

    void FixedUpdate()
    {
        // 데미지를 입고 있지 않을때만 이동 시킴 
        if (!isDamaged)
            // 몬스터 이동 로직 : x축은 좌측 방향 * 속도, y축은 현재 속력 그대로 
            rigid2D.velocity = new Vector2(nextMove * moveSpeed, rigid2D.velocity.y);

        // 지형 체크 

        Vector2 frontVector = new Vector2(rigid2D.position.x + nextMove * 0.2f, rigid2D.position.y);
        // 레이 범위 체크를 위한 가상의 레이를 디버그로 쏨 
        Debug.DrawRay(frontVector, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVector, Vector3.down, 1, LayerMask.GetMask("Platform"));

        // 맞은 개체가 없다면 
        if (rayHit.collider == null)
        {
            Debug.Log("경고!! 이 앞 낭떠러지");
            Turn();
        }

    }

    // 데미지를 입는 메소드 
    public void TakeDamage(int damage, Vector3 playerPosition)
    {
        isDamaged = true;
        rigid2D.velocity = Vector2.zero;
        int knockbackDirection = playerPosition.x > transform.position.x ? -1 : 1;
        rigid2D.AddForce(new Vector2(0, 0.5f), ForceMode2D.Impulse);
        currentHp -= damage;
        hpSlider.value = (float)currentHp / (float)maxHp;
        GameObject cloneHitEffect = Instantiate(hitEffect, new Vector2(transform.position.x, transform.position.y + 0.1f), Quaternion.identity);
        animator.SetTrigger("isHit");
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        StartCoroutine(HitEffectDelay(cloneHitEffect)); // 타격 이펙트 생성 및 재생 

        // HudText 표시 
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPosition.position;
        hudText.GetComponent<DamageText>().damage = damage;

        if (currentHp <= 0)
        {
            Debug.Log("몬스터 체력다닳음!!" + gameObject.name);
            
            rigid2D.velocity = Vector2.zero;        // 체력이 다닳으면 몬스터의 움직임을 멈춤 
            if (cloneHitEffect != null)
            {            // 효과 이펙트가 남아있을경우 삭제 
                Destroy(cloneHitEffect);
            }
            animator.SetTrigger("isDie");           // 사망 애니메이션 실행 
            // 몬스터를 판단하여 spawn 시스템에게 자신이 제거되었다는것을 알려줌 
            // 버섯일때 
            if (gameObject.name == "MushRoom(Clone)")
            {   Debug.Log("버섯!!");
                SpawnManager2.instance.enemyCount--;
                SpawnManager2.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
            }

            // 달팽이 일때 
            else if (gameObject.name == "Snail(Clone)")
            {
                Debug.Log("달팽이!!");
                SpawnManager.instance.enemyCount--;
                Debug.Log("transform parent name: "+transform.parent.name);
                SpawnManager.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
            }
            Destroy(gameObject);    // 몬스터 제거 
            Destroy(hpBar.gameObject); // hpBar제거


            DropCoin();

            // 경험치 획득 - 테스트용
            GameManager.instance.GainExperience(10);
        }
    }

    // 몬스터가 생각하는 메소드 (재귀)
    void Think()
    {
        nextMove = Random.Range(-1, 2);

        // 이동 애니메이션 추가 (nextMove는 랜덤값)
        animator.SetInteger("WalkSpeed", nextMove);


        // Flip(방향 전환)
        if (nextMove != 0)
        {
            // nextMove가 1이면 
            spriteRenderer.flipX = nextMove == 1;
        }

        float nextThinkTime = Random.Range(2f, 5f);

        // Invoke: 해당 메소드를 정해진 시간뒤에 실행 
        Invoke("Think", nextThinkTime);


    }

    // 몬스터가 좌우 전환 로직 
    void Turn()
    {
        nextMove *= -1;
        // nextMove가 1이면 
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }

    // 몬스터가 드랍할 코인 
    void DropCoin()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

        // // 코인의 RigidBody 가져오기
        // Rigidbody2D coinRigidBody = coin.GetComponent<Rigidbody2D>();

        // // 코인을 띄워 올림
        // float jumpForce = 2.0f;

        // coinRigidBody.velocity = new Vector2(0, jumpForce);

        // // 중력 설정
        // coinRigidBody.gravityScale = 1.0f;
        
    }



    // // 몬스터 죽음 딜레이용 
    // IEnumerator DelayedDie(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     // spawn 시스템에게 자신이 제거되었다는것을 알려줌 
    //     SpawnManager.instance.enemyCount--;
    //     SpawnManager.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
    //     Destroy(gameObject);    // 몬스터 제거 
    //     Destroy(hpBar.gameObject); // hpBar제거
    // }

    // 몬스터 hitEffect Delay용
    IEnumerator HitEffectDelay(GameObject hitEffect)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(hitEffect);
    }

    // 몬스터의 충돌 체크 
    private void OnCollisionEnter2D(Collision2D collider)
    {
        // 몬스터가 바닥에 닿아있을 경우 isDamaged = false;
        if (collider.gameObject.tag == "Platform")
        {
            isDamaged = false;
        }

        // 몬스터가 플레이어에게 닿았을 경우 플레이어의 체력을 깎는 로직 추가 
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerController>().OnDamaged(transform.position);
        }
    }
}
