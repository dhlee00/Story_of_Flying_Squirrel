using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftDistanceManager : MonoBehaviour
{
    // 플레이어 매니저
    public Transform pm;

    // 도착지점
    public Transform finish;

    // 슬라이더
    Slider leftDistance;

    void Start()
    {
        leftDistance = GetComponent<Slider>();
    }

    void Update()
    {
        // UI에 현재 플레이어의 위치 출력
        leftDistance.value = (pm.position.x + 15) / (finish.position.x + 15);
    }
}
