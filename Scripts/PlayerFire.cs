using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    #region 선언부
    // 플레이어 매니저 스크립트
    PlayerManager pm;

    // UI 텍스트 매니저
    public UIManager um;

    // 던지는 힘
    public float throwPower;

    // 던지는 방향
    int dir;

    // 1초 측정할 변수
    float timer = 0;

    // 쿨타임
    public float CoolTime;

    // 공격 상태
    bool attack;

    // 스킬 얻은 상태
    public bool getSkill = false;
    #endregion
    void Start()
    {
        pm = GetComponent<PlayerManager>();

        // 스킬 3 활성화 시
        if (GameManager.instance.skill[2])
        {
            // 공격 쿨타임 감소
            CoolTime /= 3;
        }

        // 스킬 4 활성화 시
        if (GameManager.instance.skill[3])
        {
            // 던지는 힘 증가
            throwPower = 40;
        }
    }

    void Update()
    {
        // 소지한 도토리가 1개 이상일 때 + 공격 쿨타임이 0초일 때 + 공격 버튼 누를 때
        if (pm.acorn > 0 && timer == 0 && pm.attackOn)
        {
            // 오른쪽을 보고 있다면
            if (pm.dir == 1)
            {
                // 오른쪽으로 던짐
                dir = 1;
            }

            // 왼쪽을 보고 있다면
            else if(pm.dir == -1)
            {
                // 왼쪽으로 던짐
                dir = -1;
            }

            // 공격 애니메이션 재생
            pm.anim.SetTrigger("attack");

            // 공격 상태 저장
            attack = true;

            // 소지 도토리 개수 감소
            pm.acorn--;

            // 소지 도토리 수 출력
            um.PlayerAcorn(pm.acorn);
        }

        // 공격을 했다면
        if (attack)
        {
            // 쿨타임 시작
            timer += Time.deltaTime;
        }

        // 1초가 지나면
        if (timer >= CoolTime)
        {
            // 공격 버튼 상태
            pm.attackOn = false;

            // 공격 중단 상태
            attack = false;

            // 쿨타임 초기화
            timer = 0;
        }
    }

    // 공격 애니메이션에서 호출
    void Attack()
    {
        // 도토리를 던지는 위치 설정
        GameObject acornBullet = Instantiate(Resources.Load("AcornBullet"), transform.position + new Vector3(dir * 1.3f, -0.15f, 0), Quaternion.identity) as GameObject;

        // 도토리를 던짐 (힘을 부여)
        acornBullet.GetComponent<Rigidbody2D>().AddForce((new Vector2(1, 0) * dir + new Vector2(0, 0.35f)) * throwPower, ForceMode2D.Impulse);

        // 공격 효과음 재생
        SoundManager.instance.playersound[3].Play();

        // 스킬 2 활성화 시
        if (GameManager.instance.skill[1])
        {
            // 딜레이를 주고 함수 실행
            Invoke("DoubleShot", 0.07f);
        }
    }

    // 도토리 한번 더 던짐 (스킬 2)
    void DoubleShot()
    {
        // 도토리를 던지는 위치 설정
        GameObject acornBullet = Instantiate(Resources.Load("AcornBullet"), transform.position + new Vector3(dir * 1.3f, -0.15f, 0), Quaternion.identity) as GameObject;

        // 도토리를 던짐 (힘을 부여)
        acornBullet.GetComponent<Rigidbody2D>().AddForce((new Vector2(1, 0) * dir + new Vector2(0, 0.35f)) * throwPower, ForceMode2D.Impulse);
    }
}
