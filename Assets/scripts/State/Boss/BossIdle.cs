using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class BossIdle : IdleState
    {
        bool isFirst = true;

        public override void EnterState()
        {
            base.EnterState();
            aiCharacter.SetIdle();
            if (isFirst)
            {
                MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.BossSpawn);
            }
        }

        IEnumerator WaitForSpeakOver()
        {
            bool ret = false;
            EventDel del = delegate(MyEvent evt)
            {
                ret = true;
            }; 
            MyEventSystem.myEventSystem.RegisterEvent(MyEvent.EventType.SpeakOver, del);
            while (!ret)
            {
                yield return new WaitForSeconds(1);
            }
            MyEventSystem.myEventSystem.dropListener(MyEvent.EventType.SpeakOver, del);
            isFirst = false;
        }

        public override IEnumerator RunLogic()
        {
            if (isFirst)
            {
                yield return GetAttr().StartCoroutine(WaitForSpeakOver());
            }
            //aiCharacter.ChangeState(AIStateEnum.IDLE, 1);
            while (true)
            {
                yield return new WaitForSeconds(2);
                GameObject player = ObjectManager.objectManager.GetMyPlayer();
                if (player && !player.GetComponent<NpcAttribute>().IsDead)
                {
                    aiCharacter.ChangeState(AIStateEnum.COMBAT);
                }
            }
        }

    }
}