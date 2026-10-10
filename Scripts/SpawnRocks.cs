using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRocks : MonoBehaviour
{
    // 바위 생성 위치들
    Transform RockSpawnPoints;

    // 바위 번호
    int rockNum;

    // 뽑은 바위 생성 위치를 저장할 변수
    Queue<int> savePick = new Queue<int>();

    // 턴을 저장할 변수
    int turn = 0;

    void Start()
    {
        RockSpawnPoints = GetComponent<Transform>();

        // 바위 스폰 시작
        SpawnRock();
    }

    void SpawnRock()
    {
        // 턴 증가
        turn++;

        // 랜덤으로 바위 뽑기
        rockNum = Random.Range(1, 4);

        // 랜덤으로 바위 생성 위치 뽑기
        int pickSpawnPoint = Random.Range(0, RockSpawnPoints.childCount);

        // 저장 된 위치와 다른 위치를 뽑을 동안
        while (savePick.Contains(pickSpawnPoint))
        { 
            // 위치 뽑기
            pickSpawnPoint = Random.Range(0, RockSpawnPoints.childCount);
        }

        // 뽑은 위치를 저장
        savePick.Enqueue(pickSpawnPoint);

        // 7턴 이후로
        if (turn >= 7)
        {
            // 저장한 위치 중 가장 먼저 저장한 것 부터 삭제
            savePick.Dequeue();
        }

        // 바위 던지는 위치 설정
        GameObject Rock = Instantiate(Resources.Load($"Rock_{rockNum}"), RockSpawnPoints.GetChild(pickSpawnPoint)) as GameObject;

        // 바위를 던짐 (힘을 부여)
        Rock.GetComponent<Rigidbody2D>().AddForce(new Vector2(-8, 0), ForceMode2D.Impulse);

        // 자기자신 호출
        Invoke("SpawnRock", 0.3f);
    }
}
