using UnityEngine;

namespace _02._Scripts.Commonness.Attack
{
    public class BezierProjectile : Skill
    {
        public BezierProjectile prefab;
        
        private Vector2 m_P0, m_P1, m_P2;
        private float m_Duration;
        private float m_Time;
        private Vector2 m_LastVelocity; 

        private void NavInitialize(Vector2 start, Vector2 control, Vector2 end, Data.EAttackType type)
        {
            m_P0 = start; m_P1 = control; m_P2 = end;

            float straightDist = Vector2.Distance(start, end);
            m_Duration = Mathf.Max(0.01f, straightDist / projectileSpeed);
            m_Time = 0f;

            transform.position = start;
        }
        
        private void Update()
        {
            if (m_Duration <= 0f) return;

            m_Time += Time.deltaTime;
            float t = Mathf.Clamp01(m_Time / m_Duration);

            if (t < 1f)
            {
                // B(t) = (1-t)^2 p0 + 2(1-t)t p1 + t^2 p2
                Vector2 pos =
                    (1 - t) * (1 - t) * m_P0 +
                    2f * (1 - t) * t * m_P1 +
                    t * t * m_P2;

                // 진행 방향
                Vector2 vel =
                    2f * (1 - t) * (m_P1 - m_P0) +
                    2f * t * (m_P2 - m_P1);

                transform.position = new Vector3(pos.x, pos.y, transform.position.z);

                if (vel.sqrMagnitude > 0.001)
                {
                    m_LastVelocity = vel.normalized;   // ← 진행 방향 기록
                    float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }

            }else
            {
                transform.Translate(m_LastVelocity * (projectileSpeed * Time.deltaTime), Space.World);
            }
        }
        
        protected override void Fire(Transform attackerBow, BaseController defender)
        {
            Vector2 start = (Vector2)attackerBow.transform.position;
            Vector2 end   = (Vector2)defender.transform.position;

            Vector2 controlPoint = (start + end) * 0.5f;
            controlPoint.y += 5f;

            var proj = Instantiate(prefab, start, Quaternion.identity);
            proj.NavInitialize(start, controlPoint, end, Data.EAttackType.Bezier);
        }
        
        protected override void OnTriggerEnter2D(Collider2D col)
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
