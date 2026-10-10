using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    #region 선언부
    // 리지드 바디
    Rigidbody2D rig;

    // 콜라이더
    BoxCollider2D bc;

    // 플레이어 매니저
    public PlayerManager pm;

    // 플레이어 글라이딩 매니저
    public GlidingManager glid;

    // 플레이어 공격
    public PlayerFire pf;

    // UI 텍스트 매니저
    public UIManager um;

    // 체력바 슬라이더
    public Slider hpBar;

    // 도착지점
    public GameObject finish;

    // 보스 체력
    public int hp;

    // 보스 좌, 우 방향
    int dir = -1;

    // 보스 이동 속도
    public float speed;

    // 보스의 행동 패턴
    public enum BossState
    {
        Idle,
        Walk,
        CloseAttack,
        FarAttack,
        Damaged,
        Die
    }

    // 보스의 상태
    public BossState bState;

    // 플레이어 X축 감지
    public float detectDisX;

    // 플레이어 Y축 감지
    public float detectDisY;

    // 공격 상태
    bool isAttack = false;

    // 스턴 공격만 진행하는 상태
    public bool OnlyStun;

    // 근거리 공격 타이머
    float closeAttackTimer = 3;

    public int attackPattern;

    // 플레이어 기절 상태
    bool isStun = false;

    // 플레이어 기절 타이머
    float stunTimer;

    // 원거리 공격 타이머
    float farAttackTimer = 0;

    // 랜덤으로 뽑은 숫자 할당할 변수
    int rockNum;

    // 바위 던지기 랜덤 쿨타임 변수
    float randomCoolTime;

    // 바위 던지기 방향
    float throwDir;

    // 바위 던지기 각도
    float throwY = 0.55f;

    // 이동 효과음 타이머
    float soundTimer;

    // 게임 나가기 화면
    public GameObject exitView;

    // 애니메이터
    Animator anim;

    // 플레이어 컨트롤러
    public GameObject playerController;
    #endregion

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        // 보스 기본 상태
        bState = BossState.Idle;
    }

    private void Update()
    {
        // 플레이어가 죽으면 or 클리어 실패 했다면
        if (pm.hp <= 0 || GameManager.instance.isFail)
        {
            // 곰 기능 정지
            enabled = false;
        }

        // 곰이 도토리를 맞아도 죽지 않을 체력일 때 + 플레이어의 도토리가 다 떨어졌다면
        if (hp > pm.acornDamage && pm.acorn <= 0)
        {
            // 게임 나가기 화면 활성화
            exitView.SetActive(true);

            // 만약 죽었다면
            if (hp <= 0)
            {
                // 게임 나가기 화면 비활성화
                exitView.SetActive(false);
            }
        }

        // 플레이어의가 왼쪽에 있다면
        if (pm.transform.position.x < transform.position.x)
        {
            // 왼쪽을 바라봄
            transform.localScale = new Vector3(0.6f, 0.6f, 1);

            hpBar.transform.localScale = new Vector3(0.02f, 0.02f, 1);
        }

        // 플레이어가 오른쪽에 있다면
        else if (pm.transform.position.x > transform.position.x)
        {
            // 오른쪽을 바라봄
            transform.localScale = new Vector3(-0.6f, 0.6f, 1);

            hpBar.transform.localScale = new Vector3(-0.02f, 0.02f, 1);
        }

        // 플레이어 감지
        detectDisX = Mathf.Abs(pm.transform.position.x - transform.position.x);
        detectDisY = Mathf.Abs(pm.transform.position.y - transform.position.y);

        // 이동
        rig.velocity = new Vector2(dir * speed, rig.velocity.y);

        // 이동 애니메이션 재생
        anim.SetInteger("walk", dir);

        // 이동 중이라면
        if (dir != 0)
        {
            // 효과음 타이머 시작
            soundTimer += Time.deltaTime;

            // 타이머가 0.6초라면
            if (soundTimer >= 0.6f)
            {
                // 이동 효과음 재생
                SoundManager.instance.bosssound[0].Play();

                // 타이머 초기화
                soundTimer = 0;
            }
        }

        // 멈췄다면
        else if (dir == 0)
        {
            // 효과음 타이머 초기화
            soundTimer = 0;

            // 이동 효과음 정지
            SoundManager.instance.bosssound[0].Stop();
        }

        // 플레이어 기절 시켰으면
        if (isStun)
        {
            // 기절 타이머 시작
            stunTimer += Time.deltaTime;
        }

        // 1초 이상 기절 시켰다면
        if (stunTimer >= 1)
        {
            // 기절 상태 해제
            isStun = false;

            // 플레이어 기절 해제
            pm.enabled = true;
            glid.enabled = true;
            pf.enabled = true;

            // 플레이어 컨트롤러 활성화
            playerController.SetActive(true);

            // 플레이어 기절 애니메이션 정지
            pm.anim.SetBool("isStun", isStun);

            // 기절 타이머 초기화
            stunTimer = 0;
        }

        // 보스의 상태에 따라 다른 함수 호출
        switch (bState)
        {
            case BossState.Idle: Idle(); break;
            case BossState.Walk: Walk(); break;
            case BossState.CloseAttack: CloseAttack(); break;
            case BossState.FarAttack: FarAttack(); break;
            case BossState.Damaged: break;
            case BossState.Die: Die(); break;
        }
    }

    // 기본 상태일 때
    void Idle()
    {
        // 방향 없음
        dir = 0;

        // 플레이어가 바위 던지기 범위 안에 들어온다면
        if (detectDisX <= 35)
        {
            // 바위 던지기 상태로 전환
            bState = BossState.FarAttack;
        }
    }

    // 원거리 공격 패턴 (바위 던지기)
    void FarAttack()
    {
        // 더 멀어진 경우
        if (detectDisX > 35)
        {
            // 기본 상태로 전환
            bState = BossState.Idle;

            // 스턴 공격만 진행하는 상태 비활성화
            OnlyStun = false;
        }

        // 더 가까워졌다면
        if (detectDisX <= 25 && detectDisY <= 2.2f)
        {
            // 이동 상태로 전환
            bState = BossState.Walk;

            // 스턴 공격만 진행하는 상태 비활성화
            OnlyStun = false;
        }

        // 플레이어가 거기는 가까운데 위에 있을 때
        if (detectDisX <= 8 && detectDisY > 2.2f)
        {
            // 근거리 공격 상태로 전환
            bState = BossState.CloseAttack;

            // 스턴 공격만 진행하는 상태 활성화
            OnlyStun = true;
        }

        // 플레이어의가 왼쪽에 있다면
        if (pm.transform.position.x < transform.position.x)
        {
            // 왼쪽으로 던짐
            throwDir = -1;
        }

        // 플레이어가 오른쪽에 있다면
        else if (pm.transform.position.x > transform.position.x)
        {
            // 오른쪽으로 던짐
            throwDir = 1;
        }

        // 타이머 시작
        farAttackTimer += Time.deltaTime;

        // 타이머가 랜덤 쿨타임을 넘었다면
        if (farAttackTimer >= randomCoolTime)
        {
            // 바위 던지기 애니메이션 재생
            anim.SetTrigger("throw");

            // 바위 던지기 효과음 재생
            SoundManager.instance.bosssound[3].Play();

            // 랜덤 쿨타임 다시 설정
            randomCoolTime = Random.Range(0.7f, 1.5f);

            // 랜덤 각도 다시 설정
            throwY = Random.Range(0.3f, 0.7f);

            // 타이머 초기화
            farAttackTimer = 0;
        }
    }
    
    // 바위 던지기 애니메이션에서 호출
    void ThrowRock()
    {
        // 랜덤으로 바위 뽑기
        rockNum = Random.Range(1, 4);

        // 바위를 던지는 위치 설정
        GameObject Rock = Instantiate(Resources.Load($"Rock_{rockNum}"), transform.position + new Vector3(throwDir * 5f, 0.5f, 0), Quaternion.identity) as GameObject;

        // 바위를 던짐 (힘을 부여)
        Rock.GetComponent<Rigidbody2D>().AddForce((new Vector2(1, 0) * throwDir + new Vector2(0, throwY)) * 30, ForceMode2D.Impulse);
    }

    // 이동 상태일 때
    void Walk()
    {
        // 플레이어와 멀어진 경우
        if (detectDisX > 25 || detectDisY > 2.2f)
        {
            // 바위 던지기 상태로 전환
            bState = BossState.FarAttack;

            // 스턴 공격만 진행하는 상태 비활성화
            OnlyStun = false;

            // 정지
            dir = 0;
        }

        else
        {
            // 효과음 재생
            SoundManager.instance.bosssound[0].Play();

            // 플레이어의가 왼쪽에 있다면
            if (pm.transform.position.x < transform.position.x)
            {
                // 왼쪽으로 이동
                dir = -1;
            }

            // 플레이어가 오른쪽에 있다면
            else if (pm.transform.position.x > transform.position.x)
            {
                // 오른쪽으로 이동
                dir = 1;
            }

            // 플레이어가 거리는 가까운데 위에 있을 때
            if (detectDisX < 8 && detectDisY > 2.2f)
            {
                // 근거리 공격 상태로 전환
                bState = BossState.CloseAttack;

                // 스턴 공격만 진행하는 상태 활성화
                OnlyStun = true;
            }

            // 플레이어가 더 가까워졌다면
            if (detectDisX <= 6 && detectDisY <= 2.2f)
            {
                // 공격 중인 상태
                isAttack = true;

                // 스턴 공격만 진행하는 상태 비활성화
                OnlyStun = false;

                // 근거리 공격 상태로 전환
                bState = BossState.CloseAttack;
            }
        }
    }

    // 근거리 공격 패턴 (할퀴기, 소리치기)
    void CloseAttack()
    {
        // 플레이어의 거리가 멀어졌다면
        if (detectDisX > 6 && detectDisY <= 2.2f)
        {
            // 공격 중이 아닌 상태
            isAttack = false;

            // 스턴 공격만 진행하는 상태 비활성화
            OnlyStun = false;

            // 이동 상태로 전환
            bState = BossState.Walk;
        }

        // 아래에 있던 플레이어가 위로 갔다면
        if (detectDisX < 8 && detectDisY > 2.2f)
        {
            // 스턴 공격만 진행하는 상태 비활성화
            OnlyStun = true;
        }

        // 위에 있던 플레이어가 내려 왔다면
        if (detectDisX < 8 && detectDisY <= 2.2f)
        {
            // 스턴 공격만 진행하는 상태 비활성화
            OnlyStun = false;
        }

        // 위에 있던 플레이어의 거리가 너무 멀어졌다면
        if (detectDisX > 8 && detectDisY > 2.2f)
        {
            // 스턴 공격만 진행하는 상태 비활성화
            OnlyStun = false;

            // 바위 던지기 상태로 전환
            bState = BossState.FarAttack;
        }

        // 정지
        dir = 0;

        // 근거리 공격 타이머 시작
        closeAttackTimer += Time.deltaTime;

        // 타이머가 3초가 지나면
        if (closeAttackTimer >= 3)
        {
            // 랜덤으로 공격 패턴 뽑기
            attackPattern = Random.Range(0, 2);

            // 스턴 공격만 진행하는 상태가 활성화 되어 있다면
            if (OnlyStun == true)
            {
                // 뽑는 숫자 1로 고정
                attackPattern = 1;
            }

            // 뽑은 숫자가 0이라면 할퀴기 공격
            if (attackPattern == 0)
            {
                // 할퀴기 애니메이션 재생
                anim.SetTrigger("scratch");

                // 타이머 초기화
                closeAttackTimer = 0;
            }

            // 뽑은 숫자가 1이라면 소리치기 공격
            else if (attackPattern == 1)
            {
                // 소리치기 애니메이션 재생
                anim.SetTrigger("scream");

                // 소리치기 효과음 재생
                SoundManager.instance.bosssound[2].Play();

                // 이때 플레이어가 공격범위 안에 있다면 공격
                if (detectDisX <= 8 && detectDisY <= 10f)
                {
                    // 공격
                    pm.hp -= 1;

                    // 체력 제한
                    pm.hp = Mathf.Clamp(pm.hp, 0, 30);

                    // 플레이어 체력 출력
                    um.PlayerHp(pm.hp);

                    // 플레이어의 체력이 0 이상이라면
                    if (pm.hp > 0)
                    {
                        // 플레이어 기절
                        isStun = true;
                        pm.enabled = false;
                        glid.enabled = false;
                        pf.enabled = false;

                        // 플레이어 컨트롤러 비활성화
                        playerController.SetActive(false);

                        // 플레이어 기절 애니메이션 재생
                        pm.anim.SetBool("isStun", isStun);
                    }
                }

                // 타이머 초기화
                closeAttackTimer = 0;
            }
        }
    }

    // 할퀴기 애니메이션에서 호출
    void Scratch()
    {
        // 할퀴기 효과음 재생
        SoundManager.instance.bosssound[1].Play();

        // 이때 플레이어가 공격범위 안에 있다면 공격
        if (detectDisX <= 6 && detectDisY <= 2.2f)
        {
            // 공격
            pm.hp -= 7;

            // 체력 제한
            pm.hp = Mathf.Clamp(pm.hp, 0, 30);

            // 플레이어 체력 출력
            um.PlayerHp(pm.hp);

            // 플레이어 피격 애니메이션 재생
            pm.anim.SetTrigger("damaged");
        }
    }

    // 피해 입었을 때
    void Damaged()
    {
        // 정지
        dir = 0;

        // 도토리 피해량 만큼 체력 감소
        hp -= pm.acornDamage;

        // 체력을 슬라이더에 출력
        hpBar.value = hp / 80f;

        // 피격 효과음 재생
        SoundManager.instance.enemysound[0].Play();

        // 공격 중인 상태라면
        if (isAttack)
        {
            // 공격 상태 유지
            bState = BossState.FarAttack;
        }

        // 보스 체력이 0이하라면
        if (hp <= 0)
        {
            // hp바 없애기
            hpBar.gameObject.SetActive(false);

            // 죽음 애니메이션 재생
            anim.SetTrigger("die");

            // 죽음 상태로 전환
            bState = BossState.Die;
        }

        // 공격 중이 아닐 때 + 체력이 남아 있을 때
        if (!isAttack && hp > 0)
        {
            // 피격 상태로 전환
            bState = BossState.Damaged;

            // 애니메이션 재생
            anim.SetTrigger("damaged");
        }
    }

    // 피격 애니메이션이 끝나면 호출
    void DamagedEnd()
    {
        // 기본 상태로 전환
        bState = BossState.Idle;
    }

    // 죽었을 때
    void Die()
    {
        // 배경음 정지
        for (int i = 0; i <= 6; i++)
        {
            SoundManager.instance.backgroundmusic[i].Stop();
        }

        // 정지
        dir = 0;

        // 보스 기능 정지
        enabled = false;

        // 리지드바디 비활성화
        rig.simulated = false;

        // 콜라이더 비활성화
        bc.enabled = false;

        // 죽음 효과음 재생
        SoundManager.instance.bosssound[4].Play();

        // 도착지점 활성화
        finish.SetActive(true);
    }
}
