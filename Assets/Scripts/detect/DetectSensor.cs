using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어 감지 센서 
public class DetectSensor : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 부모객체 가져오기 
        Boss boss = GetComponentInParent<Boss>();
        
        // 충돌한 객체가 플레이어가 아닌 경우
        if (collider.gameObject.name != "Player") return;

        // 현재 상태가 추적상태가 아닐 경우 추적 상태로 변경 
        if (boss.currentState != Boss.State.Chase) boss.ChangeState(Boss.State.Chase);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Debug.Log("Player와 DetectSensor가 접촉을 끝냈다!!");
        }
    }
}
