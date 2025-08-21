using System;
using System.Collections;
using _02._Scripts.Commonness.Attack.Arrow;
using _02._Scripts.Commonness.Attack.SkillType;
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
            Bow.currentTarget = enemy;
            if (animator != null)
            {
                Bow.animator = animator;
            }
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
        private bool _eventsBound;
        private Coroutine _waitBindCo;
        protected virtual void OnEnable()
        {
            TryBindGameStartEvent();
        }

        private void TryBindGameStartEvent()
        {
            // 이미 묶였으면 중복 방지
            if (_eventsBound) return;

            // GameManager가 아직 없으면 기다렸다가 재시도
            if (GameManager.Instance == null)
            {
                if (_waitBindCo == null)
                    _waitBindCo = StartCoroutine(Co_WaitAndBind());
                return;
            }
            
            GameManager.Instance.gameStartEvent -= SetEvents;
            GameManager.Instance.gameStartEvent += SetEvents;
            _eventsBound = true;
        }
        
        private IEnumerator Co_WaitAndBind()
        {
            // 다음 프레임/몇 프레임 정도만 기다렸다가 Instance 생기면 구독
            while (GameManager.Instance == null)
                yield return null;

            _waitBindCo = null;
            TryBindGameStartEvent();
        }
        
        private void UnbindGameStartEvent()
        {
            if (!_eventsBound) return;
            if (GameManager.Instance != null)
                GameManager.Instance.gameStartEvent -= SetEvents;
            _eventsBound = false;
        }
        protected virtual void OnDisable()
        {
            UnbindGameStartEvent();          // gameStartEvent 해제 (대기 코루틴도 Stop)
            UnsetMoveAsset();                // onSkillUsed 해제
            UnsetNonAttackSkills();          // nonAttackSkill.onUsed 해제

            if (Health?.Events != null)      // 내 체력 이벤트 해제
                Health.Events.onMinReached -= Die;

            if (enemy?.Health?.Events != null) // 상대 체력 이벤트 해제
                enemy.Health.Events.onMinReached -= Victory;

            onMove -= SetMoveTrigger;

            if (_waitBindCo != null) { StopCoroutine(_waitBindCo); _waitBindCo = null; }

            // 다음 라운드 대비 초기화
            if (Bow != null) Bow.currentTarget = null;
            m_IsSetMoveAsset = false;
        }
        
        private void UnsetMoveAsset()
        {
            if (Bow == null || Bow.skills == null || Bow.skills.Length <= 2) return;

            if (Bow.skills[1] != null) Bow.skills[1].onSkillUsed -= MoveAsset1;
            if (Bow.skills[2] != null) Bow.skills[2].onSkillUsed -= MoveAsset2;
        }

        private void SetEvents(bool isGameStart)
        {
            Health.Events.onMinReached += Die;
            enemy.Health.Events.onMinReached += Victory;
            onMove += SetMoveTrigger;
        }
        
        private void SetMoveTrigger(bool isMoving)
        {
            if (animator == null)
            {
                return;
            }
            animator.SetBool("isMoving", isMoving);
        }
        
        protected virtual void Die(int prev, int current)
        {
            GameManager.Instance.gameStartEvent -= SetEvents;
            StopAllCoroutines();
            animator.SetBool("Die", true);
            haveBow = false;
            Bow.skills[1].onSkillUsed -= MoveAsset1;
            Bow.skills[2].onSkillUsed -= MoveAsset2;
            UnsetNonAttackSkills();
        }

        protected virtual void Victory(int  prev, int current)
        {
            GameManager.Instance.gameStartEvent -= SetEvents;
            StopAllCoroutines();
            animator.SetBool("Victory", true);
            haveBow = false;
            Bow.skills[1].onSkillUsed -= MoveAsset1;
            Bow.skills[2].onSkillUsed -= MoveAsset2;
            UnsetNonAttackSkills();
        }
        protected virtual void OnDestroy()
        {
            UnsetNonAttackSkills(); // 씬 언로드/즉시 파괴 케이스까지 커버
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

        public void SetNonAttackSkills()
        {
            if (nonAttackSkill == null)
            {
                return;
            }

            Skill decoyArrow = Bow.skills[0].Clone();
            nonAttackSkill.SkillSet(Bow.currentTarget, self, decoyArrow, Bow.targetPlatform);
            nonAttackSkill.onUsed += CoolDown;
            nonAttackSkill.onUsed += MoveAsset3;
        }
        
        private void UnsetNonAttackSkills()
        {
            if (nonAttackSkill == null) return;
            nonAttackSkill.onUsed -= CoolDown;
            nonAttackSkill.onUsed -= MoveAsset3;
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
                    animator.SetTrigger("isAttack");
                    Bow.Attack(this, Bow.skills[0]);
                }
            }
        }
    }
}
