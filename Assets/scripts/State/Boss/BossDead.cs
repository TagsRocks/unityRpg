using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class BossDead : MonsterDead 
    {
        public override void EnterState()
        {
            base.EnterState();
            MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.BossDead);
        }
    }

}