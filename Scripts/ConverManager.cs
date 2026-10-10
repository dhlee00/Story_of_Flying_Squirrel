using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverManager : MonoBehaviour
{
    // 튜토리얼 매니저
    public TutorialManager tm;

    // 플레이어 매니저
    public PlayerManager pm;

    // 플레이어 공격
    public PlayerFire pf;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 닿았다면
        if (collision.gameObject.tag == "Player")
        {
            // 플레이어 기능 정지
            pm.enabled = false;
            pf.enabled = false;

            // 게임 멈춤
            Time.timeScale = 0;

            // 대화창 활성화
            tm.converView.SetActive(true);

            // 대화창 버튼 활성화
            tm.prev.gameObject.SetActive(true);
            tm.next.gameObject.SetActive(true);
            tm.close.gameObject.SetActive(true);

            // 효과음 재생
            SoundManager.instance.uisound[1].Play();

            // 대화 상태
            if (tm.nowConversation == 1)
            {
                tm.conState = TutorialManager.Conversation.Glid;
            }

            else if (tm.nowConversation == 2)
            {
                tm.conState = TutorialManager.Conversation.Acorn;
            }

            else if (tm.nowConversation == 3)
            {
                tm.conState = TutorialManager.Conversation.Thorn;
            }

            else if (tm.nowConversation == 4)
            {
                tm.conState = TutorialManager.Conversation.Enemy;
            }

            else if (tm.nowConversation == 5)
            {
                tm.conState = TutorialManager.Conversation.Finish;
            }

            // 자기자신 삭제
            Destroy(gameObject);
        }
    }
}
