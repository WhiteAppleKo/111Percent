using System;
using _02._Scripts.Commonness;
using UnityEngine;
using UnityEngine.UI;

namespace _02._Scripts.UI
{
    public class HPBar : MonoBehaviour
    {
        public BaseController bc;

        private Slider m_Slider;
        private float m_Ratio;
        private int m_Value;
        private Text m_Text;
        private void Start()
        {
            bc.Health.Events.onValueChanged += ChangeValue;
            m_Slider = GetComponent<Slider>();
            m_Text = GetComponentInChildren<Text>();
        }

        private void OnDisable()
        {
            bc.Health.Events.onValueChanged -= ChangeValue;
        }

        private void ChangeValue(int prev, int current)
        {
            m_Ratio = bc.Health.Ratio;
            m_Value = current;
            
            m_Slider.value = m_Ratio;
            m_Text.text = m_Value.ToString();
        }
    }
}
