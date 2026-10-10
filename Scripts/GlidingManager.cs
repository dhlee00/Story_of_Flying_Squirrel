using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidingManager : MonoBehaviour
{
    #region 선언부
    // 플레이어 매니저
    PlayerManager pm;

    // 리지드바디
    Rigidbody2D rig;

    // 글라이딩 상태
    public bool isGliding;

    // 글라이딩 속도
    public float glidSpeed;
    #endregion

    void Start()
    {
        pm = GetComponent<PlayerManager>();
        rig = GetComponent<Rigidbody2D>();

        // 스킬 5 활성화 시
        if (GameManager.instance.skill[4])
        {
            // 글라이딩 속도 증가
            glidSpeed += glidSpeed / 2;
        }
    }

    void Update()
    {
        // 점프 중인 상태 + 글라이딩 버튼 눌렀을 때
        if (!pm.Hit && pm.glidOn)
        {
            // 글라이딩 중
            isGliding = true;

            // 사선으로 천천히 떨어지기 (글라이딩)
            rig.velocity = new Vector2(pm.h * glidSpeed, -0.5f);

            // 글라이딩 중에는 공격 불가
            gameObject.GetComponent<PlayerFire>().enabled = false;
        }

        // 글라이딩 버튼 뗐을 때
        else if (!pm.glidOn)
        {
            // 글라이딩 중 아닌 상태
            isGliding = false;

            // 글라이딩 효과음 정지
            SoundManager.instance.playersound[1].Stop();
        }

        // 글라이딩 애니메이션 재생
        pm.anim.SetBool("isGliding", isGliding);
    }

    // 애니메이션에서 호출
    void GlidSound()
    {
        // 글라이딩 효과음 재생
        SoundManager.instance.playersound[1].Play();
    }
}
