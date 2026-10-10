using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    // 타이핑 매니저
    public TypingManager tm;

    void Update()
    {
        // 만약 스크립트가 마지막 스크립트라면
        if (tm.scriptIndex == tm.storyScript.Length)
        {
            // 레벨업
            GameManager.instance.level++;

            // 다음 씬 전환
            SceneManager.LoadScene("2. TutorialScene");
        }
    }
}
