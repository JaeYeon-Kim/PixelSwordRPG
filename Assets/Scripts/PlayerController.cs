using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;

    Animator animator; // 애니메이터 조작을 위한 변수 

    [SerializeField] private float moveSpeed = 3.0f;    // 기본 이동 속도 
    [SerializeField] private float jumpForce = 5.0f;    // 점프 힘

    [SerializeField]
    private LayerMask groundLayer; // 바닥 체크를 위한 충돌 레이어
    private BoxCollider2D boxCollider2D;    // 오브젝트의 충돌 범위 컴포넌트 
    private bool isGrounded;        // 바닥 체크 변수 
    private Vector3 footPosition; // 발의 위치 

    [SerializeField]
    private int maxJumpCount = 2; // 땅을 밟기 전까지 할 수 있는 최대 점프 횟수 
    private int currentJumpCount = 0;   // 현재 가능한 점프 횟수 

    // 무기 공격 감지를 위한 속성들 
    [SerializeField] private Transform pos;
    [SerializeField] private Vector2 boxSize;

    // 플레이어 무기 데미지 지정 
    private int attackDamage;



    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        // // 빔을쏜다(Debug: 게임 창 상에서 보이지 않음) 매개변수: 빔 시작위치, 빔의 방향, 빔의 색
        // Debug.DrawRay(rigid2D.position, Vector3.down, new Color(0, 1, 0));


        // // RayCast를 쏴서 빔과 닿았는지 확인 : 파라미터: 빔 시작위치, 빔의 방향, distance, 닿은 Layer
        // RaycastHit2D rayHit = Physics2D.Raycast(rigid2D.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        // // 아래로 떨어질때만 빔을 쏨
        // if (rigid2D.velocity.y < 0)
        // {
        //     if (rayHit.collider != null)
        //     {
        //         if (rayHit.distance < 0.3f)
        //         {
        //             isGrounded = true;
        //             animator.SetBool("isJumping", false);
        //         }
        //     }
        // }

        // // 점프 횟수 구현
        // if (isGrounded && rigid2D.velocity.y <= 0)
        // {
        //     currentJumpCount = maxJumpCount;
        // }
        //

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

    // Update is called once per frame
    void Update()
    {

    }

    // OverlapBox는 보이지 않기 때문에 그림으로 그려줌
    private void OnDrawGizmos() {
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
    public void Attack() {
        // 공격 데미지 설정 
        attackDamage = Random.Range(1, 5);

        // 공격 애니메이션 처리 
        animator.SetTrigger("isAttack");


        // 공격 범위에 걸린 Collider들을 가져옴
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        // 이중 Enemy 값만 골라줌 
        foreach (Collider2D collider in collider2Ds)
        {
            // Debug.Log(collider);
            // Collider에 걸린 녀석이 적개체 일경우 
            if(collider.tag == "Enemy") {
                collider.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
    }
}
