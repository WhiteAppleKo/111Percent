using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02._Scripts.Commonness.Attack
{
    public class Skill : MonoBehaviour
    {
        public string targetPlatform;
        public string targetTag;
        public Action onSkillUsed;
        public Action<BaseController> onSkillFired;
        public Data.EAttackType attackType;
        public Data.AttackData attackData;
        public float projectileSpeed;
        
        public float skillCooldown;
        public bool isUsable;
        
        private float m_BaseAttackDamage;
        public Arrow arrow;
        public void SkillInit(float baseAttackSpeed, string targetPlatform, string tag)
        {
            SkillSetting(baseAttackSpeed, targetPlatform, tag);
        }
        
        protected virtual void SkillSetting(float baseAttackSpeed, string targetPlatform, string tag)
        {
            arrow = GetComponent<Arrow>();
            attackData = Data.AttackDataDict[attackType];
            attackData.attackCooldown *= baseAttackSpeed * skillCooldown;
            m_BaseAttackDamage = arrow.baseArrowDamage;
            attackData.attackDamage *= m_BaseAttackDamage;
            this.targetPlatform  = targetPlatform;
            targetTag = tag;
            isUsable = true;
        }
        public void CanFire(Transform attackerBow, BaseController defender)
        {
            onSkillUsed?.Invoke();
            Fire(attackerBow, defender, attackData);
        }
        

        // 베지어 발사: attacker → (중간 y+5) → defender
        protected virtual void Fire(Transform attackerBow, BaseController defender, Data.AttackData attackData) { }
        
        protected virtual void OnTriggerEnter2D(Collider2D col) { }
    }
}
