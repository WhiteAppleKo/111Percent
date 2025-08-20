using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _02._Scripts.Commonness.NonAttackSkill
{
    public class DecoyTotem : NonAttackSkill
    {
        public DecoyTotem prefab;
        public Text coolTimeAnnounce;
        private DecoyTotem m_DecoyTotem;
        private BaseController m_DecoyController;
        
        protected override void SkillInit(BaseController enemy, BaseController user)
        {
            target = enemy;
            self = user;
            isUsable = true;
            m_DecoyController = GetComponent<BaseController>();
        }

        protected override void SkillEffect()
        {
            onUsed?.Invoke(cooldown);
            var dir = (target.transform.position - self.transform.position).normalized;
            var spawnpos = self.transform.position;
            spawnpos.x += dir.x;
            m_DecoyTotem = Instantiate(prefab, spawnpos, Quaternion.identity);
            
            m_DecoyTotem.target = target;
            m_DecoyTotem.self = self;
            m_DecoyTotem.m_DecoyController = m_DecoyTotem.GetComponent<BaseController>();
            m_DecoyTotem.m_DecoyTotem = m_DecoyTotem;
            m_DecoyTotem.m_DecoyController.Bow.currentTarget = target;
            m_DecoyTotem.StartCoroutine(co_SkillEffect());
        }
        
        public IEnumerator co_SkillEffect()
        {
            float time = 0.0f;
            target.Bow.currentTarget = m_DecoyController;
            while (time <= duration)
            {
                time += Time.deltaTime;
                yield return null;
            }
            target.Bow.TargetSetting(self);
            Destroy(m_DecoyTotem.gameObject);
        }
    }
}
