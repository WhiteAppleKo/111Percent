using System;
using System.Collections;
using _02._Scripts.Commonness;
using _02._Scripts.Commonness.Attack;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02._Scripts
{
    public class GameManager : SingletonBase<GameManager>
    {
        public Action<bool> gameStartEvent;
        // 0번 플레이어 1번 에너미
        public BaseController[] controllers;
        public SelectSkillData skillSetter;
        private float m_GameTimer;
        private float m_GameStartCount = 3.0f;

        protected override void Awake()
        {
            base.Awake();
            skillSetter.MakeAndSetSkills();
            Time.timeScale = 0;
            StartCoroutine(co_GameStartCount());
        }

        private void Update()
        {
            m_GameTimer += Time.unscaledDeltaTime;
        }

        private IEnumerator co_GameStartCount()
        {
            var time = m_GameStartCount;
            while (time > 0)
            {
                time -= Time.unscaledDeltaTime;
                yield return null;
            }
            Time.timeScale = 1;
            gameStartEvent?.Invoke(true);
        }

        public void LoadTitle()
        {
            SceneManager.LoadScene(0);
        }
    }
}
