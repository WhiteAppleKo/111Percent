using UnityEngine;

namespace _02._Scripts.Commonness.Attack.SkillType
{
    public class BezierProjectile : Skill
    {
        public BezierProjectile prefab;
        
        private Vector2 m_P0, m_P1, m_P2;
        private float m_Duration;
        private float m_Time;
        private Vector2 m_LastVelocity;
        private Arrow.Arrow m_CurrentArrow;

        private void Awake()
        {
            attackType = Data.EAttackType.Bezier;
            attackData = Data.AttackDataDict[attackType];
        }
        private void Initialize(Vector2 start, Vector2 control, Vector2 end)
        {
            m_P0 = start; 
            m_P1 = control; 
            m_P2 = end;

            float straightDist = Vector2.Distance(start, end);
            m_Duration = Mathf.Max(0.01f, straightDist / projectileSpeed);
            m_Time = 0f;
        }
        
        private void Update()
        {
            if (m_Duration <= 0f)
            {
                return;
            }

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

                transform.position = new Vector2(pos.x, pos.y);

                if (vel.sqrMagnitude > 0.001)
                {
                    m_LastVelocity = vel.normalized;   // ← 진행 방향 기록
                    float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }

            }else
            {
                transform.position += (Vector3)(m_LastVelocity * (float)(projectileSpeed * 1.5 * Time.deltaTime));
            }
        }
        
        protected override void Fire(Transform attackerBow, BaseController defender)
        {
            Vector2 start = (Vector2)attackerBow.transform.position;
            Vector2 end   = (Vector2)defender.transform.position;

            Vector2 controlPoint = (start + end) * 0.5f;
            controlPoint.y += 10f;

            var proj = Instantiate(prefab, start, Quaternion.identity);
            proj.arrow = proj.GetComponent<Arrow.Arrow>();
            proj.arrow.currentArrowDamage = proj.attackData.attackDamage * proj.arrow.baseArrowDamage;
            proj.Initialize(start, controlPoint, end);
            proj.m_CurrentArrow = proj.arrow;
        }
        
        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(targetPlatform) || col.CompareTag("Platform"))
            {
                if (m_CurrentArrow.isAreaOfEffect)
                {
                    arrow.ReachedWall(targetTag, col);
                }
                Destroy(gameObject);
            }

            if (col.CompareTag(targetTag))
            {
                var defender = col.GetComponent<BaseController>();
                defender.TakeDamage((int)(m_CurrentArrow.currentArrowDamage));
                m_CurrentArrow.HitEffect(col);
                Destroy(gameObject);
            }
        }
    }
}
