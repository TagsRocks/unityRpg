using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class HideBloodBar : MonoBehaviour
    {
        List<GameObject> hideObject = new List<GameObject>();
        void OnDestroy() {
            RestoreAll();
        }
        public void RestoreAll() {
            var arr = hideObject.ToArray();
            foreach(var o in arr) 
            {
                if(o != null) {
                    Show(o);
                }
            }
            hideObject.Clear();
        }

        void Awake()
        {
            var sp = gameObject.AddComponent<SphereCollider>();
            sp.isTrigger = true;
            sp.center = new Vector3(0, 2, 0);
            sp.radius = 2.5f;
        }

        void Hide(GameObject go) {
            var attr= go.GetComponent<NpcAttribute>();
            var tc = attr.TeamColor;
            var myTc = ObjectManager.objectManager.GetMyAttr().TeamColor;
            if(myTc != tc) {
                attr.ShowBloodBar = false;
                hideObject.Add(go);
            }
        }

        void Show(GameObject go) {
            //Log.Sys("ShowBloodbar: "+go);
            var attr= go.GetComponent<NpcAttribute>();
            var tc = attr.TeamColor;
            var myTc = ObjectManager.objectManager.GetMyAttr().TeamColor;
            if(myTc != tc) {
                attr.ShowBloodBar = true;
                hideObject.Remove(go);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == GameTag.Player)
            {
                var attr = other.gameObject.GetComponent<NpcAttribute>();
                if(attr != null) {
                    Hide(attr.gameObject);
                }
            }
        }
        void OnTriggerExit(Collider other) {
            if(other.tag == GameTag.Player) {
                var attr = other.gameObject.GetComponent<NpcAttribute>();
                if(attr != null) {
                    
                    Show(attr.gameObject);
                }
            }
        }
    }

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
            var bd = new BlockDead();
            ai.AddState(new ChestIdle());
            ai.AddState(bd);
            ai.AddState(new MonsterKnockBack());

            Util.SetLayer(gameObject, GameLayer.IgnoreCollision2);

            var g = new GameObject("HideBloodBar");
            g.transform.parent = transform;
            Util.InitGameObject(g);
            var hb = g.AddComponent<HideBloodBar>();
            bd.deadCallback = hb.RestoreAll;
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