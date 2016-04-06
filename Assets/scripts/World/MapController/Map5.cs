using UnityEngine;
using System.Collections;
using System;

namespace MyLib
{

    public class Map5 :CScene
    {
        public override bool ShowTeamColor
        {
            get
            {
                return true;
            }
        }

        public override bool IsEnemy(GameObject a, GameObject b)
        {
            return a != b && b.tag == GameTag.Player;
        }

        public override bool IsNet
        {
            get
            { 
                return true;
            }
        }
        public override bool IsRevive
        {
            get
            {
                return true;
            }
        }


        private NetworkScene netScene;

        protected override void Awake()
        {
            base.Awake();
            netScene = gameObject.AddComponent<NetworkScene>();
            gameObject.AddComponent<ScoreManager>();
        }

        public override void BroadcastMsg(CGPlayerCmd.Builder cmd)
        {
            netScene.BroadcastMsg(cmd);
        }

    }

}
