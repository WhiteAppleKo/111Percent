using System.Collections.Generic;

namespace _02._Scripts.Commonness
{
    public static class Data
    {
        public enum EAttackType
        {
            Bezier,
            Line,
            Rain,
        }

        public struct AttackData
        {
            public float attackCooldown; 
            public float attackDamage;
        }

        public static Dictionary<EAttackType, AttackData> AttackDataDict = new Dictionary<EAttackType, AttackData>
        {
            { EAttackType.Bezier, new AttackData { attackCooldown = 1.0f, attackDamage = 1.0f } },
            { EAttackType.Line, new AttackData { attackCooldown = 0.5f, attackDamage = 3.0f } },
            { EAttackType.Rain, new AttackData { attackCooldown = 1.0f, attackDamage = 0.8f } }
        };
    }
}
