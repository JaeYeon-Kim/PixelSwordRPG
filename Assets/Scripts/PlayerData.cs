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
        this.exp += expAmount;

        // 레벨업 체크 처리: 현재 경험치가 목표 경험치를 넘으면 처리  
        if(this.exp >= this.maxExp) {
            LevelUp();
        }
    }

    // 레벨업 메서드 
    private void LevelUp() {
        // 현재 경험치 초기화 
        this.exp = 0;

        // 목표 경험치 2배 증가 
        this.maxExp *= 2;

        // 레벨 증가 
        this.level++;

        Debug.Log("레벨 몇이니?" + level);
    }

    // 체력 감소 메서드
    public void TakeDamage(int damageAmount) {
        this.currentHealth -= damageAmount;
        if(this.currentHealth <= 0) {
            this.currentHealth = 0;
        }
    }
}
