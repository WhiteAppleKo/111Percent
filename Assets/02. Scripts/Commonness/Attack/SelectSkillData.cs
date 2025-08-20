using System;
using UnityEngine;

namespace _02._Scripts.Commonness.Attack
{
    public class SelectSkillData : MonoBehaviour
    {
        public Arrow.Arrow[] arrows;
        public class SkillSet
        {
            public Arrow.Arrow arrow;
            public int moveAsset;
            public Data.EAttackType attackType;
        }

        public SkillSet playerSkillSet;
        public SkillSet enemySkillSet;

        private void Start()
        {
            playerSkillSet = new SkillSet();
            enemySkillSet = new  SkillSet();
        }
    }
}
