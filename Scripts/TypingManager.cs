using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    // 출력 할 텍스트
    Text printTxt;

    // 스토리 스크립트
    [TextArea]
    public string[] storyScript;

    // 스크립트 인덱스
    public int scriptIndex;

    // 타이핑 속도
    public float typingSpeed;

    // 타이핑 끝 상태
    public bool endTyping;

    // 도토리 아이콘
    public GameObject NextScript;

    void Start()
    {
        printTxt = GetComponent<Text>();

        // 처음엔 0을 대입
        scriptIndex = 0;

        // 타이핑 시작
        StartCoroutine(TypingScript());
    }

    void Update()
    {
        // 타이핑이 끝나면 
        if (endTyping)
        {
            // 타이핑 멈춤
            StopCoroutine(TypingScript());

            // 도토리 아이콘 활성화
            NextScript.SetActive(true);

            // 이때 아무 키를 누르면
            if (Input.anyKey)
            {
                // 타이핑 상태 초기화
                endTyping = false;

                // 도토리 아이콘 비활성화
                NextScript.SetActive(false);

                // 다음 스크립트 불러오기
                scriptIndex++;

                // 타이핑 다시 시작
                StartCoroutine(TypingScript());
            }
        }
    }

    // 타이핑 효과
    IEnumerator TypingScript()
    {
        // 타이핑 할 스크립트의 길이만큼 반복
        for (int i = 0; i <= storyScript[scriptIndex].Length; i++)
        {
            // 한 글자씩 출력
            printTxt.text = storyScript[scriptIndex].Substring(0, i);

            // 딜레이
            yield return new WaitForSeconds(typingSpeed);
        }

        // 타이핑 끝난 상태
        endTyping = true;
    }
}
