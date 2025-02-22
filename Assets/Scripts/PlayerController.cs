using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;

    Animator animator; // 애니메이터 조작을 위한 변수 
    SpriteRenderer spriteRenderer;

    AudioSource audioSource;


    [SerializeField] private float moveSpeed = 3.0f;    // 기본 이동 속도 
    [SerializeField] private float jumpForce = 5.0f;    // 점프 힘

    private int health = 0;

    [SerializeField] private LayerMask groundLayer; // 바닥 체크를 위한 충돌 레이어
    private BoxCollider2D boxCollider2D;    // 오브젝트의 충돌 범위 컴포넌트 
    private bool isGrounded;        // 바닥 체크 변수 
    private Vector3 footPosition; // 발의 위치 

    [SerializeField] private int maxJumpCount = 2; // 땅을 밟기 전까지 할 수 있는 최대 점프 횟수 
    private int currentJumpCount = 0;   // 현재 가능한 점프 횟수 

    // 무기 공격 감지를 위한 속성들 
    [SerializeField] private Transform pos;
    [SerializeField] private Vector2 boxSize;


    // 플레이어 무기 사운드 
    [SerializeField] private AudioClip hitSound;

    // 플레이어 무기 데미지 지정 
    private int attackDamage;



    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        // 플레이어 오브젝트의 Collider 2D min, center, max 위치 정보
        Bounds bounds = boxCollider2D.bounds;

        // 플레이어의 발 위치 설정 
        footPosition = new Vector2(bounds.center.x, bounds.min.y);

        // 플레이어의 발 위치에 원을 생성하고, 원이 바닥과 닿아 있으면 isGrounded = true
        isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, groundLayer);


        // 점프횟수 구현 : 플레이어의 발이 땋에 닿아 있고, y축 속도가 0이하이면 점프 횟수 초기화 
        // velocity.y <=0을 추가하지 않으면 점프키를 누르는 순간에도 초기화가 됨
        // 최대 점프횟수를 2로 설정하면 3번까지 점프가 가능하게 된다.
        if (isGrounded && rigid2D.velocity.y <= 0)
        {
            currentJumpCount = maxJumpCount;
            animator.SetBool("isJumping", false);
        }

    }

    // OverlapBox는 보이지 않기 때문에 그림으로 그려줌
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    // 플레이어의 이동 처리 
    public void Move(float x)
    {
        // x축은 : Input 좌표 * 설정한 이동속도 , y축은 기본 속력(현재는 RigidBody 적용으로 중력) 
        rigid2D.velocity = new Vector2(x * moveSpeed, rigid2D.velocity.y);

        // 걷는 애니메이션 처리 
        if (Mathf.Abs(rigid2D.velocity.x) < 0.3)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

    }

    // 플레이어의 점프 처리 
    public void Jump()
    {
        if (currentJumpCount > 0)
        {
            // JumpForce 만큼 위쪽으로 속력을 설정 
            rigid2D.velocity = Vector2.up * jumpForce;
            animator.SetBool("isJumping", true);
            // 점프횟수 1감소
            currentJumpCount--;
        }

    }


    // 플레이어의 공격 처리 
    public void Attack()
    {
        // 공격 데미지 설정 
        attackDamage = Random.Range(1, 5);

        // 공격 효과음 재생
        audioSource.PlayOneShot(hitSound);

        // 공격 애니메이션 처리 
        animator.SetTrigger("isAttack");


        // 공격 범위에 걸린 Collider들을 가져옴
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        // 이중 Enemy 값만 골라줌 
        foreach (Collider2D collider in collider2Ds)
        {
            // Collider에 걸린 녀석이 적개체 일경우 
            if (collider.tag == "Enemy")
            {
                collider.GetComponent<Enemy>().TakeDamage(attackDamage, transform.position);
            }
            else
            {
                return;
            }
        }
    }

    // 플레이어 충돌 이벤트 
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    // 적으로부터 데미지를 입었을 경우 무적 처리 
    public void OnDamaged(Vector2 targetPosition)
    {
        // playerDamaged 번호로 
        gameObject.layer = 9;

        // 맞았을 경우 플레이어의 색을 변경 해줌(살짝 반투명)
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force(피격 당할 경우 튕겨져 나가는 힘 구현)
        int direction = transform.position.x - targetPosition.x > 0 ? 1 : -1;
        rigid2D.AddForce(new Vector2(direction, 1) * 2, ForceMode2D.Impulse);

        // 체력 감소 시키기 UI
        GameManager.instance.InflictDamageToPlayer(10);

        Invoke("OffDamaged", 2);


        // 플레이어의 체력이 모두 닳을 경우 
        if (GameManager.instance.playerData.currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    // 무적 처리를 해제 
    private void OffDamaged()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    // 플레이어의 죽음 처리
    IEnumerator Die()
    {
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3.0f);
        GameManager.instance.GameRestart();
    }
}
