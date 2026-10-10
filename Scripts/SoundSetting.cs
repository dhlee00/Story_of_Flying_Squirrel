using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    #region 선언부
    // 배경음 설정 슬라이더
    public Slider BGMSetting;

    // 효과음 설정 슬라이더
    public Slider SESetting;

    // 배경음 퍼센티지 텍스트
    public Text BGMPercent;

    // 효과음 퍼센티지 텍스트
    public Text SEPercent;
    #endregion

    void Start()
    {
        // 씬이 시작될 때 사운드 매니저에 할당한 볼륨 값 설정창에 불러오기
        for (int i = 0; i <= 6; i++)
        {
            BGMSetting.value = SoundManager.instance.backgroundmusic[i].volume;
        }

        for (int i = 0; i <= 5; i++)
        {
            SESetting.value = SoundManager.instance.uisound[i].volume;
            SESetting.value = SoundManager.instance.playersound[i].volume;
        }

        SESetting.value = SoundManager.instance.enemysound[0].volume;

        SESetting.value = SoundManager.instance.foxsound[0].volume;
        SESetting.value = SoundManager.instance.foxsound[1].volume;

        for (int i = 0; i <= 4; i++)
        {
            SESetting.value = SoundManager.instance.bosssound[i].volume;
        }

    }

    void Update()
    {
        // 슬라이더 값 퍼센티지로 출력
        BGMPercent.text = $"{Mathf.FloorToInt(BGMSetting.value * 100)}%";
        SEPercent.text = $"{Mathf.FloorToInt(SESetting.value * 100)}%";

        // 설정한 배경음 볼륨 사운드 매니저에 할당
        for (int i = 0; i <= 6; i++)
        {
            SoundManager.instance.backgroundmusic[i].volume = BGMSetting.value;
        }

        // 설정한 효과음 볼륨 사운드 매니저에 할당
        for (int i = 0; i <= 5; i++)
        {
            SoundManager.instance.uisound[i].volume = SESetting.value;

            if (i >= 0 && i <= 1)
            {
                SoundManager.instance.playersound[i].volume = SESetting.value * 0.8f;
            }
            else
            {
                SoundManager.instance.playersound[i].volume = SESetting.value;
            }
        }

        SoundManager.instance.enemysound[0].volume = SESetting.value;

        SoundManager.instance.foxsound[0].volume = SESetting.value;
        SoundManager.instance.foxsound[1].volume = SESetting.value;

        for (int i = 0; i <= 4; i++)
        {
            SoundManager.instance.bosssound[i].volume = SESetting.value;
        }
    }
}
