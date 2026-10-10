using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    // 두번째 스킬 정보
    public GameObject secondSkill;
    public Text secondName;
    public Image secondIcon;
    public Text secondInform;
    public Text secondCost;
    public int secondCostAcorn;

    // 세번째 스킬 정보
    public GameObject thirdSkill;
    public Text thirdName;
    public Image thirdIcon;
    public Text thirdInform;
    public Text thirdCost;
    public int thirdCostAcorn;

    // 스킬 이름 목록
    public string[] skillName = new string[6];

    // 스킬 아이콘 목록
    public Sprite[] skillIcon = new Sprite[6];

    // 스킬 설명 목록
    public string[] skillInform = new string[6];

    // 스킬 비용 목록
    public string[] skillCost = new string[6];

    // 스킬 비용 변수
    public int[] CostAcorn = new int[6];

    // 랜덤으로 뽑은 스킬을 담을 변수
    List<int> selectedSkill = new List<int>();

    // 랜덤으로 스킬 두개를 생성
    void RandomSkill()
    {
        // 스킬을 두 번 뽑을 때까지
        while (selectedSkill.Count < 2)
        {
            // 뽑은 숫자를 할당
            int select = Random.Range(0, 6);

            // 뽑은 숫자가 리스트에 없다면 + 뽑은 스킬이 활성화 상태가 아니라면
            if (!selectedSkill.Contains(select) && !GameManager.instance.skill[select])
            {
                // 뽑은 숫자 리스트에 할당
                selectedSkill.Add(select);
            }
        }

        // 두번째 스킬 UI에 뽑은 스킬의 이름, 아이콘, 설명 할당
        secondSkill.name = selectedSkill[0].ToString();
        secondName.text = skillName[selectedSkill[0]];
        secondIcon.sprite = skillIcon[selectedSkill[0]];
        secondInform.text = skillInform[selectedSkill[0]];
        secondCost.text = skillCost[selectedSkill[0]];
        secondCostAcorn = CostAcorn[selectedSkill[0]];

        // 세번째 스킬 UI에 뽑은 스킬의 이름, 아이콘, 설명 할당
        thirdSkill.name = selectedSkill[1].ToString();
        thirdName.text = skillName[selectedSkill[1]];
        thirdIcon.sprite = skillIcon[selectedSkill[1]];
        thirdInform.text = skillInform[selectedSkill[1]];
        thirdCost.text = skillCost[selectedSkill[1]];
        thirdCostAcorn = CostAcorn[selectedSkill[1]];
    }

    void Start()
    {
        RandomSkill();
    }
}
