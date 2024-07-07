using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Video;

public class Monster : MonoBehaviour
{

    int HP = 1;

    Rigidbody2D rig;
    SpriteRenderer sr;

    Animator animator;

    float move = -1;

    enum State
    {
        Idle,
        MoveLeft,

        MoveRight,
        Chase
    }

    State state;
    PlayerController player;

    bool cancelWait;


    // Start함수가 코루틴으로 작동 
    IEnumerator Start() {
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cancelWait = false;

        while (HP > 0) {
            yield return StartCoroutine(state.ToString());
        }
    }

    void StateChange(State next) {
        state = next;
        cancelWait = true;
    }

    void SetNextRoaming() {
        // 현재 State가 Idle, MoveLeft, MoveRight 이면 다음 로밍 설정
        if (state == State.Idle || state == State.MoveLeft || state == State.MoveRight) {
            state = (State)Random.Range(0, 3);
        }
    }

    IEnumerator CancelableWait(float t) {
        var d = Time.deltaTime;
        cancelWait = false;

        // 지정한 시간 t 만큼 대기
        // cancelWait == true면 중단
        while(d < t && cancelWait == false) {
            d += Time.deltaTime;
            yield return null;
        }

        if (cancelWait == true) print("Canceled");
    }

    IEnumerator Idle() {
        // 움직이지 않음
        move = 0;

        animator.SetBool("isMoving", false);

        // 1~2초간 대기
        yield return StartCoroutine(CancelableWait((Random.Range(1f, 3f))));

        SetNextRoaming();
    }

      IEnumerator MoveLeft()
   {      
      move = -1;
      sr.flipX = false;      
      animator.SetBool("isMoving", true);
      yield return StartCoroutine(CancelableWait((Random.Range(3f, 6f))));

      SetNextRoaming();
   }

    IEnumerator MoveRight()
   {
      move = 1;
      sr.flipX = true;
      animator.SetBool("isMoving", true);
      yield return StartCoroutine(CancelableWait((Random.Range(3f, 6f))));

      SetNextRoaming();
   }

   IEnumerator Chase()
   {
      if (player != null)
      {
         Vector3 vec;

         do
         {
            yield return new WaitForSeconds(0.5f);

            // Player와의 방향 벡터
            vec = player.transform.position - transform.position;

            // Player 가 오른쪽에 있으면
            if (vec.x > 0)
            {
               sr.flipX = true;
               move = 1f;
               animator.SetBool("isMoving", true);
            }
            // Player 가 왼쪽에 있으면
            else
            {
               sr.flipX = false;
               move = -1f;
               animator.SetBool("isMoving", true);
            }
         }
         // 거리가 6 이내인 동안 따라 다니기
         while (vec.magnitude < 3f); 

         print("End of chase");

         // Player 가 멀어지면 State 를 Idle 로 변경
         StateChange(State.Idle);
      }
   }


   // Update is called once per frame
   void FixedUpdate()
   {
      // move 값에 따라 x 축으로 이동
      rig.velocity = new Vector2(move, rig.velocity.y);
   }

   // Sensor 객체의 CircleCollider2D와 충돌시 발생하는 이벤트
   private void OnTriggerEnter2D(Collider2D collision)
   {           
      // 충돌한 객체의 이름이 Player 가 아니면 return
      if (collision.gameObject.name != "Player") return;
      player = collision.gameObject.GetComponent<PlayerController>();
      if (state != State.Chase) StateChange(State.Chase);
   }
}
