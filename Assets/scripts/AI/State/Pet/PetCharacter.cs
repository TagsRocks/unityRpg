using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    //火焰陷阱不支持移动
    public class PetCharacter : AICharacter
    {
        public override void SetRun() {
        }
        public override void SetIdle() {
            SetAni ("idle", 1, WrapMode.Loop);
        }
    }
}
