using UnityEngine;
using System.Collections;

namespace MyLib
{
    public class TankMoveAndShoot : MoveShootState
    {
        float moveSpeed = 0;
        float walkSpeed = 8.0f;
        float rotateSpeed = 500;
        float speedSmoothing = 10;
        float WindowTime = 0.5f;

        public override void EnterState()
        {
            base.EnterState();
            inSkill = false;
        }

        public override IEnumerator RunLogic()
        {
            GetAttr().StartCoroutine(Move());
            yield return null;
        }

        IEnumerator Move()
        {
            var playerMove = GetAttr().GetComponent<MoveController>();
            var vcontroller = playerMove.vcontroller;
            var camRight = playerMove.camRight;
            var camForward = playerMove.camForward;
            var physics = playerMove.GetComponent<TankPhysicComponent>();
            var first = true;
            while (!quit)
            {
                if (CheckEvent())
                {
                    yield break;
                }

                float v = 0;
                float h = 0;
                if (vcontroller != null)
                {
                    h = vcontroller.inputVector.x;//CameraRight 
                    v = vcontroller.inputVector.y;//CameraForward
                }
                bool isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;
                isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;
                if (!isMoving)
                {
                    if (!inSkill && !first)
                    {
                        aiCharacter.ChangeState(AIStateEnum.IDLE);
                        break;
                    } else
                    {
                        first = false;
                        yield return null;
                        continue;
                    }
                }

                Vector3 moveDirection = Vector3.zero;
                Vector3 targetDirection = h * camRight + v * camForward;
                if (targetDirection != Vector3.zero)
                {
                    moveDirection = targetDirection;
                    if (moveSpeed < walkSpeed * 0.3f)
                    {
                        moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * 2 * Mathf.Deg2Rad * Time.deltaTime, 1000);
                        moveDirection = moveDirection.normalized;
                    }
                    else
                    {
                        moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);         
                        moveDirection = moveDirection.normalized;
                    }
                }

                var curSmooth = speedSmoothing * Time.deltaTime;
                var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
                targetSpeed *= walkSpeed;
                moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
                var movement = moveDirection * moveSpeed;
                physics.MoveSpeed(movement);
                //没有使用技能则自动设置方向 有技能则最近设置方向
                /*
                if(!inSkill) {
                }
                */
                physics.TurnTo(moveDirection);
                yield return null;
            }
        }


        //检测输入shoot命令
        public override bool CheckNextState(AIStateEnum next)
        {
            if (next == AIStateEnum.COMBAT)
            {
                GetAttr().StartCoroutine(SkillState());
                return false;
            }
            return base.CheckNextState(next);
        }

        private bool inSkill = false;

        IEnumerator SkillState()
        {
            if (inSkill)
            {
                yield break;
            }
            inSkill = true;
            var st = new TankShoot();
            aiCharacter.AddTempState(st);
            yield return GetAttr().StartCoroutine(st.RunLogic());
            inSkill = false;
        }
    }
}