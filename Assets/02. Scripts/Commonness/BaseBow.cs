using System;
using System.Collections;
using _02._Scripts.Commonness.Attack;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public abstract class BaseBow : MonoBehaviour
    {
        public BaseController currentTarget;
        public float baseAttackSpeed;
        // 0번 기본 공격 이외 나머지 스킬
        public Skill[] skills;
        

        protected string targetPlatform;
        
        private float m_AttackTime = 0;
        private Transform m_FirePoint;
        private bool m_UseSkill;

        protected virtual void Start()
        {
            SkillInitialize();
            m_FirePoint = gameObject.transform;
        }

        protected virtual void Update()
        {
            m_AttackTime += Time.deltaTime;
        }

        public void Attack(BaseController attacker, Skill skill)
        {
            if (m_AttackTime < baseAttackSpeed)
            {
                return;
            }
            skill.CanFire(m_FirePoint, currentTarget);
            m_AttackTime = 0;
            StartCoroutine(co_CoolDown(skill));
        }

        private IEnumerator co_CoolDown(Skill skill)
        {
            skill.isUsable = false;
            var leftCooldown = skill.attackData.attackCooldown;
            while (leftCooldown >= 0)
            {
                leftCooldown -= Time.deltaTime;
                yield return null;
            }
            skill.isUsable = true;
        }

        private void SkillInitialize()
        {
            foreach (Skill skill in skills)
            {
                skill.SkillInit(baseAttackSpeed, targetPlatform, currentTarget.tag);
            }
        }
    }
}
