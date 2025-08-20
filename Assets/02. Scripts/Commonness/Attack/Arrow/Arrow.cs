using _02._Scripts.Commonness.Attack.SkillType;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02._Scripts.Commonness.Attack.Arrow
{
    public class Arrow : MonoBehaviour
    {
        public float currentArrowDamage;
        public float baseArrowDamage;
        public float searchRadius;
        public bool isAreaOfEffect = true;
        public float coolTime;
        public Sprite sprite;
        [FormerlySerializedAs("overlapCapacity")] [SerializeField] 
        int m_OverlapCapacity = 32;
        private Skill m_SkillType;
        private Collider2D[] m_Hits;
        protected BaseController hitBaseController;
        

        protected virtual void Awake()
        {
            m_SkillType = GetComponent<Skill>();
            m_Hits = new Collider2D[m_OverlapCapacity];
        }

        public void ReachedWall(string  targetTag, Collider2D col)
        {
            Debug.Log("ReachedWall");
            if (isAreaOfEffect)
            {
                AreaOfEffect(targetTag);
            }
        }

        public void HitEffect(Collider2D col)
        {
            ArrowEffect(col);
        }

        protected virtual void FindTarget(string targetTag) { }

        
        protected virtual void AreaOfEffect(string targetTag)
        {
            var pos = (Vector2)transform.position;
            int count = Physics2D.OverlapCircleNonAlloc(pos, searchRadius, m_Hits);
            Debug.Log(count);
            if (count == 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                var col = m_Hits[i];
                if (col.CompareTag(targetTag))
                {
                    hitBaseController = col.GetComponent<BaseController>();
                }
            }
        }

        protected virtual void ArrowEffect(Collider2D col) { }
    }
}
