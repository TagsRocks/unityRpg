using UnityEngine;
using System.Collections;

namespace MyLib
{
    [RequireComponent(typeof(AnimationController))]
    [RequireComponent(typeof(SkillCombineBuff))]
    [RequireComponent(typeof(MySelfAttributeSync))]
    [RequireComponent(typeof(PlayerSyncToServer))]
    [RequireComponent(typeof(TankPhysicComponent))]
    public class TankAIController : AIBase
    {
        void Awake()
        {
            var tower = Util.FindChildRecursive(transform, "tower");
            tower.gameObject.AddComponent<TowerAutoCheck>();

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