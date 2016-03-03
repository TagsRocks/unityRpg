using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class HumanJump : JumpState
    {
        const float RushSpeed = 5;
        const float AddSpeed = 20;
        const float MaxSpeed = 8;


        const float UpSpeed = 10;
        const float gravity = 10;
        const float dropGravity = 20;
        const float friction = 20;

        public override bool CheckNextState(AIStateEnum next)
        {
            if(next == AIStateEnum.IDLE) {
                return true;
            }
            if(next == AIStateEnum.COMBAT) {
                GetAttr().StartCoroutine(SkillState());
                return false;
            }
            return false;
        }

        string GetAttackAniName()
        {
            return "rslash_1";
        }


        IEnumerator WaitForAttackAnimation(Animation animation)
        {
            var rd = Random.Range(1, 3);
            BackgroundSound.Instance.PlayEffect("onehandswinglarge" + rd);
            var skillStateMachine = SkillLogic.CreateSkillStateMachine(GetAttr().gameObject, activeSkill.skillData, GetAttr().transform.position);
            Log.AI("Wait For Combat Animation");
            float passTime = 0;
            do
            {
                if (passTime >= animation [GetAttackAniName()].length * 0.8f / animation [GetAttackAniName()].speed)
                {
                    break;
                }
                passTime += Time.deltaTime;

                yield return null;
            } while(!quit);
            
            Log.Ani("Animation is Playing stop ");
            skillStateMachine.Stop();
        }

        SkillFullInfo activeSkill = null;
        private bool inSkill = false;
        IEnumerator SkillState() {
            if(inSkill) {
                yield break;
            }

            inSkill = true;

            activeSkill = GetAttr().GetComponent<SkillInfoComponent>().GetActiveSkill();
            var attackAniName = GetAttackAniName(); 
            var realAttackTime = activeSkill.skillData.AttackAniTime / GetAttr().GetSpeedCoff();
            var rate = GetAttr().animation [attackAniName].length / realAttackTime;
            PlayAni(attackAniName, rate, WrapMode.Once);
            yield return GetAttr().StartCoroutine(WaitForAttackAnimation(GetAttr().animation));

            aiCharacter.SetRun();
            //等动画彻底播放完成
            yield return new WaitForSeconds(0.1f);
            inSkill = false;
        }


        public override void EnterState()
        {
            base.EnterState();
            activeSkill = null;
            aiCharacter.SetRun();
            var ab = GetAttr().GetComponent<AIBase>();
            ab.ignoreFallCheck = true;
            var ret = GetAttr().GetComponent<PhysicComponent>().EnterSkillMoveState();
            GetAttr().GetComponent<ShadowComponent>().LockShadowPlane();
            BackgroundSound.Instance.PlayEffect("fall4");
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
            var attr = GetAttr();
            var playerForward = GetAttr().transform.forward;
            var physics = GetAttr().GetComponent<PhysicComponent>();
            //var forwardSpeed = RushSpeed;
            attr.JumpForwardSpeed = RushSpeed;
            var upSpeed = UpSpeed;

            var controller = GetAttr().GetComponent<CharacterController>();
            var passTime = 0.0f;
            var soundYet = false;

            var playerMove = GetAttr().GetComponent<MoveController>();
            var vcontroller = playerMove.vcontroller;
            var camRight = playerMove.camRight;
            var camForward = playerMove.camForward;

            while (!quit)
            {
                if (CheckEvent())
                {
                    break;
                }

                float v = 0;
                float h = 0;
                if (vcontroller != null)
                {
                    h = vcontroller.inputVector.x;//CameraRight 
                    v = vcontroller.inputVector.y;//CameraForward
                }

                var targetDirection = h * camRight + v * camForward;
                var curDir = playerForward;
                //targetDirection.Normalize();
                var addOrMinus = Vector3.Dot(curDir, targetDirection);
                var val = Mathf.Abs(addOrMinus);
                val = Mathf.Min(1, val);
                var sign = Mathf.Sign(addOrMinus);

                var forwardSpeed = attr.JumpForwardSpeed;
                //没有落地的时候可以控制
                if ((controller.collisionFlags & CollisionFlags.Below) == 0)
                {
                    if (sign > 0)
                    {
                        forwardSpeed += val * AddSpeed * Time.deltaTime;
                        forwardSpeed = Mathf.Min(MaxSpeed, forwardSpeed);
                    } else
                    {
                        forwardSpeed -= val * AddSpeed * Time.deltaTime;
                        forwardSpeed = Mathf.Max(0, forwardSpeed);
                    }
                }
                attr.JumpForwardSpeed = forwardSpeed;

                var movement = playerForward * forwardSpeed + Vector3.up * upSpeed;
                physics.JumpMove(movement);



                if (upSpeed <= 0)
                {
                    upSpeed -= dropGravity * Time.deltaTime;
                } else
                {
                    upSpeed -= gravity * Time.deltaTime;
                }
                passTime += Time.deltaTime;
                yield return null;
                if (passTime >= 0.2f)
                {
                    if ((controller.collisionFlags & CollisionFlags.Below) != 0)
                    {
                        if (!soundYet)
                        {
                            soundYet = true;
                            BackgroundSound.Instance.PlayEffect("fall1");
                        }
                        forwardSpeed -= friction * Time.deltaTime;
                        forwardSpeed = Mathf.Max(0, forwardSpeed);
                        attr.JumpForwardSpeed = forwardSpeed;
                        if (forwardSpeed <= 0)
                        {
                            break;
                        }
                    }
                }
            }

            while(inSkill) {
                yield return null;
            }
            aiCharacter.ChangeState(AIStateEnum.IDLE);
        }
    }

}