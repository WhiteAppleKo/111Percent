using System;
using UnityEngine;

namespace _02._Scripts.Commonness.NonAttackSkill
{
    public class NonAttackSkill : MonoBehaviour
    {
        public bool isUsable;
        public float duration;
        public float cooldown;
        public Action<float> onUsed;
        protected BaseController target;
        protected BaseController self;
        public void SkillSet(BaseController enemy, BaseController user)
        {
            SkillInit(enemy, user);
        }

        public void ActiveSkill()
        {
            SkillEffect();
        }
        protected virtual void SkillInit(BaseController enemy, BaseController self) { }
        protected virtual void SkillEffect(){}
    }
}
