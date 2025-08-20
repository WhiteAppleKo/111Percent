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
        private bool[] m_SkillCount;

        private void Awake()
        {
            m_SkillCount = new bool[arrows.Length];
            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                prefab.skillSets[i] = null;
            }
        }

        public void Set()
        {
            var skill = new SkillSetDataPrefab.SkillSet();
            skill.arrow = arrows[dropdowns[0].value];
            m_SkillCount[dropdowns[0].value] = true;
            skill.attackType = (Data.EAttackType)dropdowns[1].value;
            skill.buttonNumber =  dropdowns[2].value;
            prefab.skillSets[skill.buttonNumber] = skill;
        }

        public void LoadScene()
        {
            for (int i = 0; i < m_SkillCount.Length; i++)
            {
                if (m_SkillCount[i] == false)
                {
                    Debug.Log("모든 화살을 종류별로 하나씩 골라 주세요");
                    return;
                }
            }
            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                if (prefab.skillSets[i] == null)
                {
                    Debug.Log("모든 무빙 에셋을 골라 주세요");
                    return;
                }
            }
            SceneManager.LoadScene(1);
        }
    }
}
