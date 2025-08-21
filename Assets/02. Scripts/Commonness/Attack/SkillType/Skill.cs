using System;
using UnityEngine;

namespace _02._Scripts.Commonness.Attack.SkillType
{
    public class Skill : MonoBehaviour
    {
        public string targetPlatform;
        public string targetTag;
        public Action onSkillUsed;
        public Action<BaseController> onSkillFired;
        
        public Data.AttackData attackData;
        public float projectileSpeed;
        public float skillCooldown;
        public bool isUsable;
        public Arrow.Arrow arrow;
        
        protected Data.EAttackType attackType;
        public void SkillInit(float baseAttackSpeed, string targetPlatform, string tag)
        {
            SkillSetting(baseAttackSpeed, targetPlatform, tag);
        }
        
        public Skill Clone()
        {
            return (Skill)this.MemberwiseClone(); // 얕은 복사
        }
        
        protected virtual void SkillSetting(float baseAttackSpeed, string targetPlatform, string tag)
        {
            arrow = GetComponent<Arrow.Arrow>();
            attackData = Data.AttackDataDict[attackType];
            attackData.attackCooldown = baseAttackSpeed * skillCooldown * arrow.coolTime * attackData.attackCooldown;
            this.targetPlatform  = targetPlatform;
            targetTag = tag;
            isUsable = true;
        }
        public void CanFire(Transform attackerBow, BaseController defender)
        {
            onSkillUsed?.Invoke();
            Fire(attackerBow, defender);
        }
        

        // 베지어 발사: attacker → (중간 y+5) → defender
        protected virtual void Fire(Transform attackerBow, BaseController defender) { }
        
        protected virtual void OnTriggerEnter2D(Collider2D col) { }
    }
}
