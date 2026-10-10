using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{    
    // 로딩 다음에 나올 씬의 이름
    static string nextSceneName;

    // 에이싱크 오퍼레이션
    AsyncOperation op;

    // 스토리 창
    public GameObject storyView;

    // 레벨 2 스토리
    public GameObject level_2_story;

    // 레벨 4 스토리
    public GameObject level_4_story;

    // 채우는 바 이미지
    public Image progressBar;

    // 로딩 텍스트
    public GameObject loadingTxt;

    // 시간 저장할 변수
    float t;

    // 로딩씬을 부르면서 그 다음으로 보여줄 씬 이름을 전달받는 함수
    public static void LoadScene(string sceneName)
    {
        // 불러올 씬 이름 저장
        nextSceneName = sceneName;

        // 로딩 씬 불러오기
        SceneManager.LoadScene("9. LoadingScene");
    }

    void Start()
    {
        // 다음 맵이 2단계 맵이라면
        if (GameManager.instance.level == 2)
        {
            // 스토리 창 활성화
            storyView.SetActive(true);

            // 레벨 2 스토리 활성화
            level_2_story.SetActive(true);
        }

        // 다음 맵이 4단계 맵이라면
        else if (GameManager.instance.level == 4)
        {
            // 스토리 창 활성화
            storyView.SetActive(true);

            // 레벨 4 스토리 활성화
            level_4_story.SetActive(true);
        }

        // 레벨 2, 4를 제외한 나머지 레벨이라면
        else
        {
            // 스토리 창 비활성화
            storyView.SetActive(false);
        }

        // 로딩 바 활성화
        progressBar.gameObject.SetActive(true);

        // 로딩씬이 시작되자마자 씬 전환
        LoadNextScene();
    }

    void Update()
    {
        // 로딩이 완료되기 전에
        if (!op.isDone)
        {
            // 시간 카운트
            t += Time.deltaTime;

            // 2초동안 바를 채움
            progressBar.fillAmount = Mathf.Lerp(0, 1, t / 2);
        }

        // 페이크로딩까지 완료됐다면
        if (progressBar.fillAmount >= 1f)
        {
            // 로딩 바 비활성화
            progressBar.gameObject.SetActive(false);

            // 로딩 텍스트 활성화
            loadingTxt.SetActive(true);

            // 아무키나 누르면
            if (Input.anyKey)
            {
                // 로딩 텍스트 비활성화
                loadingTxt.SetActive(false);

                // 로딩 중이 아닌 상태
                GameManager.instance.isLoading = false;

                // 진짜로 씬 전환
                op.allowSceneActivation = true;
            }
        }
    }

    // 로딩 다음으로 나올 씬 불러오기
    void LoadNextScene()
    {
        // 씬을 불러오는 동안에 다른 행동을 할 수 있도록
        op = SceneManager.LoadSceneAsync(nextSceneName);

        // 씬이 불려와지자마자 넘어가지 못하게
        op.allowSceneActivation = false;
    }
}
