using System;
using UnityEngine;
namespace _02._Scripts.Commonness.Attack
{
    public class LineProjectile : Skill
    {
        public LineProjectile prefab;
        
        private float m_Duration;
        private float m_Time;
        private Vector2 m_P0, m_P1;
        private Vector2 m_LastVelocity; 

        private void Initialize(Vector2 start, Vector2 end)
        {
            m_P0 = start; 
            m_P1 = end;
            
            float straightDist = Vector2.Distance(start, end);
            m_Duration = Mathf.Max(0.01f, straightDist / projectileSpeed * 0.75f);
            m_Time = 0f;
        }

        private void Update()
        {
            if (m_Duration <= 0f)
            {
                return;
            }

            m_Time += Time.deltaTime;
            float t = m_Time / m_Duration;

            if (t < 1f)
            {
                // 직선 구간: 보간 이동
                Vector2 pos = Vector2.Lerp(m_P0, m_P1, t);
                transform.position = new Vector2(pos.x, pos.y);

                // 진행 방향(직선은 고정)
                m_LastVelocity = (m_P1 - m_P0).normalized;
            }
            else
            {
                // 종점 이후: 같은 방향으로 계속 직진
                transform.Translate((m_LastVelocity * (projectileSpeed * Time.deltaTime)), Space.World);
            }
        }

        protected override void Fire(Transform attackerBow, BaseController defender, Data.AttackData atkData)
        {
            Vector2 start = (Vector2)attackerBow.transform.position;
            Vector2 end   = (Vector2)defender.transform.position;
            
            var proj = Instantiate(prefab, start, Quaternion.identity);
            proj.attackData = atkData;
            proj.Initialize(start, end);
        }
        
        protected override void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(col.name);
            if (col.CompareTag(targetPlatform) || col.CompareTag("Platform"))
            {
                Destroy(gameObject);
            }

            if (col.CompareTag(targetTag))
            {
                var defender = col.GetComponent<BaseController>();
                defender.TakeDamage((int)(attackData.attackDamage));
                Destroy(gameObject);
            }
        }
    }
}
