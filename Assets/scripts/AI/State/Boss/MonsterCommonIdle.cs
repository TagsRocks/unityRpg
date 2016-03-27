﻿using UnityEngine;
using System.Collections;

namespace MyLib
{
    /// <summary>
    /// 通用怪物Idle状态 
    /// </summary>
    public class MonsterCommonIdle : IdleState 
    {
        float RunSpeed = 5;
        float directionChangeInterval = 3;
        public MonsterCommonIdle() {
            type = AIStateEnum.IDLE;
            layer = 1;
        }

        IEnumerator CheckFarAway()
        {
            var oriPos = GetAttr().OriginPos;
            while (!quit)
            {
                yield return new WaitForSeconds(1);
                if (!quit)
                {
                    var curPos = GetAttr().transform.position;
                    var dis = Pathfinding.AstarMath.SqrMagnitudeXZ(curPos, oriPos);
                    if (dis > 25)
                    {
                        GetAttr().transform.position = oriPos;
                    }
                }
            }
        }

        public override void EnterState()
        {
            base.EnterState();
            aiCharacter.SetIdle();
        }

        public override IEnumerator RunLogic()
        {
            GetAttr().StartCoroutine(CheckFarAway());
            yield return GetAttr().StartCoroutine(NewHeading());
        }

        bool CheckTarget()
        {
            if (CheckEvent())
            {
                return true;
            }
            
            GameObject player = ObjectManager.objectManager.GetMyPlayer();
            if (player && !player.GetComponent<NpcAttribute>().IsDead)
            {
                float distance = (player.transform.position - GetAttr().transform.position).magnitude;
                if (distance < GetAttr().ApproachDistance)
                {
                    aiCharacter.ChangeState(AIStateEnum.COMBAT);
                    return true;
                }
            }
            return false;
        }
        
        IEnumerator NewHeadingRoutine()
        {
            while (!quit)
            {
                var heading = Random.Range(0, 360);
                var targetRotation = new Vector3(0, heading, 0);
                Quaternion qua = Quaternion.Euler(targetRotation);
                Vector3 dir = (qua * Vector3.forward);
                
                RaycastHit hitInfo;
                if (!Physics.Raycast(GetAttr().transform.position, dir, out hitInfo, 3))
                {
                    break;
                }
                yield return null;
            }
        }
        
        IEnumerator NewHeading()
        {
            var physics = GetAttr().GetComponent<PhysicComponent>();
            while (!quit)
            {
                aiCharacter.SetIdle();
                float passTime = Random.Range(1, 3);
                while (passTime > 0)
                {
                    if (CheckTarget())
                    {
                        yield break;
                    }
                    passTime -= Time.deltaTime;
                    yield return null;
                }
                yield return GetAttr().StartCoroutine(NewHeadingRoutine());
                
                
                aiCharacter.SetRun();
                passTime = directionChangeInterval;

                while (passTime > 0)
                {
                    if (CheckTarget())
                    {
                        yield break;
                    }
                    passTime -= Time.deltaTime;
                    var forward = GetAttr().transform.TransformDirection(Vector3.forward);
                    physics.MoveSpeed(forward * RunSpeed);
                    yield return null;
                }
                
                yield return null;
            }

        }

    }
}