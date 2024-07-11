using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class DialogueManager : MonoBehaviour, IPointerDownHandler
{
    public static DialogueManager instance;
    public Text dialogueText;
    public GameObject nextText;
    public CanvasGroup dialoguegroup;

    // 큐 선언 (선입선출) - 대화를 담음 
    public Queue<string> sentences;

    private string currentSentence;

    public float typingSpeed = 0.1f;
    private bool isTyping;  // 타이핑 중인지 판단하기 위한 값

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
        sentences = new Queue<string>();
    }

    public void OnDialogue(string[] lines)
    {
        sentences.Clear();  // 큐를 비워줌
        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        // 다이얼로그 호출시 alpha값 1로 
        dialoguegroup.alpha = 1;
        dialoguegroup.blocksRaycasts = true;        // true일때는 마우스 이벤트를 감지함 

        NextSentence();
    }


    public void NextSentence()
    {
        // Count()를 통해 큐의 데이터 갯수 파악 
        if (sentences.Count != 0)
        {
            currentSentence = sentences.Dequeue();  // 큐에 존재하는 데이터 중 가장 먼저 들어온 데이터를 반환하고 제거 

            // 코루틴
            isTyping = true;
            nextText.SetActive(false);

            // 타이핑 효과 코루틴
            StartCoroutine(Typing(currentSentence));

        }
        else
        {
            // 큐안에 데이터가 없을 경우 (대화가 모두 끝나면)
            dialoguegroup.alpha = 0;
            dialoguegroup.blocksRaycasts = false;
        }
    }

    // 타이핑 효과를 주기 위한 코루틴
    IEnumerator Typing(string line)
    {
        dialogueText.text = "";

        // char 형태의 문자를 받아와서 dialogueText에 한글자씩 더해줌 
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 대사 한줄의 끝을 알기 위해 추가 : dialogueText == currentSentence
        if (dialogueText.text.Equals(currentSentence))
        {
            nextText.SetActive(true);
            // 대사 창을 클릭하면 다음 대사로 넘어가게 해줌 
            isTyping = false;
        }
    }

    // 해당 스크립트가 붙은 오브젝트에 클릭, 터치가 있을때 호출 
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("다음 문장 호출하니?");
        // 터치시 NextSentence() 메서드 호출 
        if (!isTyping) NextSentence();
    }
}
