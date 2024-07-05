using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
- 카메라가 캐릭터를 따라가도록 설정 
*/
public class MainCameraController : MonoBehaviour
{

    public Transform player;
    [SerializeField] float smoothing = 0.2f;
    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;

    private void FixedUpdate()
    {
        CameraPositionUpdate();
    }


    public void CameraPositionUpdate()
    {
        // Debug.Log("플레이어 위치 확인" + player.position.x + "   " + player.position.y);
        
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPosition.x = Mathf.Clamp(targetPosition.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minCameraBoundary.y, maxCameraBoundary.y);


        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }
}
