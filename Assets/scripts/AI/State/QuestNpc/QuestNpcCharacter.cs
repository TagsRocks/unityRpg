using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class QuestNpcCharacter : AICharacter
    {
        public override void SetIdle() {
            SetAni ("idle", 1, WrapMode.Loop);
        }
    }

}