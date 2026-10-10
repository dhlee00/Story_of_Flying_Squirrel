using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    #region 선언부
    // 리지드 바디
    Rigidbody2D rig;

    // 콜라이더
    BoxCollider2D bc;

    // 스프라이트 랜더러
    SpriteRenderer sr;

    // 플레이어 매니저
    public PlayerManager pm;

    // UI 텍스트 매니저
    public UIManager um;

    // 체력바 슬라이더
    public Slider hpBar;

    // 애니메이터
    Animator anim;

    // 적 체력
    public int hp;

    // 적 이동 속도
    public float speed;

    // 이동 방향
    public int dir;

    // 플레이어 X값 감지 변수
    float detectDisX;

    // 플레이어 Y값 감지 변수
    float detectDisY;

    // 공격 중
    bool isAttack = false;

    // 공격 쿨타임
    float attackCoolTime;

    // 레이
    RaycastHit2D hit;

    // 기즈모용 변수
    int gizmos;

    // 레이가 감지할 오브젝트
    public LayerMask mask;

    // 적 행동패턴
    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Damaged,
        Die
    }

    // 적의 상태
    public EnemyState eState;

    // 죽고 나서 사용할 타이머
    float timer;

    // 도토리를 떨어트릴 개수를 저장할 변수
    int dropAcorn;
    #endregion

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // 시작하자마자 기본 상태
        eState = EnemyState.Idle;

        // 떨어트릴 도토리 개수 정하기
        dropAcorn = Random.Range(1, 6);
    }

    private void Update()
    {
        // 플레이어 X값 감지
        detectDisX = Mathf.Abs(pm.transform.position.x - transform.position.x);

        // 플레이어 Y값 감지
        detectDisY = Mathf.Abs(pm.transform.position.y - transform.position.y);

        // 이동
        rig.velocity = new Vector2(dir * speed, rig.velocity.y);

        // 이동 애니메이션
        anim.SetInteger("walk", dir);

        // 오른쪽으로 이동하고 있다면
        if (dir > 0)
        {
            // 오른쪽을 바라보게
            transform.localScale = new Vector3(-0.45f, 0.45f, 1);

            // HP바를 항상 고정되게
            hpBar.transform.localScale = new Vector3(-0.02f, 0.02f, 1);

            // 기즈모용 변수에 할당
            gizmos = 1;
        }

        // 왼쪽으로 이동하고 있다면
        else if (dir < 0)
        {
            // 왼쪽을 바라보게
            transform.localScale = new Vector3(0.45f, 0.45f, 1);

            // HP바를 항상 고정되게
            hpBar.transform.localScale = new Vector3(0.02f, 0.02f, 1);

            // 기즈모용 변수에 할당
            gizmos = -1;
        }

        // 레이 hit의 기즈모 그리기
        Debug.DrawRay(transform.position + new Vector3(gizmos * 0.9f, 0, 0), Vector2.down * 1.8f, Color.red);

        // 적의 위치보다 앞에서 시작해서 아래쪽으로 발사되는 레이에 맞은 물체 저장
        hit = Physics2D.Raycast(transform.position + new Vector3(gizmos * 0.9f, 0, 0), Vector2.down, 1.8f, mask);

        // 적의 상태에 따라 다른 함수 호출
        switch (eState)
        {
            case EnemyState.Idle: Idle(); break;
            case EnemyState.Walk: Walk(); break;
            case EnemyState.Attack: Attack(); break;
            case EnemyState.Damaged: break;
            case EnemyState.Die: Die(); break;
        }
    }

    // 기본 상태
    void Idle()
    {
        // 방향 없음
        dir = 0;

        // 감지했다면 + hit이 땅에 닿았을 때
        if (detectDisX <= 13 && detectDisY <= 3 && hit)
        {
            // 이동 상태로 전환
            eState = EnemyState.Walk;
        }

        // 감지는 되는데 낭떨어지가 왼쪽에 있고 플레이어는 오른쪽에 있을 때
        else if (detectDisX <= 13 && detectDisY <= 3 && hit.point.x == 0 && gizmos == -1 && pm.transform.position.x > transform.position.x)
        {
            // 이동 상태로 전환
            eState = EnemyState.Walk;
        }

        // 감지는 되는데 낭떨어지가 오른쪽에 있고 플레이어는 왼쪽에 있을 때
        else if (detectDisX <= 13 && detectDisY <= 3 && hit.point.x == 0 && gizmos == 1 && pm.transform.position.x < transform.position.x)
        {
            // 이동 상태로 전환
            eState = EnemyState.Walk;
        }

        else
        {
            // 공격 상태 유지
            eState = EnemyState.Idle;
        }
    }

    // 이동
    void Walk()
    {
        // 플레이어가 왼쪽에 있다면
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

        // 레이에 맞은 대상이 있을 때
        if (hit)
        {
            // 플레이어가 공격 범위 안에 들어왔다면
            if (detectDisX <= 2.5f && detectDisY <= 0.4f && detectDisY >= 0.2f)
            {
                // 공격 애니메이션 재생
                anim.SetTrigger("steal");

                // 공격 상태로 전환
                eState = EnemyState.Attack;

                // 공격 중인 상태
                isAttack = true;
            }

            // 감지 중이 아니라면
            if (detectDisX > 13 || detectDisY > 3)
            {
                // 기본 상태로 전환
                eState = EnemyState.Idle;
            }
        }

        // 레이에 맞은 대상이 없을 때
        else if (!hit)
        {
            eState = EnemyState.Idle;
        }
    }

    // 공격 상태
    void Attack()
    {
        // 플레이어가 공격 범위를 벗어났다면
        if (detectDisX > 2.5f || detectDisY > 0.4f || detectDisY < 0.2f)
        {
            // 쿨타임 초기화
            attackCoolTime = 0;

            // 이동 상태로 전환
            eState = EnemyState.Walk;

            // 공격 중이 아닌 상태
            isAttack = false;
        }

        // 정지
        dir = 0;

        // 공격 쿨타임 시작
        attackCoolTime += Time.deltaTime;

        // 쿨타임이 1초 이상이라면
        if (attackCoolTime >= 1)
        {
            // 공격 애니메이션 재생
            anim.SetTrigger("steal");

            // 쿨타임 초기화
            attackCoolTime = 0;
        }
    }

    // 공격 애니메이션에서 호출
    void Steal()
    {
        // 공격할 때 플레이어가 범위 안에 있다면
        if (detectDisX <= 2.5f && detectDisY <= 0.4f && detectDisY >= 0.2f)
        {
            // 공격 (도토리 훔치기)
            pm.acorn--;

            // 도토리 개수 제한
            pm.acorn = Mathf.Clamp(pm.acorn, 0, 99);

            // 소지 도토리 수 출력
            um.PlayerAcorn(pm.acorn);

            // 플레이어 강탈 당한 애니메이션 재생
            pm.anim.SetTrigger("robbed");
        }
    }

    void AttackSound()
    {
        // 공격 효과음 재생
        SoundManager.instance.foxsound[0].Play();
    }

    // 피격 상태
    void Damaged()
    {
        // 정지
        dir = 0;

        // 도토리 피해량 만큼 체력 감소
        hp -= pm.acornDamage;

        // 체력을 슬라이더에 출력
        hpBar.value = hp / 10f;

        // 체력이 없다면
        if (hp <= 0)
        {
            // 체력바 없애기
            hpBar.gameObject.SetActive(false);

            // 죽음 애니메이션 재생
            anim.SetTrigger("die");

            // 죽음 상태 전환
            eState = EnemyState.Die;
        }

        // 체력이 남았다면
        else
        {
            // 공격 중이 아니라면
            if (!isAttack)
            {
                // 피격 상태로 전환
                eState = EnemyState.Damaged;

                // 피격 애니메이션 재생
                anim.SetTrigger("damaged");

                // 피격 소리 재생
                SoundManager.instance.enemysound[0].Play();
            }

            // 공격 중이라면
            else
            {
                // 공격 상태 유지
                eState = EnemyState.Attack;
            }
        }
    }

    // 애니메이션에서 호출
    void DamagedEnd()
    {
        // 기본 상태로 전환
        eState = EnemyState.Idle;
    }

    // 죽음 상태
    void Die()
    {
        // 정지
        dir = 0;

        // 적 기능 정지
        enabled = false;

        // 리지드바디 비활성화
        rig.simulated = false;

        // 콜라이더 비활성화
        bc.enabled = false;

        // 죽음 효과음 재생
        SoundManager.instance.foxsound[1].Play();
    }

    // 서서히 사라지기 (애니메이션에서 호출)
    IEnumerator Destroy()
    {
        // 투명도가 0이 아닐 동안 반복
        while (sr.color.a != 0)
        {
            // 타이머 시작
            timer += Time.deltaTime;

            // 서서히 사라지도록 설정한 시간동안 투명도 낮추기
            sr.color = Vector4.Lerp(Color.white, new Vector4(1, 1, 1, 0), timer);

            // 현재 프레임이 끝날 때 까지 기다린다
            yield return null;
        }

        // 생성될 때 정한 떨어트릴 도토리 개수만큼 반복
        for (int i = 0; i <= dropAcorn; i++)
        {
            // 정한 도토리 개수만큼 약간 옆으로 한개씩 드랍
            GameObject dropAcorn = Instantiate(Resources.Load("Acorn"), transform.position + new Vector3(i / 2f, -0.8f, 0), Quaternion.identity) as GameObject;
        }

        // 적 삭제
        Destroy(gameObject);
    }
}
