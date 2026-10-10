using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    void Start()
    {
        // 이전 배경음 정지
        for (int i = 0; i <= 6; i++)
        {
            SoundManager.instance.backgroundmusic[i].Stop();
        }

        // 씬에 따라 배경음악 재생
        // 메인화면
        if (GameManager.instance.level == -2)
        {
            // 배경음 재생
            SoundManager.instance.backgroundmusic[0].Play();
        }

        // 인트로화면
        else if (GameManager.instance.level == -1)
        {
            // 배경음 재생
            SoundManager.instance.backgroundmusic[1].Play();
        }

        // 튜토리얼 ~ 레벨 3화면
        else if (GameManager.instance.isSuccess == false
              && GameManager.instance.isFail == false
              && GameManager.instance.isLoading == false
              && GameManager.instance.level >= 0
              && GameManager.instance.level <= 3)
        {
            // 배경음 재생
            SoundManager.instance.backgroundmusic[2].Play();
        }

        // 보스 스테이지
        else if (GameManager.instance.isSuccess == false
              && GameManager.instance.isFail == false
              && GameManager.instance.isLoading == false
              && GameManager.instance.level == 4)
        {
            // 배경음 재생
            SoundManager.instance.backgroundmusic[3].Play();
        }

        // 성공엔딩
        else if (GameManager.instance.isSuccess == true)
        {
            // 배경음 재생
            SoundManager.instance.backgroundmusic[4].Play();
        }

        // 실패엔딩
        else if (GameManager.instance.isFail == true)
        {
            // 배경음 재생
            SoundManager.instance.backgroundmusic[5].Play();
        }

        // 로딩화면
        else if (GameManager.instance.isLoading == true)
        {
            // 배경음 재생
            SoundManager.instance.backgroundmusic[6].Play();
        }
    }
}
