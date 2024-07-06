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
    [SerializeField] private Slider playerHp;       // 플레이어 HP 
    [SerializeField] private Slider playerExp;      // 플레이어의 경험치 

    [SerializeField] private Text playerLevelText;


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
    public void UpdatePlayerExp(float currentExp, float maxExp, int playerLevel) {
        this.currentExp = currentExp;
        this.maxExp = maxExp;
        playerExp.value = currentExp / maxExp;

        UpdatePlayerLevel(playerLevel);
    }

    // 유저의 레벨 관리 
    private void UpdatePlayerLevel(int playerLevel) {
        // 레벨 업데이트 
        playerLevelText.text = "Lv " + playerLevel;
    }
}
