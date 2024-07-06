using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
게임의 UI를 담당 
*/
public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    // 플레이어의 정보 관련 UI 담당 
    [SerializeField] private Slider playerHp;
    [SerializeField] private Slider playerExp;
    private float maxHp = 100;  // 유저의 총 체력 
    private float currentHp = 100;  // 유저의 현재 체력 

    private float currentExp = 0;
    private float maxExp = 100;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    // 유저의 체력 관리 
    public void UpdatePlayerHp() {
        
    }

    // 유저의 경험치바 관리
    public void UpdatePlayerExp(float currentExp, float maxExp) {
        this.currentExp = currentExp;
        this.maxExp = maxExp;
        playerExp.value = currentExp / maxExp;
    }

    // 유저의 레벨 관리 
    public void UpdatePlayerLevel() {

    }


    // // 유저의 체력바 관리 
    // public void DetectPlayerHp()
    // {
    //     playerHp.value = (float)currentHp / (float)maxHp;
    // }

    // // 유저의 경험치바 관리
    // public void DetectPlayerExp()
    // {
    //     playerExp.value = (float)currentExp / (float)maxExp;
    // }
}
