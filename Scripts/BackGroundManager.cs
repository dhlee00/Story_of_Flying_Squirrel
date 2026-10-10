using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    #region 선언부
    // 배경 트랜스폼
    public Transform backGround;

    // 카메라 트랜스폼
    public Transform cam;
    #endregion

    void Update()
    {
        // 카메라가 오른쪽으로 배경을 지나쳤다면
        if (cam.position.x > transform.position.x + 40)
        {
            // 첫번째 배경 두번째 배경 뒤로 이동
            transform.position = new Vector2(transform.position.x + 80, 0);
        }
        // 카메라가 왼쪽으로 배경을 지나쳤다면
        else if(cam.position.x < transform.position.x - 40)
        {
            // 두번째 배경 첫번째 배경 앞으로 이동
            transform.position = new Vector2(transform.position.x - 80, 0);
        }
    }
}
