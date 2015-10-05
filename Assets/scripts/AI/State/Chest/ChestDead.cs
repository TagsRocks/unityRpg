using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class ChestDead : DeadState 
    {
        public override void EnterState ()
        {
            base.EnterState ();
            GetAttr().animation.CrossFade ("opening");
            GetAttr().IsDead = true;
            GetAttr().OnlyShowDeadEffect();
        }
    }

}