using System.Collections;
using UnityEngine;

namespace _02._Scripts.EnemyScripts
{
    public class MoveState : BaseState
    {
        public float moveSpeed;
        public float moveTime;

        private Rigidbody2D m_Rigidbody;
        public override void Enter()
        {
            Controller.isMoving = true;
            if (m_Rigidbody == null)
            {
                m_Rigidbody = Controller.GetComponent<Rigidbody2D>();
            }

            StartCoroutine(co_MoveRoutine());
        }

        public override void Exit()
        {
            Controller.isMoving = false;
            Controller.ReInitializeWeight();
            StopCoroutine(co_MoveRoutine());
        }

        private IEnumerator co_MoveRoutine()
        {
            int dir = Random.Range(-1, 1);
            if (dir == 0)
            {
                dir = 1;
            }
            float end = Time.time + moveTime;

            while (Time.time < end)
            {
                m_Rigidbody.velocity = new Vector2(dir * moveSpeed, 0);
                
                yield return null;
            }
            var v = m_Rigidbody.velocity;
            v.x = 0f;
            m_Rigidbody.velocity = v;
            m_Rigidbody.angularVelocity = 0f;
            Controller.SelectAction();
        }
    }
}
