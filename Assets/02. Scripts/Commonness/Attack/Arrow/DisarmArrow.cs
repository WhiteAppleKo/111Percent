using UnityEngine;

namespace _02._Scripts.Commonness.Attack.Arrow
{
    public class DisarmArrow : Arrow
    {
        public float effectDuration;
        protected override void ArrowEffect(Collider2D col)
        {
            col.GetComponent<BaseController>().HitDisarmArrow(effectDuration);
        }
    }
}
