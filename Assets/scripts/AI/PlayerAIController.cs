
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ChuMeng
{
    [RequireComponent(typeof(AnimationController))]
    [RequireComponent(typeof(PlayerSync))]
    public class PlayerAIController : AIBase
    {
        /// <summary>
        /// 从配置代码文件中读取用于所有的职业角色 例如  Config.cs  Config.config.xxxx
        /// </summary>
        float KnockBackTime
        {
            get
            {
                return 0.2f;
            }
        }

        float StopKnockTime
        {
            get
            {
                return 0.1f;
            }
        }

        float KnockBackSpeed
        {
            get
            {
                return 10;
            }
        }

        float UpSpeed
        {
            get
            {
                return 1;
            }
        }

        float Gravity
        {
            get
            {
                return 20;
            }
        }


        void Awake()
        {
            attribute = GetComponent<NpcAttribute>();

            ai = new HumanCharacter();
            ai.attribute = attribute;
            ai.AddState(new HumanIdle());
            ai.AddState(new HumanMove());
            ai.AddState(new HumanCombat());
            ai.AddState(new HumanSkill());
            ai.AddState(new HumanDead());
            ai.AddState(new MonsterKnockBack());
            ai.AddState(new HumanStunned());

            this.regEvt = new List<MyEvent.EventType>()
            {
                MyEvent.EventType.EnterSafePoint,
                MyEvent.EventType.ExitSafePoint,
            };
            RegEvent();

        }


        List<Vector3> samplePos;
        IEnumerator CheckFall()
        {
            Vector3 originPos = attribute.OriginPos;
            samplePos = new List<Vector3>(){ originPos };
            while (true)
            {
                var lastOne = transform.position;
                if(samplePos.Count > 0) {
                    lastOne = samplePos [0];
                }
                Log.Sys("lastPos nowPos " + lastOne + " now " + transform.position);
                if (transform.position.y < (lastOne.y - 3))
                {
                    if (!inSafe)
                    {
                        /*
                        //检查所有相邻的高度 < 3 则表示没有坠落 若存在相邻s > 3 则跳回
                        for (int i = 1; i < samplePos.Count; i++)
                        {
                            var dy = samplePos [i].y - samplePos [i - 1].y;
                            if (Mathf.Abs(dy) > 1f)
                            {
                                break;
                            }
                        }
                        */

                        transform.position = lastOne;    
                    }
                } else
                {
                    if (inSafe)
                    {
                        samplePos.Clear();
                        //samplePos.Add(transform.position);
                    } else
                    {
                        var pos = transform.position;
                        samplePos.Add(pos);
                        if (samplePos.Count > 4)
                        {
                            samplePos.RemoveAt(0);
                        }
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }

        private bool inSafe = false;

        protected override void OnEvent(MyEvent evt)
        {
            if (evt.type == MyEvent.EventType.EnterSafePoint)
            {
                inSafe = true;
                samplePos.Clear();
            } else if (evt.type == MyEvent.EventType.ExitSafePoint)
            {
                inSafe = false;
                samplePos.Add(transform.position);
            }
            Log.Sys("InSafeNow "+inSafe+" evt "+evt);
        }

        void Start()
        {
            ai.ChangeState(AIStateEnum.IDLE);
            StartCoroutine(CheckFall());
        }

    }



}
