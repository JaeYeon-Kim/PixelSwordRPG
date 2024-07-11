using System;
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

    [SerializeField] private Text playerGoldText;


    // FadeInFadeOut을 위한 Panel 및 속성값 
    [SerializeField] private Image Panel;

    float time = 0f;        // 0부터 1까지 deltaTime을 더해서 지속시간으로 사용 
    float F_time = 1f;      // 페이드가 몇초간 지속될지 정하는 값 


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

    // 유저의 경험치바 관리
    public void UpdatePlayerExp(float currentExp, float maxExp, int playerLevel)
    {
        this.currentExp = currentExp;
        this.maxExp = maxExp;
        playerExp.value = currentExp / maxExp;

        UpdatePlayerLevel(playerLevel);
    }

    // 유저의 레벨 관리 
    private void UpdatePlayerLevel(int playerLevel)
    {
        // 레벨 업데이트 
        playerLevelText.text = "Lv " + playerLevel;
    }

    // 유저의 체력 관리 
    public void UpdatePlayerHp(int currentHealth, int maxHealth)
    {
        this.currentHp = currentHealth;
        this.maxHp = maxHealth;
        playerHp.value = (float)currentHp / (float)maxHp;
    }

    // 유저의 골드 관리 
    public void UpdatePlayerGold(int coinAmount)
    {
        Debug.Log("UI로 들어오는 골드" + coinAmount);
        playerGoldText.text = coinAmount.ToString();
    }


    // FadeInFadeOut
    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }

    // Fade 기능 구현을 위한 Coroutine
    IEnumerator FadeFlow()
    {
        // 루틴이 시작될때 이미지 활성화 
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(1f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
        yield return null;
    }
}
