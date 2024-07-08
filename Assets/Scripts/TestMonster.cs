using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour
{


    private void Update()
    {
        if (hp <= 0)
        {
            // SpawnManager의 enemyCount 값을 차감
            SpawnManager.instance.enemyCount--;
            SpawnManager.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
            Destroy(this.gameObject);
        }
    }
    public int hp = 3;
    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

}
