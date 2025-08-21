using System;
using System.Collections;
using _02._Scripts.Commonness;
using _02._Scripts.Commonness.Attack;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _02._Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public Action<bool> gameStartEvent;
        // 0번 플레이어 1번 에너미
        public BaseController[] controllers;
        public SelectSkillData skillSetter;
        private float m_GameTimer;
        private float m_GameStartCount = 4.0f;
        public Text gameStartCounter;

        private void Awake()
        {
            skillSetter.MakeAndSetSkills();
            Time.timeScale = 0;
            StartCoroutine(co_GameStartCount());
        }

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }else
            {
                Destroy(gameObject);
                return;
            }
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
                if (0 >= (int)time)
                {
                    gameStartCounter.text = "시작!";
                }
                else
                {
                    gameStartCounter.text = ((int)time).ToString("F0");
                }
                yield return null;
            }
            Time.timeScale = 1;
            gameStartEvent?.Invoke(true);
            gameStartCounter.gameObject.SetActive(false);
        }

        public void LoadTitle()
        {
            SceneManager.LoadScene(0);
        }
    }
}
