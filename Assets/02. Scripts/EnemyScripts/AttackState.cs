using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Commonness.Attack;
using _02._Scripts.Commonness.Attack.SkillType;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02._Scripts.EnemyScripts
{
    public class AttackState : BaseState
    {
        public float attackTime;
        private List<Skill> m_UsableSkills;
        private Coroutine m_SelectAttackCo;

        public override void Enter()
        {
            m_SelectAttackCo = StartCoroutine(co_SelectAttack());
        }

        public override void Exit()
        {
            Controller.ReInitializeWeight();
            if (m_SelectAttackCo != null)
            {
                StopCoroutine(m_SelectAttackCo);
                m_SelectAttackCo = null;
            }
        }

        private IEnumerator co_SelectAttack()
        {
            var duration = attackTime;
            while (duration > 0)
            {
                m_UsableSkills = new List<Skill>();
                for (int i = 1; i < Controller.Bow.skills.Length; i++)
                {
                    if (Controller.Bow.skills[i].isUsable)
                    {
                        m_UsableSkills.Add(Controller.Bow.skills[i]);
                    }
                }

                if (m_UsableSkills.Count > 0)
                {
                    Controller.Bow.Attack(Controller, m_UsableSkills[Random.Range(0, m_UsableSkills.Count)]);
                }
                duration -= Time.deltaTime;
                yield return null;
            }
            Controller.SelectAction();
        }
    }
}
