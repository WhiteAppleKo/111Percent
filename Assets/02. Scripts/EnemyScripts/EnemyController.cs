using System;
using _02._Scripts.Commonness;
using Unity.VisualScripting;
using UnityEngine;

namespace _02._Scripts.EnemyScripts
{
    public class EnemyController : BaseController
    {
        private enum EState
        {
            BasicAttack,
            Move,
            SkillAttack,
        }
    }
}
