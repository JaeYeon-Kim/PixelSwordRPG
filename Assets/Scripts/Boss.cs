using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
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
    public enum State
    {
        Idle, Attack, Chase
    }
    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    private Transform player;


    public State currentState;

    private float move = 0;


    // 몬스터의 능력 지정 
    private int maxHp = 500; // 총 체력 

    private int currentHp = 500; // 현재 체력 
    private int attackDamage = 20;  // 공격력 

    // 공격 딜레이용 변수 
    private float curTime;
    public float coolTime = 0.5f;

    // 공격 감지 거리
    private float attackDetectDistance = 1.5f;


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
    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    // 코루틴 정지 
    public void StopState(State currentState) {
        StopCoroutine(currentState.ToString());
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("정지");
        move = 0;
        animator.SetBool("isAttack", false);
        animator.SetBool("isMove", false);
    }
    // 공격
    IEnumerator Attack()
    {
        if(currentState != State.Attack) yield break;

        Debug.Log("공격 들어오니?");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("공격!");
        // 공격 딜레이를 줌 : curtime이 0보다 작을때 z키를 누르면 공격이되고, curTime은 coolTime으로 초기화 
        if (curTime <= 0)
        {
            Debug.Log("공격 내부 타니?");
            animator.SetBool("isAttack", true);
            // 공격 
            curTime = coolTime;
        }
        else
        {
            curTime -= Time.deltaTime;
        }
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
                if(currentState != State.Chase) yield break;

                yield return new WaitForSeconds(0.5f);

                animator.SetBool("isAttack", false);

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
            }

            // 거리가 6이내인 동안 따라 다니기 
            while (vec.magnitude < 6f);


            // Player가 멀어질 경우 State를 Idle로 변경
            ChangeState(State.Idle);
        }
    }

}
