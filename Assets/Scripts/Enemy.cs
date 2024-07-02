using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int hp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) {
        hp -= damage;
        Debug.Log("몬스터의 현재 체력: " + hp);
        if(hp <= 0) {
            Debug.Log("몬스터 사망");
        }
    }
}
