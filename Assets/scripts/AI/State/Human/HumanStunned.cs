using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class HumanStunned : StunnedState 
    {
        public override void EnterState ()
        {
            Log.AI ("Enter Stunned State");
            base.EnterState ();
            aiCharacter.SetIdle ();
        }

    }
}
