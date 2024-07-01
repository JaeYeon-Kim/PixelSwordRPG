using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
- 사용자의 입력만을 받는 클래스: 입력 감지만 행하고 실제 행동은 PlayerController에서 진행 
*/
public class PlayerInput : MonoBehaviour
{
    private PlayerController PlayerController;


    private bool facingDirection = true;    // 캐릭터의 방향 전환을 위한 bool 값


    private void Awake()
    {
        // 사용자의 입력을 행하는 PlayerController 
        PlayerController = GetComponent<PlayerController>();
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // 플레이어 이동 입력
        float x = Input.GetAxis("Horizontal");

        PlayerController.Move(x);
        // 플레이어 점프 입력   
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerController.Jump();
        }

        // 방향 전환 로직
        // 오른쪽키를 눌렀을때 플레이어는 왼쪽을 보고 있어야함 
        if (x > 0 && !facingDirection)
        {
            Flip();
        }
        else if (x < 0 && facingDirection)
        {
            Flip();
        }
    }

    // 플레이어 방향키 입력에 따른 좌우 전환 로직 
    private void Flip()
    {
        // bool 값을 반대로 변경해주고 현재 플레이어가 보고있는 방향 x좌표를 반대로 변경
        facingDirection = !facingDirection;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}
