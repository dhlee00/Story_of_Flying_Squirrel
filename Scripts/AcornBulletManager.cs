using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornBulletManager : MonoBehaviour
{
    void Start()
    {
        // 3초 뒤에 자기자신 삭제
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 도토리에 맞은 대상이
        switch (collision.gameObject.tag)
        {
            // 적이라면
            case "Enemy":
                // 피해를 입힘
                collision.SendMessage("Damaged");
                //자기자신 삭제
                Destroy(gameObject);
                break;
        }
    }
}
