using System;
using _02._Scripts.Commonness;
using Singleton;
using UnityEngine;

namespace _02._Scripts
{
    public class GameManager : SingletonBase<GameManager>
    {
        public Action<bool> gameStartEvent;
        // 0번 플레이어 1번 에너미
        public BaseController[] controllers;
        private float m_GameTimer;
        private void Awake()
        {
            m_GameTimer = 0.0f;
            Time.timeScale = 0;
        }

        private void Update()
        {
            m_GameTimer += Time.unscaledDeltaTime;
            if (m_GameTimer >= 5)
            {
                Time.timeScale = 1;
                gameStartEvent?.Invoke(true);
            }
        }
    }
}
