using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 공격 감지 센서 
public class AttackSensor : MonoBehaviour
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
        if (collider.gameObject.name == "Player")
        {
            Debug.Log("AttackSensor와 Player가 닿았다!!");
            // 부모 객체 가져오기 
            Boss boss = GetComponentInParent<Boss>();
            Debug.Log("AttackSensor에 닿았을때 상태" + boss.currentState);
            boss.ChangeState(Boss.State.Attack);
            Debug.Log("AttackSensor에 닿았을때 상태2" + boss.currentState);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Debug.Log("AttackSensor와 Player가 접촉을 끝냈다!!");

            // 부모 객체 가져오기 
            Boss boss = GetComponentInParent<Boss>();
            boss.ChangeState(Boss.State.Chase);
        }
    }
}
