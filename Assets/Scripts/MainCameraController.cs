using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
- 카메라가 캐릭터를 따라가도록 설정 
*/
public class MainCameraController : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] Vector2 minCameraBoundary;
    [SerializeField] Vector2 maxCameraBoundary;

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, this.transform.position.z);


        targetPosition.x = Mathf.Clamp(targetPosition.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }

    // private void Update()
    // {
    //     Vector3 targetPosition = new Vector3(player.position.x, player.position.y, this.transform.position.z);
    //     transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    // }


    // // 플레이어가 움직이면 위치가 변경되는데 카메라 위치도 같은 프레임에 변경되게 하면 카메라 위치가 버벅된다. 
    // // 따라서, 플레이어 위치가 변경 된 후 카메라 위치가 변경되게 하자 
    // private void LateUpdate() {
    //     Vector3 targetPosition = new Vector3(player.position.x, player.position.y, this.transform.position.z);
    //     transform.position = targetPosition;
    // }
}
