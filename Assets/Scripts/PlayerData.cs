using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class PlayerData : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

public struct PlayerData {
    public int level;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int exp;



    // 생성자
    public PlayerData(int initialLevel, int initialMaxHealth, int initialCurrentHealth, int initialAttackPower, int initialExp) {
        level = initialLevel;
        maxHealth = initialMaxHealth;
        currentHealth = initialCurrentHealth;
        attackPower = initialAttackPower;
        exp = initialExp;
    }


    // 경험치 획득 메서드 
    public void GetExp(int expAmount) {
        exp += expAmount;

        // 레벨업 체크 처리 
        if(exp >= 10) {
            Debug.Log("레벨업!!");
        }
    }
}
