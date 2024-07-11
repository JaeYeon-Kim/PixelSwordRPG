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

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.name == "Player") {
            Debug.Log("Player와 DetectSensor가 닿았다.");
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.name == "Player") {
            Debug.Log("Player와 DetectSensor가 접촉을 끝냈다!!");
        }
    }
}
