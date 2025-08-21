using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _02._Scripts.Commonness.Attack
{
    public class MakeSkillCanvas : MonoBehaviour
    {
        public SkillSetDataPrefab prefab;
        public Dropdown[] dropdowns;
        public Arrow.Arrow[] arrows;

        [Serializable]
        public class Cautionpanel
        {
            public GameObject panel;
            public Text text;
        }

        [Serializable]
        public class CurrentSkillView
        {
            public Image arrow;
            public Image move;
        }

        public CurrentSkillView[] skillViews;
        public Cautionpanel cautionPanel;

        private string m_CautionText;

        private void Awake()
        {
            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                prefab.skillSets[i].arrow = null;
            }

            m_CautionText = "모든 화살과 무빙 에셋을 한번 씩은 조합 해야 합니다. \n 아래는 현재 스킬 조합 목록 입니다.";
        }

        public void Set()
        {
            var skill = new SkillSetDataPrefab.SkillSet();
            skill.arrow = arrows[dropdowns[0].value];
            skill.attackType = (Data.EAttackType)dropdowns[1].value;
            skill.buttonNumber = dropdowns[2].value;
            prefab.skillSets[skill.buttonNumber] = skill;

            skillViews[dropdowns[2].value].arrow.sprite = dropdowns[0].options[dropdowns[0].value].image;
            skillViews[dropdowns[2].value].move.sprite = dropdowns[2].options[dropdowns[2].value].image;
        }

        public void SetCaution(bool isOpen)
        {

            cautionPanel.panel.SetActive(isOpen);
            cautionPanel.text.text = m_CautionText;
        }

        private bool CheckSelectAllArrow()
        {
            Arrow.Arrow[] tempArrows = new Arrow.Arrow[prefab.skillSets.Length];
            bool isfail = false;
            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                tempArrows[i] = prefab.skillSets[i].arrow;
            }

            if (tempArrows.Contains(arrows[0]) == false)
            {
                isfail = true;
            }

            if (tempArrows.Contains(arrows[1]) == false)
            {
                isfail = true;
            }

            if (tempArrows.Contains(arrows[2]) == false)
            {
                isfail = true;
            }

            if (isfail)
            {
                return false;
            }

            return true;
        }

        public void LoadScene()
        {
            if (CheckSelectAllArrow() == false)
            {
                return;
            }

            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                if (prefab.skillSets[i] == null)
                {
                    SetCaution(true);
                    return;
                }
            }

            SceneManager.LoadScene(1);
        }

        public void ShowCautionPanel()
        {
            SetCaution(true);
        }
    }
}