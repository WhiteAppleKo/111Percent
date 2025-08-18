using System;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public class BaseController : MonoBehaviour
    {
        public int maxHitPoint;
        // 전투 이벤트
        public Action<BaseController, int> onDamaged; // (피격자, 피해량)
        public Action<BaseController> onDied;
        
        private int m_CurrentHitPoint;
        private BaseBow m_Bow;
        private Vector2 m_MoveVector;
        
        public void TakeDamage(int damage)
        {
            m_CurrentHitPoint = Mathf.Clamp(m_CurrentHitPoint - damage, 0, maxHitPoint);
            Debug.Log(m_CurrentHitPoint);
            onDamaged?.Invoke(this, damage);

            if (m_CurrentHitPoint <= 0)
            {
                Debug.Log($"{this.name} 죽음");
                onDied?.Invoke(this);
            }
        }
        
        protected virtual void Start()
        {
            m_CurrentHitPoint = maxHitPoint;
            m_Bow = GetComponentInChildren<BaseBow>();
        }

        protected virtual void Update()
        {
            
            if(m_MoveVector.sqrMagnitude < 0.005f)
            {
                if (m_Bow.skills[0])
                {
                    m_Bow.BasicAttack(this);
                }
            }
        }
    }
}
