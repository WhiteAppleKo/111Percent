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
        private Skill m_CurrentSkill;
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
            if (m_AttackTime < skills[0].attackSpeed)
            {
                Debug.Log($"{attacker.name} {skills[0].name} 공격 쿨타임");
                return;
            }
            m_CurrentSkill = skills[0];
            m_CurrentSkill.Fire(m_FirePoint, currentTarget.GetComponent<BaseController>());
            m_AttackTime = 0.0f;
        }

        private void SkillSetting()
        {
            foreach (Skill skill in skills)
            {
                skill.attackSpeed *= baseAttackSpeed;
                skill.attackDamage *= baseAttackDamage;
                skill.targetPlatform  = targetPlatform;
            }
        }
    }
}
