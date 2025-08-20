using System;
using _02._Scripts.Commonness.Attack.SkillType;
using Unity.VisualScripting;
using UnityEngine;

namespace _02._Scripts.Commonness.Attack
{
    public class SelectSkillData : MonoBehaviour
    {
        public SkillSetDataPrefab prefab;
        public BaseController playerController;
        public Skill[] skills;
        private Skill m_SkillComponent;
        private void OnEnable()
        {
            GameManager.Instance.gameStartEvent += MakeAndSetSkills;
        }

        private void OnDisable()
        {
            GameManager.Instance.gameStartEvent -= MakeAndSetSkills;
        }
        
        private void MakeAndSetSkills(bool t)
        {
            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                //애로우 고르고
                //타입에 맞는 컴포넌트 붙히고
                //버튼에 할당
                var skill = Instantiate(prefab.skillSets[i].arrow);
                switch (prefab.skillSets[i].attackType)
                {
                    case Data.EAttackType.Bezier :
                        skill.AddComponent<BezierProjectile>();
                        m_SkillComponent = skill.GetComponent<BezierProjectile>();
                        m_SkillComponent.isUsable = true;
                        m_SkillComponent.arrow = skill;
                        var bezierProjectile = m_SkillComponent as BezierProjectile;
                        bezierProjectile.prefab = bezierProjectile;
                        break;
                    case Data.EAttackType.Line :
                        skill.AddComponent<LineProjectile>();
                        m_SkillComponent = skill.GetComponent<LineProjectile>();
                        m_SkillComponent.isUsable = true;
                        m_SkillComponent.arrow = skill;
                        var lineProjectile = m_SkillComponent as LineProjectile;
                        lineProjectile.prefab = lineProjectile;
                        break;
                    case Data.EAttackType.Rain :
                        skill.AddComponent<RainProjectile>();
                        m_SkillComponent = skill.GetComponent<RainProjectile>();
                        m_SkillComponent.isUsable = true;
                        m_SkillComponent.arrow = skill;
                        var rainProjectile = m_SkillComponent as RainProjectile;
                        rainProjectile.prefab = rainProjectile;
                        break;
                }

                skills[i] = m_SkillComponent;
                playerController.Bow.skills[i] = m_SkillComponent;
            }
        }
    }
}
