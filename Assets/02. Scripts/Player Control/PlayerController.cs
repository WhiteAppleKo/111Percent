using System;
using System.Collections;
using _02._Scripts.Commonness;
using UnityEngine;
using UnityEngine.UI;

namespace _02._Scripts.Player_Control
{
    public class PlayerController : BaseController
    {
        public float moveSpeed;
        [Serializable]
        public class CoolTimeAnnounce
        {
            public Image image;
            public Text text;
        }
        public CoolTimeAnnounce coolTimeAnnounce;
        
        private bool m_LeftButtonPressed;
        private bool m_RightButtonPressed;
        private Rigidbody2D m_Rigidbody;
        private Vector2 m_MoveDir;
        private int m_MoveValue;

        protected override void Awake()
        {
            base.Awake();
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }
        public void BoolSet(int dir)
        {
            if (dir == -1)
            {
                m_LeftButtonPressed = true;
                onMove?.Invoke(m_LeftButtonPressed);
                isMoving = true;
                var rotation = transform.rotation;
                rotation.y = 180;
                transform.rotation = rotation;
                m_MoveValue = -1;
            }

            if (dir == 1)
            {
                m_LeftButtonPressed = false;
                isMoving = false;
                var rotation = transform.rotation;
                rotation.y = 0;
                transform.rotation = rotation;
                onMove?.Invoke(m_LeftButtonPressed);
                m_MoveValue = 0;
            }

            if (dir == 2)
            {
                m_RightButtonPressed = true;
                isMoving = true;
                onMove?.Invoke(m_RightButtonPressed);
                m_MoveValue = 1;
            }

            if (dir == -2)
            {
                m_RightButtonPressed = false;
                isMoving = false;
                onMove?.Invoke(m_RightButtonPressed);
                m_MoveValue = 0;
            }
        }
        
        private void Move(float i)
        {
            m_Rigidbody.velocity = new Vector2(i * moveSpeed, 0);
        }

        protected override void Update()
        {
            base.Update();
            Move(m_MoveValue);
        }

        public GameObject losePanel;
        public GameObject winPanel;
        protected override void Die(int prev, int current)
        {
            base.Die(prev, current);
            losePanel.SetActive(true);
        }

        protected override void Victory(int prev, int current)
        {
            base.Victory(prev, current);
            winPanel.SetActive(true);
        }

        protected override IEnumerator co_CoolDown(float cooldown)
        {
            nonAttackSkill.isUsable = false;
            var time = cooldown;
            coolTimeAnnounce.text.text = time.ToString("F1");
            var block = coolTimeAnnounce.image.color;
            block = Color.gray;
            coolTimeAnnounce.image.color = block;
            while (time > 0)
            {
                if (!animator.IsInTransition(0))
                {
                    AnimatorStateInfo s = animator.GetCurrentAnimatorStateInfo(0);
                    if (s.IsName("Attack") && s.normalizedTime >= 1f)
                    {
                        animator.SetBool("isAttack", false);
                    }
                }
                time -= Time.deltaTime;
                coolTimeAnnounce.text.text = time.ToString("F1");
                yield return null;
            }
            nonAttackSkill.isUsable = true;
            coolTimeAnnounce.text.text = time.ToString(" ");
            block = Color.white;
            coolTimeAnnounce.image.color = block;
        }
    }
}
