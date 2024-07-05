using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
- 게임 매니저는 여러 곳에서 호출하기 때문에 싱글톤으로 관리해줌 
- 게임에서 단 하나만 존재 
*/
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerData playerData;


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
        playerData = new PlayerData(1, 100, 100, 10, 5);
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySound(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 경험치를 획득하는 메서드 
    public void GainExperience(int amount) {
        playerData.GetExp(amount);
    }
}
