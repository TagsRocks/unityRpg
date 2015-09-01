﻿using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class ShanShuoAI : AIBase
    {
        MyAnimationEvent myAnimationEvent;
        float heading;
        Vector3 targetRotation;
        void Awake() {
            attribute = GetComponent<NpcAttribute>();
            myAnimationEvent = GetComponent<MyAnimationEvent>();
            heading = Random.Range(0, 360);
            transform.eulerAngles = new Vector3(0, heading, 0);

            ai = new MonsterCharacter ();
            ai.attribute = attribute;
            ai.AddState (new MonsterIdle ());
            ai.AddState (new MonsterCombat ());
            ai.AddState (new MonsterDead ());
            ai.AddState (new MonsterFlee ());
            ai.AddState (new MonsterKnockBack ());
            ai.AddState(new MonsterSkill());
        }
        void Start() {
            myAnimationEvent.RegFlee ();
            myAnimationEvent.RegKnockBack ();
            ai.ChangeState (AIStateEnum.IDLE);

            gameObject.AddComponent<MonsterCheckUseSkill>();
        }
        
        protected override void OnDestroy() {
            if (attribute.IsDead) {
                Util.ClearMaterial (gameObject);
            }
        }

    }
}
