using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // 몬스터 기본 정보 설정 
    [SerializeField] private int hp;        // 몬스터의 체력 
    [SerializeField] private float moveSpeed; // 몬스터의 스피드

    // 행동 지표를 결정할 변수 
    public int nextMove;


    Rigidbody2D rigid2D;

    Animator animator;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
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
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("몬스터의 현재 체력: " + hp);
        if (hp <= 0)
        {
            Debug.Log("몬스터 사망");
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

    void Turn()
    {
        nextMove *= -1;
        // nextMove가 1이면 
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }
}
