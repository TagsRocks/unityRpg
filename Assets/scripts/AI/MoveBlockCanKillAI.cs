using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class MoveBlock : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            Log.Sys("MoveBlock Enter : " + other.gameObject);
            if (NetworkUtil.IsNetMaster())
            {
                if (other.tag == GameTag.Player)
                {
                    //击退技能
                    var pos = other.transform.position;
                    var otherGo = other.gameObject;
                    //dy dx 比较 那个大 保留那个 同时另外一个修正为 自己的pos
                    var par = transform.parent.gameObject;
                    var myPos = par.transform.position;

                    //假设箱子都是 正方体
                    var dx = myPos.x - pos.x;
                    var dz = myPos.z - pos.z;
                    if (Mathf.Abs(dx) < Mathf.Abs(dz))
                    {
                        pos.x = myPos.x;
                    } else
                    {
                        pos.z = myPos.z;
                    }

                    var skill = Util.GetSkillData((int)SkillData.SkillConstId.KnockBack, 1);
                    var skillInfo = SkillLogic.GetSkillInfo(skill);
                    var evt = skillInfo.eventList [0];
                    var ret = par.GetComponent<BuffComponent>().AddBuff(evt.affix, pos);
                    if (ret)
                    {
                        NetDateInterface.FastAddBuff(evt.affix, otherGo, par, skill.Id, evt.EvtId, pos);
                    }
                }
            }
        }
    }

    [RequireComponent(typeof(MonsterSync))]
    [RequireComponent(typeof(MonsterSyncToServer))]
    public class MoveBlockCanKillAI : AIBase
    {
        void Awake()
        {
            attribute = GetComponent<NpcAttribute>();
            attribute.ShowBloodBar = false;

            ai = new ChestCharacter();
            ai.attribute = attribute;
            ai.AddState(new ChestIdle());
            var bd = new BlockDead();
            bd.deadCallback = () =>
            {
                Util.SpawnParticle("barrelbreak", transform.position, true);
            };

            ai.AddState(bd);
            ai.AddState(new MonsterKnockBack());

            var cb = transform.Find("Cube");
            cb.gameObject.AddComponent<MoveBlock>();
        }

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