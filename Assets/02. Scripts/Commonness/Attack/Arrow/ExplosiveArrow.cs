using UnityEngine;

namespace _02._Scripts.Commonness.Attack.Arrow
{
    public class ExplosiveArrow : Arrow
    {
        protected override void AreaOfEffect(string targetTag)
        { 
            base.AreaOfEffect(targetTag);
            if (hitBaseController != null)
            {
                hitBaseController.Health?.Decrease((int)currentArrowDamage);
                Debug.Log($"hit {hitBaseController.name} dmg={currentArrowDamage}, hp={hitBaseController.Health?.Current}");
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
    }
}
