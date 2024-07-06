using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
플레이어 데이터를 관리하는 구조체 
*/

public struct PlayerData {
    public int level;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public float exp;

    public float maxExp;



    // 생성자
    public PlayerData(int initialLevel, int initialMaxHealth, int initialCurrentHealth, int initialAttackPower, 
    float currentExp, float finalExp) {
        level = initialLevel;
        maxHealth = initialMaxHealth;
        currentHealth = initialCurrentHealth;
        attackPower = initialAttackPower;
        exp = currentExp;
        maxExp = finalExp;
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
