using System;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public class Skill : MonoBehaviour
    {
        public float attackSpeed;
        public float attackDamage;
        public Projectile projectile;

        public EAttackType attackType;
        public enum EAttackType
        {
            Bezier,
            Line,
            Rain,
        }
        public string targetPlatform;
        private float m_AttackTime;

        public void Fire(Transform attackerBow, BaseController defender)
        {
            /*if (m_AttackTime < attackSpeed)
            {
                Debug.Log($"{attacker.name}의 스킬 {gameObject.name} 발사 실패");
                return;
            }*/
            switch (attackType)
            {
                case EAttackType.Bezier :
                    BezierFire(attackerBow, defender);
                    break;
                case EAttackType.Line :
                    break;
                case EAttackType.Rain :
                    break;
            }
        }

        // 베지어 발사: attacker → (중간 y+5) → defender
        private void BezierFire(Transform attackerBow, BaseController defender)
        {
            Vector2 start = (Vector2)attackerBow.transform.position;
            Vector2 end   = (Vector2)defender.transform.position;

            Vector2 controlPoint = (start + end) * 0.5f;
            controlPoint.y += 5f;

            var proj = Instantiate(projectile, start, Quaternion.identity);
            proj.Initialize(start, controlPoint, end);
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(targetPlatform) || col.CompareTag("Platform"))
            {
                Destroy(gameObject);
            }
        }
    }
}
