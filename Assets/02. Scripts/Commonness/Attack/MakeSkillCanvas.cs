using System;
using UnityEditor.Build.Content;
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

        private void Awake()
        {
            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                prefab.skillSets[i] = null;
            }
        }

        public void Set()
        {
            var skill = new SkillSetDataPrefab.SkillSet();
            skill.arrow = arrows[dropdowns[0].value];
            skill.attackType = (Data.EAttackType)dropdowns[1].value;
            skill.buttonNumber =  dropdowns[2].value;
            prefab.skillSets[skill.buttonNumber] = skill;
        }

        public void LoadScene()
        {
            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                if (prefab.skillSets[i] == null)
                {
                    return;
                }
            }
            SceneManager.LoadScene(3);
        }
    }
}
