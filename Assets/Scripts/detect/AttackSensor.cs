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
    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.name == "Player") {
            Debug.Log("AttackSensor와 Player가 닿았다!!");
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.name == "Player") {
            Debug.Log("AttackSensor와 Player가 접촉을 끝냈다!!");
        }
    }
}
