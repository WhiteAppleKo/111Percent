using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02._Scripts.Commonness.Attack
{
    public class Arrow : MonoBehaviour
    {
        public int baseArrowDamage;
        public float searchRadius;
        
        private Skill m_SkillType;
        [SerializeField]
        protected bool isAreaOfEffect = true;

        private void Awake()
        {
            m_SkillType = GetComponent<Skill>();
        }

        public void ReachedWall(string  targetTag, Collider2D col)
        {
            Debug.Log("ReachedWall");
            if (isAreaOfEffect)
            {
                FindTarget(targetTag);
            }
        }

        protected virtual void FindTarget(string targetTag)
        {
            
        }
        
        protected virtual void AreaOfEffect() { }
    }
}
