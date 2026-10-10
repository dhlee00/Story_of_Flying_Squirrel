using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneManager : MonoBehaviour
{
    // 타이핑 매니저
    public TypingManager tm;

    // 엔딩 스토리 스크립트
    public GameObject storyScript;

    // 재시작, 처음으로 버튼
    public GameObject restartBtn;
    public GameObject homeBtn;

    void Update()
    {
        // 스토리 타이핑이 완료되면
        if (tm.endTyping)
        {
            // 홈버튼 활성화
            homeBtn.SetActive(true);

            // 재시작 버튼 활성화
            restartBtn.SetActive(true);
        }
    }
}
