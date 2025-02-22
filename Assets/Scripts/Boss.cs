using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Resources;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using UnityEngine.Video;

/*
- 보스용 스크립트 
*/
public class Boss : MonoBehaviour
{

    // 상태를 지정할 EnumClass
    enum State
    {
        Idle, Attack, Chase
    }
    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    private Transform player;


    State currentState;

    private float move = 0;


    // 몬스터의 능력 지정 
    private int maxHp = 500; // 총 체력 

    private int currentHp = 500; // 현재 체력 
    private int attackDamage = 20;  // 공격력 

    // 공격 딜레이용 변수 
    private float curTime;
    public float coolTime = 2f;

    // 추격 거리와 공격 거리 
    float chaseDistance = 6f;
    float attackDistance = 1.5f;

    // 공격 상태 관련 변수 
    bool isAttacking = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 기본 Start Coroutine으로 변경 가능 
    IEnumerator Start()
    {
        // 몬스터의 체력이 0이상인 동안에는 계속 코루틴을 반환
        while (currentHp > 0)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    private void FixedUpdate()
    {
        // 각 상태에서 move값 변경 후 move값에 따라 x축으로 이동 y축은 현재 속도 그대로 
        rigid.velocity = new Vector2(move, rigid.velocity.y);
    }


    // 상태 변경
    void ChangeState(State newState)
    {
        currentState = newState;
    }

    IEnumerator Idle()
    {
        move = 0;
        animator.SetBool("isMove", false);
        yield return null;
    }
    // 공격
    IEnumerator Attack()
    {
        Debug.Log("공격!!");
        move = 0;
        animator.SetBool("isMove", false);

        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttack", true);
            Debug.Log("플레이어에게 20의 데미지를 입힘!!");
            yield return new WaitForSeconds(0.8f);
            animator.SetBool("isAttack", false);
            isAttacking = false;
        }

        Vector3 vec = player.transform.position - transform.position;
        if (vec.magnitude > attackDistance)
        {
            // 멀어지면
            ChangeState(State.Chase);
        }
        yield return null;
    }


    // 몬스터가 플레이어로 부터 공격 받았을때 
    public void OnDamaged(int getDamage)
    {
        Debug.Log("공격받았을때 데미지 처리");
        currentHp -= getDamage; // 현재 체력에서 damage 만큼 깎음

        if (currentHp <= 0)
        {
            // HP가 0이하가 될 경우 사망 처리 
            Die();
        }
    }

    private void Die()
    {

    }

    // 추격
    IEnumerator Chase()
    {
        if (player != null)
        {
            Vector3 vec;

            do
            {

                yield return new WaitForSeconds(0.5f);

                // Player와의 방향 벡터 : 플레이어의 위치 파악 용도  
                vec = player.transform.position - transform.position;

                // Player가 오른쪽에 있으면
                if (vec.x > 0)
                {
                    spriteRenderer.flipX = false;
                    move = 1f;
                    animator.SetBool("isMove", true);
                }

                // Player가 왼쪽에 있으면 
                else
                {
                    spriteRenderer.flipX = true;
                    move = -1f;
                    animator.SetBool("isMove", true);
                }

                if (vec.magnitude <= attackDistance)
                {
                    ChangeState(State.Attack);
                    yield break;
                }
            }

            // 거리가 6이내인 동안 따라 다니기 
            while (vec.magnitude < chaseDistance);


            // Player가 멀어질 경우 State를 Idle로 변경
            ChangeState(State.Idle);
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 충돌한 객체가 player가 아니면 return
        if (collider.gameObject.name != "Player") return;
        if (currentState != State.Chase) ChangeState(State.Chase);
    }

}
