using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // 출력할 UI
    public Slider playerHpBar;
    public Text playerHpTxt;
    public Text playerAcornTxt;

    // 검은 화면
    public Image darkScreen;

    // 게임 나가기 화면
    public GameObject exitView;

    // 타이머
    float timer;

    private void Update()
    {
        // 게임 클리어 성공 or 실패 했다면
        if (GameManager.instance.isSuccess || GameManager.instance.isFail)
        {
            // 암전
            DarkChange();

            // 게임 나가기 창 비활성화
            exitView.SetActive(false);
        }
    }

    // 플레이어 체력 출력
    public void PlayerHp(float hp)
    {
        playerHpBar.value = hp / 30f;
        playerHpTxt.text = $"{hp} / 30";
    }

    // 플레이어 소지 도토리 개수 출력
    public void PlayerAcorn(int acorn)
    {
        playerAcornTxt.text = $"× {acorn}";
    }

    // 암전
    public void DarkChange()
    {
        // 검은 화면 활성화
        darkScreen.gameObject.SetActive(true);

        // 타이머 시작
        timer += Time.deltaTime;

        // 서서히 사라지도록 설정한 시간동안 투명도 올리기
        darkScreen.color = Vector4.Lerp(new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 1), timer / 3f);

        // 투명도가 1이 되면 씬 전환
        if (darkScreen.color.a >= 1)
        {
            // 성공 했다면
            if (GameManager.instance.isSuccess)
            {
                // 성공 엔딩 씬 전환
                SceneManager.LoadScene("7. EndingScene(S)");
            }

            // 실패 했다면
            else if (GameManager.instance.isFail)
            {
                // 실패 엔딩 씬 전환
                SceneManager.LoadScene("8. EndingScene(F)");
            }
        }
    }
}
