using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class FollowNpcAI : AIBase 
    {
        void Awake() {

            attribute = GetComponent<NpcAttribute> ();
            ai = new FollowNpcCharacter ();
            ai.attribute = attribute;
            ai.AddState (new FollowIdle ());
            ai.AddState (new FollowRun ());
            ai.AddState(new FollowDead());
        }

        void Start ()
        {
            ai.ChangeState (AIStateEnum.IDLE);
        }

    }

}