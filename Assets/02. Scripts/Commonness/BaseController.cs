using System;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public class BaseController : MonoBehaviour
    {
        public ClampInt Health { get; private set; }
        
        private BaseBow m_Bow;
        private Vector2 m_MoveVector;
        
        public void TakeDamage(int damage)
        {
            Health.Decrease(damage);
        }
        
        protected virtual void Awake()
        {
            InitializeStatus();
            m_Bow = GetComponentInChildren<BaseBow>();
        }

        private void InitializeStatus()
        {
            Health = new ClampInt(min: 0, max: 100, initial: 100);
        }

        private void OnEnable()
        {
            Health.Events.onMinReached += Die;
        }

        private void OnDisable()
        {
            Health.Events.onMinReached -= Die;
        }
        
        private void Die(int prev, int current)
        {
            Debug.Log($"{name} 사망");
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
