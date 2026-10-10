using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneBackGround : MonoBehaviour
{
    private void Update()
    {
        // 각각의 배경 레이어들을 왼쪽으로 이동시켜서 x값이 -40이 되면 현재 카메라에 보여지는 배경의 뒤쪽으로 이동하는 코드
        if (gameObject.name == "Layer_1")
        {
            transform.position += new Vector3(-0.0025f, 0, 0);

            if (transform.position.x <= -40)
            {
                transform.position = new Vector3(39.9f, transform.position.y, transform.position.z);
            }
        }

        else if (gameObject.name == "Layer_2")
        {
            transform.position += new Vector3(-0.005f, 0, 0);

            if (transform.position.x <= -40)
            {
                transform.position = new Vector3(39.9f, transform.position.y, transform.position.z);
            }
        }

        else if (gameObject.name == "Layer_3")
        {
            transform.position += new Vector3(-0.0075f, 0, 0);

            if (transform.position.x <= -40)
            {
                transform.position = new Vector3(39.9f, transform.position.y, transform.position.z);
            }
        }

        else if (gameObject.name == "Layer_4")
        {
            transform.position += new Vector3(-0.01f, 0, 0);

            if (transform.position.x <= -40)
            {
                transform.position = new Vector3(39.9f, transform.position.y, transform.position.z);
            }
        }
    }
}
