using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region 선언부
    // 플레이어의 트랜스폼 컴포넌트
    public Transform player;

    // 카메라 이동 속도
    public float cameraSpeed;

    // 카메라 이동 제한 범위
    public float min;
    public float max;
    #endregion

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();

        // 카메라의 rect 가져옴
        Rect rect = camera.rect;

        // (가로 / 세로)
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        float scalewidht = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidht;
            rect.x = (1f - scalewidht) / 2f;
        }
        camera.rect = rect;
    }

    void LateUpdate() // 업데이트 바로 다음으로 호출
    {
        // 만약 레벨이 0이상이라면
        if (GameManager.instance.level >= 0)
        {
            // 플레이어의 x,y값, 카메라의 z값 저장
            Vector3 target = new Vector3(player.position.x, transform.position.y, transform.position.z);

            // 카메라의 위치에 타겟의 위치 넣기 (서서히 이동하도록)
            transform.position = Vector3.Lerp(transform.position, target, cameraSpeed * Time.deltaTime);

            // 카메라의 이동 범위 제한
            if (transform.position.x <= min)
            {
                transform.position = new Vector3(min, transform.position.y, transform.position.z);
            }
            else if (transform.position.x >= max)
            {
                transform.position = new Vector3(max, transform.position.y, transform.position.z);
            }
        }
    }
}
