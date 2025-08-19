using System;
using System.Linq;
using _02._Scripts.Commonness;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02._Scripts.EnemyScripts
{
    public class EnemyController : BaseController
    {
        public BaseState[] states;
        public float weight;
        
        private BaseState m_CurrentState;
        [SerializeField] 
        private float m_TotalWeight = 100.0f;
        private bool m_ReadySkill;
        private bool m_StateDone = false;

        private void Start()
        {
            foreach (var state in states)
            {
                state.Initialize(this);
            }
            ChangeState<AttackState>();
            foreach (var state in states)
            {
                if (state is AttackState == false)
                {
                    state.gameObject.gameObject.SetActive(false);
                }
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            states = GetComponentsInChildren<BaseState>();
        }

        protected override void InitializeStatus()
        {
            base.InitializeStatus();
        }

        public void ReInitializeWeight()
        {
            var myHealth = m_Self.Health?.Ratio;
            var playerHealth = GameManager.Instance.controllers[0].Health?.Ratio;
            var healthGap = myHealth - playerHealth;
            if (healthGap > 0.05)
            {
                weight--;
            }
            else if (healthGap < -0.05)
            {
                weight++;
            }
        }
        public void SelectAction()
        {
            float randomValue = Random.Range(0.0f, m_TotalWeight);
            if (randomValue < weight)
            {
                ChangeState<MoveState>();
            }
            else
            {
                ChangeState<AttackState>();
            }
        }
        
        public void ChangeState<T>() where T : BaseState
        {
            m_CurrentState?.Exit();
            if (m_CurrentState != null)
            {
                m_CurrentState.gameObject.SetActive(false);
            }

            m_CurrentState = states.FirstOrDefault(state => state is T);
            if (m_CurrentState != null)
            {
                m_CurrentState.gameObject.SetActive(true);
                if (m_CurrentState == null)
                {
                    Debug.LogError($"{gameObject.name} 상태 Error. {typeof(T).Name} 없음");
                    m_CurrentState = states[0];
                }

                m_CurrentState?.Enter();
            }
        }
        
        /*private void SetStartState()
        {
            int startState = SelectAction();
            for (int i = 0; i < states.Length; i++)
            {
                if (i == startState)
                {
                    states[i].gameObject.SetActive(true);
                }
                else
                {
                    states[i].gameObject.SetActive(false);
                }
            }
        }*/
    }
}
