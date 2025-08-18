using System;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public class BaseController : MonoBehaviour
    {
        
        public int currentHitPoint;
        public int maxHitPoint;
        
        private BaseBow m_Bow;
        private Vector2 m_MoveVector;

        protected void Start()
        {
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
