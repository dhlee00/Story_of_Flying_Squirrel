using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region 선언부
    // 스태틱화
    public static SoundManager instance;

    // 배경음악
    public AudioSource[] backgroundmusic;

    // UI효과음
    public AudioSource[] uisound;

    // 플레이어
    public AudioSource[] playersound;

    // 적
    public AudioSource[] enemysound;

    // 여우
    public AudioSource[] foxsound;

    // 보스
    public AudioSource[] bosssound;
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
}
