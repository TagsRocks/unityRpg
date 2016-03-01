using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class ChestDead2 : DeadState
    {
        public override void EnterState ()
        {
            base.EnterState ();
            GetAttr().animation.CrossFade ("opening");
            GetAttr().IsDead = true;
            GetAttr().OnlyShowDeadEffect();

            DropGoods.DropStaticGoods(GetAttr());
            CreateParticle();
        }

        void CreateParticle() 
        {
            var deathBlood = GameObject.Instantiate(Resources.Load<GameObject> ("particles/deathblood")) as GameObject;
            deathBlood.transform.parent = ObjectManager.objectManager.transform;
            deathBlood.transform.localPosition = GetAttr ().transform.localPosition+Vector3.up*0.1f;
            deathBlood.transform.localRotation = Quaternion.identity;
            deathBlood.transform.localScale = Vector3.one;
            NGUITools.AddMissingComponent<RemoveSelf> (deathBlood);
        }
        public override IEnumerator RunLogic()
        {
            yield return new WaitForSeconds (2);
            yield return GetAttr().StartCoroutine (Util.SetBurn (GetAttr().gameObject));
            yield return null;

            ObjectManager.objectManager.DestroyByLocalId (GetAttr().GetComponent<KBEngine.KBNetworkView>().GetLocalId());
        }
    }

    [RequireComponent(typeof(MonsterSync))]
    [RequireComponent(typeof(MonsterSyncToServer))]
    public class ChestAI2 : AIBase
    {
        void Awake()
        {
            attribute = GetComponent<NpcAttribute>();
            ai = new ChestCharacter();
            ai.attribute = attribute;
            ai.AddState(new ChestIdle());
            ai.AddState(new ChestDead2());
        }

        void Start()
        {
            ai.ChangeState (AIStateEnum.IDLE);
            var par = Instantiate(Resources.Load<GameObject>("particles/drops/generic_item")) as GameObject;
            par.transform.parent = transform;
            par.transform.localPosition = Vector3.zero;
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            if (attribute.IsDead) {
                Util.ClearMaterial (gameObject);
            }
        }
    }
}
