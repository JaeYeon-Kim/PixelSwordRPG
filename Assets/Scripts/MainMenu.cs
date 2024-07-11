using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainUi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 게임 시작 
    public void OnClickNewGame() {
        Debug.Log("게임시작!!");
        mainUi.gameObject.SetActive(false);
        StartCoroutine(GameManager.instance.TutorialDialogue());
    }

    // 옵션 설정
    public void OnClickOptionMenu() {
        Debug.Log("옵션켜기!!");
    } 
}
