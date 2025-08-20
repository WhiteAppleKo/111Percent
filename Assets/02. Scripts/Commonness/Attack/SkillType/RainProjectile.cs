using UnityEngine;
using Random = UnityEngine.Random;

namespace _02._Scripts.Commonness.Attack.SkillType
{
    public class RainProjectile : Skill
    {
        public RainProjectile prefab;
        public int separatedCount;
        private float m_Duration;
        private float m_Time;
        private Vector2 m_P0, m_P1;
        private Vector2 m_MiddlePoint;
        private Vector2 m_FinalEnd; 
        private bool m_IsSeparated;
        private Vector2 m_LastVelocity; 
        private Arrow.Arrow m_CurrentArrow;
        
        private void Awake()
        {
            attackType = Data.EAttackType.Rain;
            attackData = Data.AttackDataDict[attackType];
        }

        private void Initialize(Vector2 start, Vector2 end)
        {
            m_P0 = start;
            m_P1 = end;

            float dist = Vector2.Distance(start, end);
            float speed = Mathf.Max(0.0001f, projectileSpeed);
            m_Duration = Mathf.Max(0.01f, dist / speed * 0.75f);
            m_Time = 0f;

        }

        private void Update()
        {
            if (m_Duration <= 0f) return;

            m_Time += Time.deltaTime;
            float t = m_Time / m_Duration;

            if (t < 1f)
            {
                Vector2 pos = Vector2.Lerp(m_P0, m_P1, t);
                transform.position = new Vector3(pos.x, pos.y, transform.position.z);
                
                m_LastVelocity = (m_P1 - m_P0).normalized;
            }
            else if (!m_IsSeparated)
            {
                // 2구간
                m_IsSeparated = true;
                for (int i = 0; i < separatedCount; i++)
                {
                    var projectile = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
                    projectile.attackData = attackData;
                    projectile.targetPlatform = targetPlatform;
                    projectile.targetTag = targetTag;
                    projectile.m_IsSeparated = true;
                    var randomX = Random.Range(-2f, 2f);
                    var finalEnd = m_FinalEnd;
                    finalEnd.x -= randomX;
                    projectile.Initialize(m_MiddlePoint, finalEnd);
                }
                Initialize(m_MiddlePoint, m_FinalEnd);
                m_Duration *= 0.8f; // 원하는 만큼 가속/감속
            }
            else
            {
                transform.Translate((m_LastVelocity * (projectileSpeed * Time.deltaTime)), Space.World);
            }
        }

        protected override void Fire(Transform attackerBow, BaseController defender)
        {
            Vector2 start = attackerBow.position;
            Vector2 end   = defender.transform.position;
            
            Vector2 mid = (start + end) * 0.5f;
            mid.y += 20f;

            var proj = Instantiate(prefab, start, Quaternion.identity);
            
            proj.targetPlatform = targetPlatform;
            proj.targetTag = targetTag;
            proj.m_IsSeparated  = false;
            proj.m_MiddlePoint = mid;      // 1구간 종점
            proj.m_FinalEnd = end;      // 2구간 종점
            
            proj.arrow = proj.GetComponent<Arrow.Arrow>();
            proj.arrow.currentArrowDamage = proj.attackData.attackDamage * proj.arrow.baseArrowDamage;
            // 1구간
            proj.Initialize(start, mid);
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
