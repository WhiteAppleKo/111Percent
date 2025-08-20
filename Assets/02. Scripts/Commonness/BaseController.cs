using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _02._Scripts.Commonness
{
    public class BaseController : MonoBehaviour
    {
        public Text text;
        public Animator animator;
        public ClampInt Health { get; private set; }
        public BaseController self;
        public BaseController enemy;
        public NonAttackSkill.NonAttackSkill nonAttackSkill;
        public BaseBow Bow { get; private set; }
        public int maxHealth;
        public Action<bool> onMove;
        public bool isMoving;
        public bool haveBow;
        public bool isTotem;
        
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
            Bow = GetComponentInChildren<BaseBow>();
            if (animator != null)
            {
                Bow.animator = animator;
            }
            SetNonAttackSkills();
            self = this;
        }

        protected virtual void InitializeStatus()
        {
            self = this;
            Health = new ClampInt(min: 0, max: maxHealth, initial: maxHealth);
            if (isTotem == false)
            {
                text.text = Health.Current.ToString();
            }
        }

        private void OnEnable()
        {
            Health.Events.onMinReached += Die;
            onMove += SetMoveTrigger;
        }

        private void OnDisable()
        {
            Health.Events.onMinReached -= Die;
            onMove -= SetMoveTrigger;
        }
        
        private void SetMoveTrigger(bool isMoving)
        {
            if (animator == null)
            {
                return;
            }
            animator?.SetBool("isMoving", isMoving);
        }
        
        private void Die(int prev, int current)
        {
            animator.SetBool("Die", true);
            enemy.animator.SetBool("Victory", true);
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
        
        private void MoveAsset3(float f)
        {
            Vector2 dir = Bow.currentTarget.transform.position - transform.position;
            dir.Normalize();
            var myPos = transform.position;
            myPos.x += -dir.x * 2;
            transform.position = myPos;
        }

        private void SetMoveAsset()
        {
            if (isTotem)
            {
                return;
            }
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
            nonAttackSkill.onUsed += MoveAsset3;
        }

        public void ActiveNonAttackSkill()
        {
            nonAttackSkill.ActiveSkill();
        }
        protected void CoolDown(float cooldown)
        {
            StartCoroutine(co_CoolDown(cooldown));
        }
        

        protected virtual IEnumerator co_CoolDown(float cooldown)
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
                return;
            }
            
            if (Bow != null && m_IsSetMoveAsset == false)
            {
                if (isTotem)
                {
                    m_IsSetMoveAsset = true;
                    return;
                }
                SetMoveAsset();
                m_IsSetMoveAsset = true;
            }
            
            if(isMoving == false)
            {
                if (Bow.skills[0].isUsable)
                {
                    animator?.SetTrigger("isAttack");
                    Bow.Attack(this, Bow.skills[0]);
                }
            }
        }
    }
}
