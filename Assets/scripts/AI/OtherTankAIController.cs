using UnityEngine;
using System.Collections;

namespace MyLib
{

    [RequireComponent(typeof(AnimationController))]
    [RequireComponent(typeof(PlayerSync))]
    [RequireComponent(typeof(TankPhysicComponent))]
    public class OtherTankAIController : AIBase 
    {
        void Awake()
        {
            attribute = GetComponent<NpcAttribute>();

            ai = new TankCharacter();
            ai.attribute = attribute;
            ai.AddState(new TankIdle());
            ai.AddState(new TankMoveAndShoot());
            ai.AddState(new TankDead());
            ai.AddState(new MonsterKnockBack());
            ai.AddState(new HumanStunned());
        }

        void Start()
        {
            ai.ChangeState(AIStateEnum.IDLE);
        }
    }

}