using UnityEngine;
using UnityEngine.Serialization;

namespace _02._Scripts.Commonness.Attack
{
    public class ExplosiveArrow : Arrow
    {
        [FormerlySerializedAs("overlapCapacity")] [SerializeField] 
        int m_OverlapCapacity = 32;
        private Collider2D[] m_Hits;
        private Skill m_Skill;

        void Awake()
        {
            m_Hits = new Collider2D[m_OverlapCapacity];
            m_Skill = GetComponent<Skill>();
        }

        protected override void FindTarget(string targetTag)
        {
            var pos = (Vector2)transform.position;
            int count = Physics2D.OverlapCircleNonAlloc(pos, searchRadius, m_Hits);
            Debug.Log(count);
            int dmg = m_Skill ? Mathf.RoundToInt(m_Skill.attackData.attackDamage) : 0;
            if (count == 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                var col = m_Hits[i];
                if (!col)
                {
                    continue;
                }

                if (!col.CompareTag(targetTag))
                {
                    continue;
                }

                var bc = col.GetComponent<BaseController>();
                if (!bc) { 
                    continue; 
                }

                bc.Health?.Decrease(dmg);
                Debug.Log($"hit {bc.name} dmg={dmg}, hp={bc.Health?.Current}");
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
    }
}
