using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
- 포탈 이동을 관리하는 스크립트 
*/
public class PortalInteraction : MonoBehaviour
{

    // 플레이어가 포탈에 닿았는지 확인해줌
    bool isPortalTriggered = false;
    MainCameraController mainCameraController;

    // 새로운 맵 카메라 최소, 최대 범위 설정
    [SerializeField] Vector2 newMinCameraBoundary;
    [SerializeField] Vector2 newMaxCameraBoundary;
    // 플레이어가 새로운 맵으로 이동된 후 위치 조절 
    [SerializeField] Vector2 playerPosOffset;
    // 다음 포탈 위치 변수 
    [SerializeField] Transform exitPos;



    private void Awake() {
        mainCameraController = Camera.main.GetComponent<MainCameraController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 포탈에 접촉한 상태에서 플레이어가 방향키 위키를 누르면 포탈 작동
        if (isPortalTriggered && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("포탈 작동!!");
            mainCameraController.minCameraBoundary = newMinCameraBoundary;
            mainCameraController.maxCameraBoundary = newMaxCameraBoundary;

            mainCameraController.player.position = exitPos.position + (Vector3)playerPosOffset;
        }
    }



    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 접촉한 대상이 플레이어면?
        if (collider.CompareTag("Player"))
        {
            Debug.Log("접촉한 대상이 플레이어입니다!");
            isPortalTriggered = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        // 접촉하던 대상중 나간 대상이 플레이어면?
        if (collider.CompareTag("Player"))
        {
            Debug.Log("접촉하다가 나간 대상이 플레이어입니다!");
            isPortalTriggered = false;
        }
    }
}
