using _02._Scripts.Commonness;
using UnityEngine;
using _02._Scripts.EnemyScripts;
using Unity.VisualScripting;

namespace _02._Scripts.Player_Control
{
    public class PlayerBow : BaseBow
    {
        protected override void Start()
        {
            targetPlatform = "EnemyPlatform";
            base.Start();
        }
    }
}
