using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;

   

    [SerializeField] private float moveSpeed = 3.0f;    // 기본 이동 속도 
    [SerializeField] private float jumpForce = 7.0f;    // 점프 힘 


    private void Awake() {
        rigid2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 플레이어의 이동 처리 
    public void Move(float x) {
        // x축은 : Input 좌표 * 설정한 이동속도 , y축은 기본 속력(현재는 RigidBody 적용으로 중력) 
        rigid2D.velocity = new Vector2(x * moveSpeed, rigid2D.velocity.y);

    }

    // 플레이어의 점프 처리 
    public void Jump() {
        // JumpForce 만큼 위쪽으로 속력을 설정 
        rigid2D.velocity = Vector2.up * jumpForce;
    }

    // 플레이어 방향키 입력에 따른 좌우 전환 로직 
    public void Flip() {

    }
}
