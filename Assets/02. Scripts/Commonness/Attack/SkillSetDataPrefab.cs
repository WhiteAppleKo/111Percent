using System;
using UnityEngine;

namespace _02._Scripts.Commonness.Attack
{
    public class SkillSetDataPrefab : MonoBehaviour
    {
        [Serializable]
        public class SkillSet
        {
            public Arrow.Arrow arrow;
            public int buttonNumber;
            public Data.EAttackType attackType;
        }
        public SkillSet[] skillSets;

        public void SetSkillSet(Arrow.Arrow arrow, int buttonNumber, Data.EAttackType  type)
        {
            skillSets[buttonNumber].arrow = arrow;
            skillSets[buttonNumber].buttonNumber = buttonNumber;
            skillSets[buttonNumber].attackType =  type;
        }
    }
}
