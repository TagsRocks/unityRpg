using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class HumanJump : JumpState
    {
        const float RushSpeed = 5;
        const float UpSpeed = 10;
        const float gravity = 10;
        const float dropGravity = 20;
        const float friction = 20;

        public override void EnterState()
        {
            base.EnterState();
            aiCharacter.SetRun();
            var ab = GetAttr().GetComponent<AIBase>();
            ab.ignoreFallCheck = true;
            var ret = GetAttr().GetComponent<PhysicComponent>().EnterSkillMoveState();
            GetAttr().GetComponent<ShadowComponent>().LockShadowPlane();
        }
        public override void ExitState()
        {
            var ab = GetAttr().GetComponent<AIBase>();
            ab.ignoreFallCheck = false;
            GetAttr().GetComponent<PhysicComponent>().ExitSkillMove();
            GetAttr().GetComponent<ShadowComponent>().UnLockShadowPlane();
            base.ExitState();
        }

        public override IEnumerator RunLogic()
        {

            var playerForward = GetAttr().transform.forward;
            var physics = GetAttr().GetComponent<PhysicComponent>();
            var forwardSpeed = RushSpeed;
            var upSpeed = UpSpeed;

            var controller = GetAttr().GetComponent<CharacterController>();
            var passTime = 0.0f;

            while (!quit)
            {
                var movement = playerForward * forwardSpeed + Vector3.up * upSpeed;
                physics.JumpMove(movement);

                if(upSpeed <= 0) {
                    upSpeed -= dropGravity * Time.deltaTime;
                }else {
                    upSpeed -= gravity * Time.deltaTime;
                }
                passTime += Time.deltaTime;
                yield return null;
                if (passTime >= 0.2f)
                {
                    if ((controller.collisionFlags & CollisionFlags.Below) != 0)
                    {
                        forwardSpeed -= friction * Time.deltaTime;
                        if (forwardSpeed <= 0)
                        {
                            break;
                        }
                        forwardSpeed = Mathf.Max(0, forwardSpeed);
                    }
                }
            }


            aiCharacter.ChangeState(AIStateEnum.IDLE);
        }
    }

}