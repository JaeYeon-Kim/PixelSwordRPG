using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
- 게임 매니저는 여러 곳에서 호출하기 때문에 싱글톤으로 관리해줌 
- 게임에서 단 하나만 존재 
*/
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerData playerData;

    
    [SerializeField] private string[] tutorialDialogue;


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


        // 초기 유저 정보 할당
        playerData = new PlayerData(1, 100, 100, 10, 0, 20, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySound(0);
        StartCoroutine(TutorialDialogue());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 경험치를 획득하는 메서드 
    public void GainExperience(int amount)
    {
        playerData.GetExp(amount);

        Debug.Log("이때 플레이어 레벨 몇?" + playerData.level);

        // UIManager를 통해 경험치 UI 업데이트
        UIManager.instance.UpdatePlayerExp(playerData.exp, playerData.maxExp, playerData.level);
    }

    // 플레이어가 데미지를 입는 메서드 
    public void InflictDamageToPlayer(int damage)
    {
        playerData.TakeDamage(damage);

        // UI 업데이트
        UIManager.instance.UpdatePlayerHp(playerData.currentHealth, playerData.maxHealth);
    }

    // 플레이어가 골드를 획득하는 메서드 
    public void GetGold(int coinAmount)
    {

        playerData.GetGold(coinAmount);

        // UI 업데이트 
        UIManager.instance.UpdatePlayerGold(playerData.currentGold);
    }

    // 게임 재시작 메서드
    public void GameRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("게임 재시작!!");
    }

    // 처음시작시 나오는 다이얼로그
    IEnumerator TutorialDialogue() {
        yield return new WaitForSeconds(1f);
        DialogueManager.instance.OnDialogue(tutorialDialogue);
    }
}
