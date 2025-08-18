using System;
using _02._Scripts.EnemyScripts;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public abstract class BaseBow : MonoBehaviour
    {
        public BaseController currentTarget;
        public int baseAttackSpeed;
        public int baseAttackDamage;
        // 0번 기본 공격 이외 나머지 스킬
        public Skill[] skills;
        

        protected string targetPlatform;
        
        private float m_AttackTime = 0;
        private Transform m_FirePoint;
        private bool m_UseSkill;

        protected virtual void Start()
        {
            SkillSetting();
            m_FirePoint = gameObject.transform;
        }

        protected virtual void Update()
        {
            m_AttackTime += Time.deltaTime;
        }

        public void BasicAttack(BaseController attacker)
        {
            if (skills[0].Fire(m_FirePoint, currentTarget, m_AttackTime))
            {
                m_AttackTime = 0;
            }
        }

        private void SkillSetting()
        {
            foreach (Skill skill in skills)
            {
                skill.attackData = Data.AttackDataDict[skill.attackType];
                skill.attackData.attackSpeed *= baseAttackSpeed;
                skill.attackData.attackDamage *= baseAttackDamage;
                skill.targetPlatform  = targetPlatform;
                skill.targetTag = currentTarget.tag;
            }
        }
    }
}
