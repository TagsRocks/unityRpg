using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class QuestIdle : IdleState
    {
        public override void EnterState()
        {
            base.EnterState();
            aiCharacter.SetIdle();
        }
    }
}
