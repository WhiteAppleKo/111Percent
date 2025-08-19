using System;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public class BaseController : MonoBehaviour
    {
        public ClampInt Health { get; private set; }
        public BaseController m_Self;
        public BaseBow Bow {get; private set;}
        public bool isMoving = false;
        
        public void TakeDamage(int damage)
        {
            Health.Decrease(damage);
        }
        
        protected virtual void Awake()
        {
            InitializeStatus();
            Bow = GetComponentInChildren<BaseBow>();
            m_Self = this;
        }

        protected virtual void InitializeStatus()
        {
            m_Self = this;
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
        private void MoveAsset1()
        {
            Vector2 dir = Bow.currentTarget.transform.position - transform.position;
            dir.Normalize();
            var myPos = transform.position;
            myPos.x += dir.x;
            transform.position = myPos;
        }

        private void MoveAsset2()
        {
            
        }

        private void SetMoveAsset()
        {
            if (Bow.skills[1] == null) return;
            Bow.skills[1].onSkillUsed += MoveAsset1;
        }

        private bool m_IsSetMoveAsset;
        protected virtual void Update()
        {
            if (Bow != null && m_IsSetMoveAsset == false)
            {
                SetMoveAsset();
                m_IsSetMoveAsset = true;
            }
            if(isMoving == false)
            {
                if (Bow.skills[0].isUsable)
                {
                    Bow.Attack(this, Bow.skills[0]);
                }
            }
        }
    }
}
