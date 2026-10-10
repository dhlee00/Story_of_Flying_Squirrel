# 날다람쥐 이야기

귀여운 날다람쥐가 친구를 구하기 위한 모험을 떠나는 2D 횡스크롤 RPG 게임입니다.

<img width="654" height="368" alt="KakaoTalk_20261010_161429389" src="https://github.com/user-attachments/assets/83d39e64-cdeb-45f8-8aee-351d5fa003cd" />

---

📺 [시연 영상 (YouTube)](링크)<br>
🎮 [빌드 apk 다운로드 (Google Drive)](https://drive.google.com/file/d/1_LEy2sbTx3pJfmc48KK42GYTq-Kdo1ia/view?usp=drive_link)<br>
📄 [기획서](https://github.com/dhlee00/Story_of_Flying_Squirrel/blob/main/%EB%82%A0%EB%8B%A4%EB%9E%8C%EC%A5%90%20%EC%9D%B4%EC%95%BC%EA%B8%B0%20%EA%B8%B0%ED%9A%8D%EC%84%9C.pdf)<br>

---

## 프로젝트 개요

- **개발 형태:** 개인 프로젝트
- **개발 기간:** 2022.06 ~~ 2022.08 (약 2개월)
- **사용 엔진 및 언어:** Unity, C#

---

## 주요 기능

<details><summary><b>일반 적 행동 패턴 설계</b></summary>

<img width="654" height="368" alt="KakaoTalk_20261008_211834369" src="https://github.com/user-attachments/assets/3dd656b4-a8df-4376-8e50-08bb0f0454c4" />
<img width="654" height="368" alt="KakaoTalk_20261008_213039812" src="https://github.com/user-attachments/assets/967c52fe-c7e8-436e-84b7-c881826a706a" />


#### 코드 [(전체 코드)](Scripts/EnemyManager.cs)

* 플레이어 감지
  ```csharp
  private void Update()
  {
      // 플레이어 X, Y 거리 차이 계산
      detectDisX = Mathf.Abs(pm.transform.position.x - transform.position.x);
      detectDisY = Mathf.Abs(pm.transform.position.y - transform.position.y);

      // 이동 방향에 따른 레이의 발사 위치 설정
      if (dir > 0)
      {
          gizmos = 1;
      }

      else if (dir < 0)
      {
          gizmos = -1;
      }

      // 전방 지형 및 플레이어 감지를 위한 레이 발사
      Debug.DrawRay(transform.position + new Vector3(gizmos * 0.9f, 0, 0), Vector2.down * 1.8f, Color.red);
      hit = Physics2D.Raycast(transform.position + new Vector3(gizmos * 0.9f, 0, 0), Vector2.down, 1.8f, mask);
  }
  ```
  > 실시간으로 변하는 플레이어와의 거리를 매 프레임 측정하여 상태 전환의 기준으로 삼았습니다.<br>
  적의 이동 방향의 전방 아래로 레이를 발사해 지형이 끊기는 것을 감지하여 이동을 제어하도록 구현했습니다.
  <br>
  
* 행동 처리
  ```csharp
  private void Update()
  {
      // 이동
      rig.velocity = new Vector2(dir * speed, rig.velocity.y);

      // 적 상태에 따른 행동 함수 호출
      switch (eState)
      {
          case EnemyState.Idle: Idle(); break;
          case EnemyState.Walk: Walk(); break;
          case EnemyState.Attack: Attack(); break;
          case EnemyState.Damaged: break;
          case EnemyState.Die: Die(); break;
      }
  }
  ```
  > 변수에 저장해둔 플레이어와의 거리 값을 기준으로 대기, 이동, 공격 중 한가지를 실행합니다.
  <br>
  
#### 설계 의도
> 여기에 쓰기
</details>
