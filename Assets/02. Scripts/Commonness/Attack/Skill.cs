using System;
using UnityEngine;

namespace _02._Scripts.Commonness.Attack
{
    public class Skill : MonoBehaviour
    {
        public string targetPlatform;
        public string targetTag;
        public Action<BaseController> onSkillUsed;
        public Action<BaseController> onSkillFired;
        public Data.EAttackType attackType;
        public Data.AttackData attackData;
        public float projectileSpeed;
        public float baseAttackDamage = 1.0f;

        public bool CanFire(Transform attackerBow, BaseController defender, float attackTime)
        {
            if (attackTime < attackData.attackCooldown)
            {
                Debug.Log($"{attackerBow.name}의 스킬 {gameObject.name} 발사 실패");
                return false;
            }
            onSkillUsed?.Invoke(null);
            Fire(attackerBow, defender);
            return true;
        }

        // 베지어 발사: attacker → (중간 y+5) → defender
        protected virtual void Fire(Transform attackerBow, BaseController defender) { }
        
        protected virtual void OnTriggerEnter2D(Collider2D col) { }
    }
}
