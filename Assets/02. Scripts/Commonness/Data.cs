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
            public float attackSpeed; 
            public float attackDamage;
        }

        public static Dictionary<EAttackType, AttackData> AttackDataDict = new Dictionary<EAttackType, AttackData>
        {
            { EAttackType.Bezier, new AttackData { attackSpeed = 1.0f, attackDamage = 1 } },
            { EAttackType.Line, new AttackData { attackSpeed = 0.5f, attackDamage = 3 } },
            { EAttackType.Rain, new AttackData { attackSpeed = 1.0f, attackDamage = 0.8f } }
        };
    }
}
