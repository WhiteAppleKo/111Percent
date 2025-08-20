using System;
using _02._Scripts.Commonness.Attack.SkillType;
using _02._Scripts.Player_Control;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02._Scripts.Commonness.Attack
{
    public class SelectSkillData : MonoBehaviour
    {
        public SkillSetDataPrefab prefab;
        public BaseController playerController;
        
        private Skill m_SkillComponent;
        
        public void MakeAndSetSkills()
        {
            for (int i = 0; i < prefab.skillSets.Length; i++)
            {
                var skill = prefab.skillSets[i].arrow;
                foreach (var component in skill.gameObject.GetComponents<Skill>())
                {
                    DestroyImmediate(component, true);
                }

                switch (prefab.skillSets[i].attackType)
                {
                    case Data.EAttackType.Bezier :
                        skill.AddComponent<BezierProjectile>();
                        m_SkillComponent = skill.GetComponent<BezierProjectile>();
                        m_SkillComponent.isUsable = true;
                        m_SkillComponent.arrow = skill;
                        m_SkillComponent.projectileSpeed = 5.0f;
                        m_SkillComponent.skillCooldown = 1.0f;
                        m_SkillComponent.SkillInit(playerController.Bow.baseAttackSpeed, playerController.Bow.targetPlatform, playerController.Bow.currentTarget.tag);
                        var bezierProjectile = m_SkillComponent as BezierProjectile;
                        bezierProjectile.prefab = bezierProjectile;
                        skill.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                        break;
                    case Data.EAttackType.Line :
                        skill.AddComponent<LineProjectile>();
                        m_SkillComponent = skill.GetComponent<LineProjectile>();
                        m_SkillComponent.isUsable = true;
                        m_SkillComponent.arrow = skill;
                        m_SkillComponent.projectileSpeed = 7.5f;
                        m_SkillComponent.skillCooldown = 1.5f;
                        m_SkillComponent.SkillInit(playerController.Bow.baseAttackSpeed, playerController.Bow.targetPlatform, playerController.Bow.currentTarget.tag);
                        var lineProjectile = m_SkillComponent as LineProjectile;
                        lineProjectile.prefab = lineProjectile;
                        skill.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                        break;
                    case Data.EAttackType.Rain :
                        skill.AddComponent<RainProjectile>();
                        m_SkillComponent = skill.GetComponent<RainProjectile>();
                        m_SkillComponent.isUsable = true;
                        m_SkillComponent.arrow = skill;
                        m_SkillComponent.projectileSpeed = 15.0f;
                        m_SkillComponent.skillCooldown = 2f;
                        m_SkillComponent.SkillInit(playerController.Bow.baseAttackSpeed, playerController.Bow.targetPlatform, playerController.Bow.currentTarget.tag);
                        var rainProjectile = m_SkillComponent as RainProjectile;
                        rainProjectile.prefab = rainProjectile;
                        rainProjectile.separatedCount = 10;
                        break;
                }
                playerController.Bow.skills[i] = m_SkillComponent;
                if (i == 0)
                {
                    continue;
                }
                var playerBow = playerController.Bow as PlayerBow;
                playerBow.coolTimeAnnounce[i - 1].image.sprite = skill.sprite;
            }
        }
    }
}
