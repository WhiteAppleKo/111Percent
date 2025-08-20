using System.Collections;
using _02._Scripts.Commonness;
using _02._Scripts.Commonness.Attack.SkillType;
using UnityEngine;
using UnityEngine.UI;

namespace _02._Scripts.Player_Control
{
    public class PlayerBow : BaseBow
    {
        public PlayerController.CoolTimeAnnounce[] coolTimeAnnounce;
        public void ActiveSkill(int skillIndex)
        {
            if (skills[skillIndex].isUsable == false)
            {
                return;
            }
            Attack(GameManager.Instance.controllers[0], skills[skillIndex]);
            animator.SetTrigger("isAttack");
        }
        protected override void Start()
        {
            targetPlatform = "EnemyPlatform";
            base.Start();
        }

        protected override IEnumerator co_CoolDown(Skill skill)
        {
            skill.isUsable = false;
            var leftCooldown = skill.attackData.attackCooldown;
            int? index = FindSkillIndex(skill);
            Color block;
            if (index.HasValue)
            {
                coolTimeAnnounce[index.Value - 1].text.text = (leftCooldown).ToString("F1");
                block = coolTimeAnnounce[index.Value - 1].image.color;
                block = Color.gray;
                coolTimeAnnounce[index.Value - 1].image.color = block;
            }
            while (leftCooldown >= 0)
            {
                leftCooldown -= Time.deltaTime;
                if (index.HasValue)
                {
                    coolTimeAnnounce[index.Value - 1].text.text = (leftCooldown).ToString("F1");
                }
                yield return null;
            }
            skill.isUsable = true;
            if (index.HasValue)
            {
                coolTimeAnnounce[index.Value - 1].text.text = " ";
                block = Color.white;
                coolTimeAnnounce[index.Value - 1].image.color = block;
            }
        }
    }
}
