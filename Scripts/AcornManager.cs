using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornManager : MonoBehaviour
{
    public void Destroy()
    {
        // 자기자신 삭제
        Destroy(gameObject);
    }
}
