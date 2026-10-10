using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    #region 선언부
    // 튜토리얼 매니저
    public TutorialManager tm;

    // 플레이어 매니저
    public PlayerManager pm;

    // 플레이어 공격
    public PlayerFire pf;

    // 스킬 매니저
    public SkillManager sm;

    // 튜토리얼 스킵 여부 창
    public GameObject TutorialSkipView;

    // 일시정지 버튼
    public GameObject pauseBtn;

    // 일시정지 창
    public GameObject PauseView;

    // 설정화면
    public GameObject SettingView;

    // 도토리 부족 문구
    public GameObject CantSelectTxt;

    // 치트 버튼
    public GameObject CheatBtn;

    // 치트 버튼 누른 횟수
    int click = 0;
    #endregion

    private void Update()
    {
        // 레벨이 0이상 4이하인 경우 + 뒤로가기
        if (GameManager.instance.level >= 0 && GameManager.instance.level <= 4 && Input.GetKeyDown(KeyCode.Escape))
        {
            // 효과음 재생
            SoundManager.instance.uisound[1].Play();

            // 일시정지
            Pause();
        }
    }

    #region 메인화면
    // 시작 버튼을 눌렀을 때 호출
    public void StartBtn()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 튜토리얼 스킵 여부 창 활성화
        TutorialSkipView.SetActive(true);
    }

    // 튜토리얼 스킵 하지 않을 경우 호출
    public void NoTutorialSkip()
    {
        // 레벨 업
        GameManager.instance.level++;

        // 인트로 씬 전환
        SceneManager.LoadScene("1. IntroScene");
    }

    // 튜토리얼 스킵할 경우 호출
    public void TutorialSkip()
    {
        // 레벨 3업
        GameManager.instance.level += 3;

        // 로딩화면 먼저 전환
        GameManager.instance.isLoading = true;

        // 1단계 씬 전환
        LoadingManager.LoadScene("3. Level_1");
    }

    // 튜토리얼 창 닫기 버튼 눌렀을 때 호출
    public void TutorialSkipViewClose()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 튜토리얼 스킵 창 닫힘
        TutorialSkipView.SetActive(false);
    }


    // 설정 버튼 눌렀을 때 호출
    public void SettingBtn()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 설정창 열림
        SettingView.SetActive(true);
    }

    // 설정 창 닫기 버튼 눌렀을 때 호출
    public void SettingClose()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 설정창 닫힘
        SettingView.SetActive(false);
    }

    // 종료 버튼을 눌렀을 때 호출
    public void QuitBtn()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 게임 종료
        Application.Quit();
    }
    #endregion

    #region 튜토리얼
    // 대화창에서 다음 버튼을 눌렀을 때 호출
   public void Next()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 다음 페이지
        tm.page++;
    }

    // 대화창에서 이전 버튼을 눌렀을 때 호출
    public void Prev()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 이전 페이지
        tm.page--;
    }

    // 대화창에서 닫기 버튼을 눌렀을 때 호출
    public void Close()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 대화창 비활성화
        tm.converView.SetActive(false);

        // 페이지 초기화
        tm.page = 1;

        // 대화 없는 상태
        tm.conState = TutorialManager.Conversation.Nothing;

        // 플레이어 기능 활성화
        pm.enabled = true;
        pf.enabled = true;

        // 게임 재개
        Time.timeScale = 1;
    }
    #endregion

    #region 일시정지
    // 뒤로가기하거나 일시정지 버튼 누르면 호출
    public void Pause()
    {
        // 효과음 재생
        SoundManager.instance.uisound[1].Play();

        // 일시정지 버튼 비활성화
        pauseBtn.SetActive(false);

        // 게임 멈추기
        Time.timeScale = 0;

        // 플레이어 기능 비활성화
        pm.enabled = false;
        pf.enabled = false;

        //일시정지 창 열기
        PauseView.SetActive(true);
    }

    // 재개 버튼을 누르면 호출
    public void Resum()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 일시정지 창 닫기
        PauseView.SetActive(false);

        // 일시정지 버튼 활성화
        pauseBtn.SetActive(true);

        // 플레이어 기능 활성화
        pm.enabled = true;
        pf.enabled = true;

        // 게임 속도 복구
        Time.timeScale = 1;
    }

    // 홈 버튼을 누르면 호출
    public void Home()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 게임 매니저에 저장된 값 초기화
        GameManager.instance._Reset();

        // 메인 화면으로 이동
        SceneManager.LoadScene(0);

        // 게임 속도 복구
        Time.timeScale = 1;
    }
    #endregion

    #region 스킬 선택지
    // 체력 회복 선택지
    public void FirstSelect()
    {
        // 효과음 재생
        SoundManager.instance.uisound[3].Play();

        // 도토리를 10개 이상 가지고 있다면
        if (GameManager.instance._ACORN >= 10)
        {
            // 체력을 10만큼 회복
            GameManager.instance._HP += 10;

            // 회복 제한
            GameManager.instance._HP = Mathf.Clamp(GameManager.instance._HP, 0, 30);

            // 비용 지불
            GameManager.instance._ACORN -= 10;

            // 다음 단계 씬 전환하기
            GameManager.instance.NextScene();
        }

        // 도토리가 충분하지 않다면
        else
        {
            // 효과음 재생
            SoundManager.instance.uisound[5].Play();

            // 도토리가 부족합니다 문구 출력
            CantSelectTxt.SetActive(true);
        }
    }

    // 두번째 선택지
    public void SecondSelect()
    {
        // 효과음 재생
        SoundManager.instance.uisound[4].Play();

        // 소지한 도토리가 비용보다 많다면
        if (GameManager.instance._ACORN >= sm.secondCostAcorn)
        {
            // 랜덤으로 뽑은 인덱스를 정수로 변환
            int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);

            // 랜덤으로 뽑은 인덱스에 따라 스킬 습득
            GameManager.instance.skill[index] = true;

            // 비용 지불
            GameManager.instance._ACORN -= sm.secondCostAcorn;

            // 다음 단계 씬 전환하기
            GameManager.instance.NextScene();
        }

        // 도토리가 충분하지 않다면
        else
        {
            // 효과음 재생
            SoundManager.instance.uisound[5].Play();

            // 도토리가 부족합니다 문구 출력
            CantSelectTxt.SetActive(true);
        }
    }

    // 세번째 선택지
    public void thirdSelect()
    {
        // 효과음 재생
        SoundManager.instance.uisound[4].Play();

        // 소지한 도토리가 비용보다 많다면
        if (GameManager.instance._ACORN >= sm.thirdCostAcorn)
        {
            // 랜덤으로 뽑은 인덱스를 정수로 변환
            int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);

            // 랜덤으로 뽑은 인덱스에 따라 스킬 습득
            GameManager.instance.skill[index] = true;

            // 비용 지불
            GameManager.instance._ACORN -= sm.thirdCostAcorn;

            // 다음 단계 씬 전환하기
            GameManager.instance.NextScene();
        }

        // 도토리가 충분하지 않다면
        else
        {
            // 효과음 재생
            SoundManager.instance.uisound[5].Play();

            // 도토리가 부족합니다 문구 출력
            CantSelectTxt.SetActive(true);
        }
    }

    // 선택하지 않음
    public void Nothing()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 다음 단계 씬 전환하기
        GameManager.instance.NextScene();
    }
    #endregion

    #region 게임 나가기
    // 보스와 전투 중 도토리가 다 떨어졌을 경우 나타나는 버튼
    public void Exit()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 배경음 정지
        for (int i = 0; i <= 6; i++)
        {
            SoundManager.instance.backgroundmusic[i].Stop();
        }

        // 게임 오버
        GameManager.instance.isFail = true;

        // 플레이어 정지
        pm.h = 0;
        pm.dir = 0;

        // 캐릭터 기능 비활성화
        pm.enabled = false;
        pf.enabled = false;
    }
    #endregion

    #region 엔딩화면
    // 재시작
    public void Restart()
    {
        // 효과음 재생
        SoundManager.instance.uisound[0].Play();

        // 게임 매니저에 저장된 값 초기화
        GameManager.instance._Reset();

        // 레벨은 1로 변경
        GameManager.instance.level = 1;

        // 로딩 중인 상태
        GameManager.instance.isLoading = true;

        // 1단계 씬 전환
        LoadingManager.LoadScene("3. Level_1");
    }
    #endregion

    #region 치트
    // 치트 버튼 누르면 호출
    public void Cheat()
    {
        // 버튼 누를 때 마다 1증가
        click++;

        // 5번 눌렀다면
        if (click == 9)
        {
            // 치트 활성화
            GameManager.instance.Cheat = true;

            // 모든 스킬 습득
            GameManager.instance.skill[0] = true;
            GameManager.instance.skill[1] = true;
            GameManager.instance.skill[2] = true;
            GameManager.instance.skill[3] = true;
            GameManager.instance.skill[4] = true;
            GameManager.instance.skill[5] = true;

            // 효과음 재생
            SoundManager.instance.uisound[3].Play();

            // 횟수 초기화
            click = 0;

            // 버튼 삭제
            Destroy(CheatBtn);
        }
    }
    #endregion
}
