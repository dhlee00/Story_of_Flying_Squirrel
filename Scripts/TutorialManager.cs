using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    #region 선언부
    // 대화창
    public GameObject converView;

    // 대화창 스크립트
    public Text script;

    // 대화창 버튼
    public GameObject prev;
    public GameObject next;
    public GameObject close;

    // 대화창 페이지
    public int page;

    // 대화창에서의 캐릭터 이미지, 이름
    public Image _image;
    public Text _name;

    // 플레이어 상반신
    public Sprite bust;

    // 편지 스프라이트
    public Sprite letter;

    // 플레이어 매니저
    public PlayerManager pm;

    // 대화 목록
    public enum Conversation
    {
        GameStart,
        Glid,
        Acorn,
        Thorn,
        Enemy,
        Finish,
        Nothing
    }

    // 현재 대화 중인 목록
    public Conversation conState;

    // 현재 대화 중인 목록 변수
    public int nowConversation;
    #endregion

    private void Start()
    {
        // 시작하자마자 플레이어 기능 정지
        pm.enabled = false;

        // 시작하자마자 대화 시작
        conState = Conversation.GameStart;

        // 게임 정지
        Time.timeScale = 0;
    }

    private void Update()
    {
        switch(conState)
        {
            case Conversation.GameStart: GameStart(); break;
            case Conversation.Glid: Glid(); break;
            case Conversation.Acorn: Acorn(); break;
            case Conversation.Thorn: Thorn(); break;
            case Conversation.Enemy: Enemy(); break;
            case Conversation.Finish: Finish(); break;
            case Conversation.Nothing: break;
        }
    }

    // 게임 시작 시 대화
    void GameStart()
    {
        nowConversation = 1;

        // 페이지에 따라 다른 내용을 스크립트에 할당
        if (page == 1)
        {
            // 1페이지에서 이전 버튼 비활성화
            prev.SetActive(false);

            // 이미지와 이름에 플레이어 할당
            _image.sprite = bust;
            _name.text = "뇽";

            script.text = "어? 동동이가 어디 갔지?";
        }

        else if (page == 2)
        {
            // 2페이지 부터 이전 버튼 활성화
            prev.SetActive(true);

            // 이미지와 이름에 플레이어 할당
            _image.sprite = bust;
            _name.text = "뇽";

            script.text = "응? 이게 뭐야?\n\n(편지를 발견한다.)";
        }

        else if (page == 3)
        {
            // 이미지와 이름에 편지 할당
            _image.sprite = letter;
            _name.text = "동동의 편지";

            script.text = "뇽아\n\n난 더의상 너와 가치 살고 십지 안아.\n\n난 멀리 떠날개 잘지네.";
        }

        else if (page == 4)
        {
            // 이미지와 이름에 플레이어 할당
            _image.sprite = bust;
            _name.text = "뇽";

            script.text = "흠... 글씨체며... 말투며... 게다가 맞춤법까지...\n이건 절대 동동의 편지가 아니야.";
        }

        else if (page == 5)
        {
            // 이미지와 이름에 플레이어 할당
            _image.sprite = bust;
            _name.text = "뇽";

            script.text = "동동의 흔적을 찾아보자!";
        }

        else if (page == 6)
        {
            // 6페이지 까지 다음 버튼 활성화
            next.SetActive(true);

            script.text = "왼쪽 아래에 좌, 우 버튼을 눌러서 이동할 수 있어!\n그리고 오른쪽 아래에 점프 버튼을 누르면 점프를 할 수 있어!";
        }

        else if (page == 7)
        {
            // 7페이지에서 다음 버튼 비활성화
            next.SetActive(false);

            script.text = "(게임 플레이 도중 멈추고 싶으면 상단에 일시정지 버튼을 누르거나 뒤로가기를 하세요.)";
        }
    }

    // 글라이딩 대화
    void Glid()
    {
        nowConversation = 2;

        // 페이지에 따라 다른 내용을 스크립트에 할당
        if (page == 1)
        {
            // 1페이지에서 이전 버튼 비활성화
            prev.SetActive(false);

            // 이미지와 이름에 플레이어 할당
            _image.sprite = bust;
            _name.text = "뇽";

            script.text = "어... 여기는 너무 높아서 점프로는 못 올라가겠네...";
        }

        else if (page == 2)
        {
            // 2페이지에서 모든 버튼 활성화
            prev.SetActive(true);
            next.SetActive(true);

            script.text = "좋아 날아서 가자!";
        }

        else if (page == 3)
        {
            // 3페이지에서 다음 버튼 비활성화
            next.SetActive(false);

            script.text = "점프 중인 상태에서 점프 버튼 위치에 있는 글라이딩 버튼을 꾹 누르고 있으면 글라이딩을 할 수 있어!\n\n글라이딩 중에 버튼에서 손을 떼면 글라이딩을 바로 멈출 수 있어!";
        }
    }

    // 도토리 대화
    void Acorn()
    {
        nowConversation = 3;

        // 페이지에 따라 다른 내용을 스크립트에 할당
        if (page == 1)
        {
            // 1페이지에서 이전 버튼 비활성화
            prev.SetActive(false);

            // 이미지와 이름에 플레이어 할당
            _image.sprite = bust;
            _name.text = "뇽";

            script.text = "오! 도토리다!";
        }

        else if (page == 2)
        {
            // 2페이지에서 모든 버튼 활성화
            prev.SetActive(true);
            next.SetActive(true);

            script.text = "나중에 쓸 데가 있을 수도 있으니까 모아놓아야겠다.";
        }

        else if (page == 3)
        {
            // 3페이지에서 다음 버튼 비활성화
            next.SetActive(false);

            script.text = "도토리에 닿으면 도토리를 얻을 수 있어!\n\n(도토리는 무기이자 재화로 사용됩니다.)";
        }
    }

    // 장애물 대화
    void Thorn()
    {
        nowConversation = 4;

        if (page == 1)
        {
            // 1페이지에서 이전 버튼 비활성화, 다음 버튼 활성화
            prev.SetActive(false);
            next.SetActive(true);

            script.text = "어...가시덤불이네...";
        }

        else if (page == 2)
        {
            // 2페이지에서 이전 버튼 활성화, 다음 버튼 비활성화
            prev.SetActive(true);
            next.SetActive(false);

            script.text = "닿으면 아프겠다... 조심해서 피해가야지\n\n(가시덤불과 닿으면 체력이 1만큼 닳습니다.)";
        }
    }

    // 적 대화
    void Enemy()
    {
        nowConversation = 5;

        if (page == 1)
        {
            // 1페이지에서 이전 버튼 비활성화
            prev.SetActive(false);

            script.text = "....여우다....";
        }

        else if (page == 2)
        {
            // 2페이지 부터 이전 버튼 활성화
            prev.SetActive(true);

            script.text = "마주치면 내 도토리 뺏을텐데....";
        }

        else if (page == 3)
        {
            script.text = "좋아! 맞서 싸우자!";
        }

        else if (page == 4)
        {
            script.text = "(오른쪽 아래에 공격 버튼을 누르면 도토리를 던져서 공격합니다.)";
        }

        else if (page == 5)
        {
            // 5페이지까지 다음 버튼 활성화
            next.SetActive(true);

            script.text = "(여우는 평소엔 가만히 서 있지만 일정 거리 이상 가까워지면 쫓아옵니다.\n더 가까워졌을 땐 공격을 하고 이때 도토리를 한 개 빼앗깁니다.)";
        }

        else if (page == 6)
        {
            // 6페이지에서 다음 버튼 비활성화
            next.SetActive(false);

            script.text = "(여우는 꼭 쓰러트리고 지나갈 필요 없이 피해서 지나가도 되지만 여우를 쓰러트리면 도토리를 얻을 수 있습니다.)";
        }
    }

    // 도착지점 대화
    void Finish()
    {
        if (page == 1)
        {
            // 1페이지에서 이전 버튼 비활성화
            prev.SetActive(false);

            script.text = "응?\n\n(바닥에서 큰 발자국과 뇽이 동동에게 선물해줬던 도토리를 발견한다.)";
        }

        else if (page == 2)
        {
            // 2페이지 부터 이전 버튼 활성화
            prev.SetActive(true);

            script.text = "아니... 이건...! 내가 동동한테 선물했던 도토리잖아?!";
        }

        else if (page == 3)
        {
            script.text = "그리고 이 정도 크기의 발자국은... 틀림없어 분명 흑곰의 발자국이야...";
        }

        else if (page == 4)
        {
            // 4페이지까지 다음 버튼 활성화
            next.SetActive(true);

            script.text = "흑곰 이 자식이 동동을 납치한 거였어!";
        }

        else if (page == 5)
        {
            // 5페이지에서 다음 버튼 비활성화
            next.SetActive(false);

            script.text = "동동아!! 내가 구해줄게!!!!";
        }
    }
}
