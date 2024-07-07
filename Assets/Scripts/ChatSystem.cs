using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatSystem : MonoBehaviour
{

    public Queue<string> sentences;
    public string currentSentence;
    public TextMeshPro text;
    public GameObject quad;

    public void OnDialogue(string[] lines, Transform chatPoint)
    {
        // 시작할때 ChatBox의 position을 Point의 position으로 초기화 
        transform.position = chatPoint.position;
        // 큐 초기화 후 string 배열의 값을 전부 큐에 차례로 넣어줌 
        sentences = new Queue<string>();
        sentences.Clear();
        foreach (var line in lines)
        {
            sentences.Enqueue(line);
        }
        StartCoroutine(DialogueFlow(chatPoint));
    }

    IEnumerator DialogueFlow(Transform chatPoint)
    {
        yield return null;  // 프레임 마다 쉼 
        while (sentences.Count > 0)
        {
            Debug.Log("while문 타니?");
            currentSentence = sentences.Dequeue();
            Debug.Log("꺼낸 문장은?" + currentSentence);
            text.text = currentSentence;    // 대사를 담아줌 
            float x = text.preferredWidth;
            x = (x > 3) ? 3 : x + 0.3f;
            quad.transform.localScale = new Vector2(x, text.preferredHeight + 0.3f);

            // 말풍선의 크기가 초기화 된 후에 크기에 맞춰서 다시 초기화 
            transform.position = new Vector2(chatPoint.position.x, chatPoint.position.y + text.preferredHeight / 2);

            yield return new WaitForSeconds(3f);
        }
        Destroy(gameObject);
    }
}
