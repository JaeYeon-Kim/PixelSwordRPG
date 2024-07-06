using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
게임의 UI를 담당 
*/
public class UIManager : MonoBehaviour
{

    // 플레이어의 정보 관련 UI 담당 
    [SerializeField] private Slider playerHp;
    [SerializeField] private Slider playerExp;
    private float maxHp = 100;  // 유저의 총 체력 
    private float currentHp = 100;  // 유저의 현재 체력 

    private float currentExp = 0;
    private float maxExp = 100;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 테스트를 위해 T키 입력시 유저의 체력 10씩 감소
        if(Input.GetKeyDown(KeyCode.T)) {
            currentHp -= 10;
        }

        // 테스트를 위해 Y키 입력시 유저의 경험치 10씩 증가 
        if(Input.GetKeyDown(KeyCode.Y)) {
            currentExp += 10;
        }

        DetectPlayerHp();
        DetectPlayerExp();
    }


    // 유저의 체력바 관리 
    void DetectPlayerHp()
    {
        playerHp.value = (float)currentHp / (float) maxHp;
    }

    // 유저의 경험치바 관리
    void DetectPlayerExp() {
        playerExp.value = (float) currentExp / (float) maxExp;

        // 경험치가 max와 currentExp가 같아질때 레벨업 처리
        if(currentExp >= maxExp) {
            Debug.Log("레벨업이라구!!");
        }
    }
}
