using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;

    Animator animator; // 애니메이터 조작을 위한 변수 

    [SerializeField] private float moveSpeed = 3.0f;    // 기본 이동 속도 
    [SerializeField] private float jumpForce = 5.0f;    // 점프 힘


    private void Awake() {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() {
        // 빔을쏜다(Debug: 게임 창 상에서 보이지 않음) 매개변수: 빔 시작위치, 빔의 방향, 빔의 색
        Debug.DrawRay(rigid2D.position, Vector3.down, new Color(0, 1, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 플레이어의 이동 처리 
    public void Move(float x) {
        // x축은 : Input 좌표 * 설정한 이동속도 , y축은 기본 속력(현재는 RigidBody 적용으로 중력) 
        rigid2D.velocity = new Vector2(x * moveSpeed, rigid2D.velocity.y);

        // 걷는 애니메이션 처리 
        if(Mathf.Abs(rigid2D.velocity.x) < 0.3) {
            animator.SetBool("isWalking", false);
        } else {
            animator.SetBool("isWalking", true);
        }

    }

    // 플레이어의 점프 처리 
    public void Jump() {
        // JumpForce 만큼 위쪽으로 속력을 설정 
        rigid2D.velocity = Vector2.up * jumpForce;
        animator.SetBool("isJumping", true);
    }
}
