using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    #region 선언부
    // 리지드바디
    Rigidbody2D rig;

    // 플랫폼 이펙트
    PlatformEffector2D pe;

    // 글라이딩 매니저
    GlidingManager glid;

    // 플레이어 파이어
    PlayerFire pf;

    // UI 텍스트 매니저
    public UIManager um;

    // 스킬 선택 창
    public GameObject skillView;

    // 이동 방향 받아올 변수
    public float h;

    // 플레이어 좌, 우 방향
    public int dir = 1;

    // 플레이어 체력
    public int hp;

    // 소지한 도토리 수
    public int acorn;

    // 도토리 한개당 피해량
    public int acornDamage;

    // 캐릭터 이동 속도
    public float speed;

    // 캐릭터 점프하는 힘
    public float jumpPower;

    // 레이가 감지할 오브젝트
    public LayerMask mask;

    // 레이 최대 범위
    public float maxRange;

    // 레이에 닿은 물체 저장
    public RaycastHit2D Hit;

    // 레이 위치
    public float boxX;
    public float boxY;

    // 애니메이터
    public Animator anim;

    // 공격 버튼 상태
    public bool attackOn;

    // 글라이팅 버튼 상태
    public bool glidOn;

    // 점프 버튼
    public GameObject JumpBtn;

    // 글라이딩 버튼
    public GameObject glidBtn;
    #endregion

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        pe = GetComponent<PlatformEffector2D>();
        glid = GetComponent<GlidingManager>();
        pf = GetComponent<PlayerFire>();
        anim = GetComponent<Animator>();

        // 게임 매니저에 저장된 체력과 도토리 수 할당
        hp = GameManager.instance._HP;
        acorn = GameManager.instance._ACORN;

        // UI에 출력
        um.PlayerHp(hp);
        um.PlayerAcorn(acorn);

        // 스킬 1 활성화 시
        if (GameManager.instance.skill[0])
        {
            // 도토리 한개당 피해량 3으로 증가
            acornDamage = 3;
        }

        // 스킬 6 활성화 시
        if (GameManager.instance.skill[5])
        {
            // 이동속도, 점프력 증가
            speed += speed / 2;
            jumpPower = 26;
        }
    }

    void Update()
    {
        // 체력이 0 이하라면
        if (hp <= 0)
        {
            // 게임 클리어 실패
            GameManager.instance.isFail = true;

            // 배경음 정지
            for (int i = 0; i <= 6; i++)
            {
                SoundManager.instance.backgroundmusic[i].Stop();
            }

            // 콜라이더가 타일이랑만 충돌
            pe.colliderMask = 128;

            // 정지
            h = 0;
            dir = 0;

            // 캐릭터 기능 비활성화
            enabled = false;
            pf.enabled = false;
            
            // 죽음 애니메이션 재생
            anim.SetTrigger("die");

            // 죽음 효과음 재생
            SoundManager.instance.playersound[5].Play();
        }

        // 이동 애니메이션 재생
        anim.SetInteger("walk", (int)h);

        // 좌, 우로 이동 + 글라이딩 중이 아닐 때
        if (!glid.isGliding)
        {
            // 이동
            rig.velocity = new Vector2(h * speed, rig.velocity.y);
        }

        // 점프 애니메이션 재생
        anim.SetInteger("jump", (int)rig.velocity.y);

        // 착지 애니메이션 재생
        anim.SetBool("landing", Hit);

        // 오른쪽으로 이동할 때
        if (h > 0)
        {
            // 오른쪽 보기
            dir = 1;

            transform.localScale = new Vector3(0.55f, 0.55f, 1);
        }

        // 왼쪽으로 이동할 때
        else if (h < 0)
        {
            // 왼쪽 보기
            dir = -1;

            transform.localScale = new Vector3(-0.55f, 0.55f, 1);
        }

        // 바닥으로 레이 쏘기
        Hit = Physics2D.BoxCast(transform.position + new Vector3(boxX * dir, boxY, 0), new Vector2(1.1f, maxRange), 0, Vector2.down, maxRange, mask);

        // 바닥에 있을 때 = 착지
        if (Hit)
        {
            // 글라이딩 중 아닌 상태
            glid.isGliding = false;

            // 글라이딩 버튼 안눌림
            glidOn = false;

            // 공격 가능
            pf.enabled = true;

            // 글라이딩 가능
            glid.enabled = true;

            // 글라이딩 버튼 비활성화
            glidBtn.SetActive(false);

            // 점프 버튼 활성화
            JumpBtn.SetActive(true);
        }

        // 점프 중일 때
        else if (!Hit)
        {
            // 점프 버튼 비활성화
            JumpBtn.SetActive(false);

            // 글라이딩 버튼 활성화
            glidBtn.SetActive(true);
        }
    }

    #region 플레이어 조작
    // 이동하지 않음
    public void Stop()
    {
        h = 0;
    }

    // 왼쪽으로 이동
    public void MoveLeft()
    {
        h = -1;
    }

    // 오른쪽으로 이동
    public void MoveRight()
    {
        h = 1;
    }

    // 점프
    public void Jump()
    {
        // 바닥에 있을 때
        if (Hit)
        {
            // 점프 효과음 재생
            SoundManager.instance.playersound[0].Play();

            // 위로 힘을 가하기 (점프)
            rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    // 글라이딩 버튼 누름
    public void EnterGliding()
    {
        glidOn = true;
    }

    // 글라이딩 버튼 뗌
    public void ExitGliding()
    {
        glidOn = false;
    }

    // 공격
    public void Attack()
    {
        // 공격 버튼 누름
        attackOn = true;
    }
    #endregion

    // 죽음 애니메이션에서 호출
    void DieEnd()
    {
        // 클리어 실패
        GameManager.instance.isFail = true;
    }

    private void OnDrawGizmos()
    {
        // 검은색
        Gizmos.color = Color.red;

        // 레이 기즈모 그리기
        Gizmos.DrawWireCube(transform.position + new Vector3(boxX * dir, boxY, 0), new Vector2(1.1f, maxRange));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            // 바위와 닿았다면
            case "Rock":

                // 체력 감소
                hp -= 3;

                // 체력 제한
                hp = Mathf.Clamp(hp, 0, 30);

                // 체력 출력
                um.PlayerHp(hp);

                // 피격 애니메이션 재생
                anim.SetTrigger("damaged");

                // 피격 효과음 재생
                SoundManager.instance.playersound[4].Play();

                // 글라이딩 중이었다면
                if (glid.isGliding)
                {
                    // 글라이딩 중지
                    glid.enabled = false;
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            // 도토리와 닿았다면
            case "Acorn":

                // 도토리 증가
                acorn++;

                // 도토리 개수 제한
                acorn = Mathf.Clamp(acorn, 0, 99);

                // 소지 도토리 수 출력
                um.PlayerAcorn(acorn);

                // 도토리 획득 효과음 재생
                SoundManager.instance.playersound[2].Play();

                // 먹은 도토리 삭제
                collision.SendMessage("Destroy");
                break;

            // 가시덤불에 닿았다면
            case "Thorn":

                // 체력 감소
                hp -= 1;

                // 레벨이 0이라면 (튜토리얼이라면)
                if (GameManager.instance.level == 0)
                {
                    // 1에서 감소하지 않음
                    hp = Mathf.Clamp(hp, 1, 30);
                }

                // 체력 제한
                hp = Mathf.Clamp(hp, 0, 30);

                // 체력 출력
                um.PlayerHp(hp);

                // 피격 애니메이션 재생
                anim.SetTrigger("damaged");

                // 피격 효과음 재생
                SoundManager.instance.playersound[4].Play();

                break;

            // 도착 지점에 닿았다면
            case "Finish":

                // 로딩 중
                GameManager.instance.isLoading = true;

                // 레벨업
                GameManager.instance.level++;

                // 레벨이 1이라면
                if (GameManager.instance.level == 1)
                {
                    // 다음 씬 전환
                    GameManager.instance.NextScene();
                }

                // 레벨이 2이상 4이하면
                else if (GameManager.instance.level >= 2 && GameManager.instance.level <= 4)
                {
                    // 체력, 소지 도토리 수 게임 매니저에 저장
                    GameManager.instance._HP = hp;
                    GameManager.instance._ACORN = acorn;

                    // 정지
                    h = 0;
                    dir = 0;

                    // 이동 애니메이션 정지
                    anim.SetInteger("walk", (int)h);

                    // 캐릭터 기능 비활성화
                    enabled = false;
                    pf.enabled = false;

                    // 치트가 활성화 된 경우
                    if (GameManager.instance.Cheat == true)
                    {
                        // 스킬 창 비활성화
                        skillView.SetActive(false);

                        // 체력 최대치까지 회복
                        GameManager.instance._HP = 30;

                        // 바로 다음 씬 전환
                        GameManager.instance.NextScene();
                    }

                    // 치트가 비활성인 경우
                    else if (GameManager.instance.Cheat == false)
                    {
                        // 스킬 창 활성화
                        skillView.SetActive(true);

                        // 효과음 재생
                        SoundManager.instance.uisound[2].Play();
                    }
                }

                // 레벨이 5이상이라면
                else if (GameManager.instance.level > 4)
                {
                    // 클리어 성공
                    GameManager.instance.isSuccess = true;

                    // 정지
                    h = 0;
                    dir = 0;

                    // 이동 애니메이션 정지
                    anim.SetInteger("walk", (int)h);

                    // 캐릭터 기능 비활성화
                    enabled = false;
                    pf.enabled = false;
                }

                break;
        }
    }
}
