using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// HudText 표시용 
public class DamageText : MonoBehaviour
{

    // 텍스트가 이동하는 속도 
    [SerializeField] private float moveSpeed;


    // 텍스트가 투명해지는 속도
    [SerializeField] private float alphaSpeed;

    // 파괴되는데 걸리는 시간 

    [SerializeField] private float destroyTime;
    TextMeshPro text;   // HudText를 적용할 Text

    Color alpha;        // color 값을 담는 변수 
    // Start is called before the first frame update

    // damage 변수 
    public int damage;
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

        /*
        텍스트의 투명도 조절 : Color.a 값에 따라 투명도가 변한다. 
        0에 가까워질수록 투명해진다. 
        범위: 0.0f ~ 1.0f;

        Mathf.Lerp(a,b,t) t값에 의해서 a와 b사이의 값을 반환 , speed값에 따라 투명해지는 속도가 달라짐 
        */
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        
        text.color = alpha;
    }

    private void DestroyObject() {
        Destroy(gameObject);
    }
}
