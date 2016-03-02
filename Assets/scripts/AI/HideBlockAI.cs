using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    [RequireComponent(typeof(MonsterSync))]
    [RequireComponent(typeof(MonsterSyncToServer))]
    public class HideBlockAI : AIBase
    {
        void Awake()
        {
            attribute = GetComponent<NpcAttribute>();
            attribute.ShowBloodBar = false;
            ai = new ChestCharacter();
            ai.attribute = attribute;
            ai.AddState(new ChestIdle());
            ai.AddState(new BlockDead());
            ai.AddState(new MonsterKnockBack());

            Util.SetLayer(gameObject, GameLayer.IgnoreCollision2);
        }

        // Use this for initialization
        void Start()
        {
            ai.ChangeState(AIStateEnum.IDLE);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (attribute.IsDead)
            {
                Util.ClearMaterial(gameObject);
            }
        }
	
    }

}