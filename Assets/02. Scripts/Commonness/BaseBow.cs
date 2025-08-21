using System;
using System.Collections;
using _02._Scripts.Commonness.Attack;
using _02._Scripts.Commonness.Attack.SkillType;
using UnityEngine;
using UnityEngine.UI;

namespace _02._Scripts.Commonness
{
    public abstract class BaseBow : MonoBehaviour
    {
        public BaseController self;
        public BaseController currentTarget;
        public float baseAttackSpeed;
        // 0번 기본 공격 이외 나머지 스킬
        public Skill[] skills;
        public string targetPlatform;
        public Animator animator;
        private Transform m_FirePoint;
        private bool m_UseSkill;

        protected virtual void Start()
        {
            SkillInitialize();
            self = GetComponentInParent<BaseController>();
            m_FirePoint = gameObject.transform;
        }
        
        public void Attack(BaseController attacker, Skill skill)
        {
            if (self.haveBow == false)
            {
                return;
            }
            skill.CanFire(m_FirePoint, currentTarget);
            StartCoroutine(co_CoolDown(skill));
        }
        protected int? FindSkillIndex(Skill skill)
        {
            for(int i = 1; i < skills.Length; i++)
            {
                if (skills[i] == skill)
                {
                    return i;
                }
            }

            return null;
        }
        
        protected virtual IEnumerator co_CoolDown(Skill skill)
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
