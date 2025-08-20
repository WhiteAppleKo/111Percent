using System;
using System.Collections;
using UnityEngine;

namespace _02._Scripts.Commonness.NonAttackSkill
{
    public class DecoyTotem : NonAttackSkill
    {
        public DecoyTotem prefab;
        private DecoyTotem m_DecoyTotem;
        private BaseController m_DecoyController;
        
        protected override void SkillInit(BaseController enemy, BaseController user)
        {
            target = enemy;
            self = user;
            isUsable = true;
            m_DecoyController = GetComponent<BaseController>();
            m_DecoyTotem = this;
        }

        protected override void SkillEffect()
        {
            var dir = (target.transform.position - self.transform.position).normalized;
            var spawnpos = self.transform.position;
            spawnpos.x += dir.x;
            m_DecoyTotem = Instantiate(prefab, spawnpos, Quaternion.identity);
            StartCoroutine(co_SkillEffect());
            onUsed?.Invoke(cooldown);
        }
        
        private IEnumerator co_SkillEffect()
        {
            float time = 0.0f;
            target.Bow.currentTarget = m_DecoyController;
            while (time <= duration)
            {
                time += Time.deltaTime;
                yield return null;
            }
            target.Bow.currentTarget = self;
            Destroy(m_DecoyTotem);
        }
    }
}
