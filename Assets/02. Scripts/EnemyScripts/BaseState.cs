using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02._Scripts.EnemyScripts
{
    public abstract class BaseState : MonoBehaviour
    {
        protected EnemyController Controller { get; private set; }
        
        public virtual void Initialize(EnemyController controller)
        {
            Controller = controller;
        }

        public abstract void Enter();
    
        public abstract void Exit();
    }
}
