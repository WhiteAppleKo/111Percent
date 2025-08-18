using System;
using System.Collections.Generic;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public class Skill : MonoBehaviour
    {
        public Projectile projectile;
        public string targetPlatform;
        public string targetTag;
        public Action<BaseController> onSkillUsed;
        public Action<BaseController> onSkillFired;
        public Data.EAttackType attackType;
        public Data.AttackData attackData;

        public bool Fire(Transform attackerBow, BaseController defender, float attackTime)
        {
            if (attackTime < attackData.attackSpeed)
            {
                Debug.Log($"{attackerBow.name}의 스킬 {gameObject.name} 발사 실패");
                return false;
            }
            onSkillUsed?.Invoke(null);
            switch (attackType)
            {
                case Data.EAttackType.Bezier :
                    BezierFire(attackerBow, defender);
                    return true;
                case Data.EAttackType.Line :
                    return true;
                case Data.EAttackType.Rain :
                    return true;
            }
            return false;
        }

        // 베지어 발사: attacker → (중간 y+5) → defender
        private void BezierFire(Transform attackerBow, BaseController defender)
        {
            Vector2 start = (Vector2)attackerBow.transform.position;
            Vector2 end   = (Vector2)defender.transform.position;

            Vector2 controlPoint = (start + end) * 0.5f;
            controlPoint.y += 5f;

            var proj = Instantiate(projectile, start, Quaternion.identity);
            proj.Initialize(start, controlPoint, end, Data.EAttackType.Bezier);
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(targetPlatform) || col.CompareTag("Platform"))
            {
                Destroy(gameObject);
            }

            if (col.CompareTag(targetTag))
            {
                var defender = col.GetComponent<BaseController>();
                defender.TakeDamage((int)(attackData.attackDamage));
            }
        }
    }
}
