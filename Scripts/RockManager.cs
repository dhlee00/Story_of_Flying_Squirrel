using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    // 무언가와 부딪혔다면
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 자기자신 삭제
        Destroy(gameObject);
    }
}