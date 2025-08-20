using System;
using _02._Scripts.Commonness;
using UnityEngine;

namespace _02._Scripts.Player_Control
{
    public class PlayerController : BaseController
    {
        public float moveSpeed;
        
        private bool m_LeftButtonPressed;
        private bool m_RightButtonPressed;
        private Rigidbody2D m_Rigidbody;
        private Vector2 m_MoveDir;

        protected override void Awake()
        {
            base.Awake();
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SkillUse(int skillNumber)
        {
            Bow.Attack(this, Bow.skills[skillNumber]);
        }

        public void BoolSet(int dir)
        {
            if (dir == -1)
            {
                m_LeftButtonPressed = true;
                Move(-1);
            }

            if (dir == 1)
            {
                m_LeftButtonPressed = false;
                Move(0);
            }

            if (dir == 2)
            {
                m_RightButtonPressed = true;
                Move(1);
            }

            if (dir == -2)
            {
                m_RightButtonPressed = false;
                Move(0);
            }
        }
        
        private void Move(float i)
        {
            m_Rigidbody.velocity = new Vector2(i * moveSpeed, 0);
        }

        protected override void Update()
        {
            base.Update();
            if (m_LeftButtonPressed != m_RightButtonPressed)
            {
                if (m_LeftButtonPressed)
                {
                   
                }
                else
                {
                    
                }
            }
        }
    }
}
