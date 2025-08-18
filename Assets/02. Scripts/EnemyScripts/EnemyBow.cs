using _02._Scripts.Commonness;
using UnityEngine;

namespace _02._Scripts.EnemyScripts
{
    public class EnemyBow : BaseBow
    {
        protected override void Start()
        {
            targetPlatform = "PlayerPlatform";
            base.Start();
        }
    }
}


