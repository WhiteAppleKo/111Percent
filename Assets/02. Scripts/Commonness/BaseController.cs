using System;
using System.Collections;
using UnityEngine;

namespace _02._Scripts.Commonness
{
    public class BaseController : MonoBehaviour
    {
        public ClampInt Health { get; private set; }
        public BaseController self;
        public NonAttackSkill.NonAttackSkill nonAttackSkill;
        public BaseBow Bow { get; private set; }
        public int maxHealth;
        public bool isMoving = false;
        public bool haveBow;
        
        public void TakeDamage(int damage)
        {
            Health.Decrease(damage);
        }

        public void HitDisarmArrow(float duration)
        {
            StartCoroutine(co_Disarm(duration));
        }

        private IEnumerator co_Disarm(float duration)
        {
            haveBow = false;
            float time = 0.0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            haveBow = true;
        }
        
        protected virtual void Awake()
        {
            InitializeStatus();
            SetNonAttackSkills();
            Bow = GetComponentInChildren<BaseBow>();
        }

        protected virtual void InitializeStatus()
        {
            self = this;
            Health = new ClampInt(min: 0, max: maxHealth, initial: maxHealth);
        }

        private void OnEnable()
        {
            Health.Events.onMinReached += Die;
        }

        private void OnDisable()
        {
            Health.Events.onMinReached -= Die;
        }
        
        private void Die(int prev, int current)
        {
            Debug.Log($"{name} 사망");
        }
        private void MoveAsset1()
        {
            Vector2 dir = Bow.currentTarget.transform.position - transform.position;
            dir.Normalize();
            var myPos = transform.position;
            myPos.x += dir.x;
            transform.position = myPos;
        }

        private void MoveAsset2()
        {
            Vector2 dir = Bow.currentTarget.transform.position - transform.position;
            dir.Normalize();
            var myPos = transform.position;
            myPos.x += -dir.x;
            transform.position = myPos;
        }

        private void SetMoveAsset()
        {
            if (Bow.skills[1] == null)
            {
                return;
            }
            Bow.skills[1].onSkillUsed += MoveAsset1;
            Bow.skills[2].onSkillUsed += MoveAsset2;
        }

        private void SetNonAttackSkills()
        {
            if (nonAttackSkill == null)
            {
                return;
            }
            nonAttackSkill.SkillSet(Bow.currentTarget, self);
            nonAttackSkill.onUsed += CoolDown;
        }

        public void ActiveNonAttackSkill()
        {
            nonAttackSkill.ActiveSkill();
        }
        private void CoolDown(float cooldown)
        {
            StartCoroutine(co_CoolDown(cooldown));
        }

        private IEnumerator co_CoolDown(float cooldown)
        {
            nonAttackSkill.isUsable = false;
            var time = cooldown;
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            nonAttackSkill.isUsable = true;
        }
        
        private bool m_IsSetMoveAsset;
        protected virtual void Update()
        {
            if (haveBow == false)
            {
                Bow.gameObject.SetActive(false);
            }
            else
            {
                Bow.gameObject.SetActive(true);
            }
            if (Bow != null && m_IsSetMoveAsset == false)
            {
                SetMoveAsset();
                m_IsSetMoveAsset = true;
            }

            if (nonAttackSkill != null)
            {
                SetNonAttackSkills();
            }
            if(isMoving == false)
            {
                if (Bow.skills[0].isUsable)
                {
                    Bow.Attack(this, Bow.skills[0]);
                }
            }
        }
    }
}
