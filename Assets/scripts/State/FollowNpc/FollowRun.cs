using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    //Move To Target Point
    public class FollowRun : MoveState
    {
        //一次性初始化代码
        public override void EnterState()
        {
            base.EnterState();
            aiCharacter.SetRun();
        } 
        public override IEnumerator RunLogic()
        {
            var myTransform = GetAttr().transform;
            var targetPlayer = ObjectManager.objectManager.GetMyPlayer().transform;
            var physic = myTransform.GetComponent<PhysicComponent>();
            while(!quit) {
                Vector3 dir = targetPlayer.position-myTransform.position;
                dir.y = 0;
                if(dir.sqrMagnitude < 9) {
                    aiCharacter.ChangeState(AIStateEnum.IDLE);
                    yield break;
                }

                dir.Normalize();
                var rotation = Quaternion.LookRotation(dir);
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, rotation, Mathf.Min(1, Time.deltaTime*10));

                physic.MoveSpeed(dir * 5 );
                yield return null ;
            }
        }
    }
}
