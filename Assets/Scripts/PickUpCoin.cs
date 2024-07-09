using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCoin : MonoBehaviour
{

    private int dropCoinAmount = 0;

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player") {
            dropCoinAmount = Random.Range(1, 100);
            Debug.Log("동전의 양은?" + dropCoinAmount);
            Debug.Log("플레이어와 동전이 닿았다!");
            GameManager.instance.GetGold(dropCoinAmount);
            Destroy(gameObject);
        }
    }
}
