using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// NPC 대화 구현 
public class NpcChat : MonoBehaviour
{
    public string[] sentences;      // 담을 문장 리스트 
    public Transform ChatTr;        // 말풍선 생성 위치 
    public GameObject chatBoxPrefab;    // chatBox 복제용

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TalkNpc", 10f);
    }

    public void TalkNpc()
    {
        GameObject go = Instantiate(chatBoxPrefab);
        go.GetComponent<ChatSystem>().OnDialogue(sentences, ChatTr);
        Invoke("TalkNpc", 10f);
    }
}
