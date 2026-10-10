using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 선언부
    // 스태틱화
    public static GameManager instance;

    // 플레이어의 체력을 저장할 변수
    public int _HP;

    // 플레이어의 소지 도토리 수를 저장할 변수
    public int _ACORN;

    // 레벨
    public int level;

    // 스킬 습득 상태
    public bool[] skill = new bool[6];

    // 로딩 중 여부
    public bool isLoading;

    // 클리어 성공
    public bool isSuccess;

    // 클리어 실패
    public bool isFail;

    // 치트
    public bool Cheat;
    #endregion

    private void Awake()
    {
        // 인스턴스에 값이 없으면 자기자신 할당
        if (instance == null)
        {
            instance = this;
        }

        // 인스턴스에 값이 있으면 삭제
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void NextScene()
    {
        if (level == 1)
        {
            LoadingManager.LoadScene("3. Level_1");
        }

        else if (level == 2)
        {
            LoadingManager.LoadScene("4. Level_2");
        }

        else if (level == 3)
        {
            LoadingManager.LoadScene("5. Level_3");
        }

        else if (level == 4)
        {
            LoadingManager.LoadScene("6. Boss");
        }

        else if (level == 5)
        {
            LoadingManager.LoadScene("7. EndingScene(S)");
        }
    }

    // 초기화
    public void _Reset()
    {
        _HP = 30;

        _ACORN = 0;

        level = -2;

        for (int i = 0; i < 6; i++)
        {
            skill[i] = false;
        }

        isLoading = false;

        isSuccess = false;

        isFail = false;

        Cheat = false;
    }
}
