using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class DropItemStatic : MonoBehaviour
    {
        private ItemData itemData;
        private GameObject Particle;
        private int num;
        private bool pickYet = false;

        void Awake() {
            gameObject.AddMissingComponent<KBEngine.KBNetworkView>();
        }
        void Start()
        {
            var player = ObjectManager.objectManager.GetMyPlayer();
            var c = gameObject.AddComponent<SphereCollider>();
            c.center = new Vector3(0, 1, 0);
            c.radius = 2;
            c.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!pickYet && other.tag == GameTag.Player)
            {
                StartCoroutine(PickItem());
            }
        }

        IEnumerator PickItem()
        {
            pickYet = true;

            BackgroundSound.Instance.PlayEffect("pickup");
            GameObject.Destroy(gameObject);
            GameInterface_Backpack.PickItem(itemData, num);
            yield break;
        }

        public static GameObject MakeDropItemFromNet(ItemData itemData, Vector3 pos, int num, EntityInfo info)
        {
            var g = Instantiate(Resources.Load<GameObject>(itemData.DropMesh)) as GameObject;
            var com = g.AddComponent<DropItemStatic>();
            com.itemData = itemData;
            g.transform.position = pos;

            var netView = g.GetComponent<KBEngine.KBNetworkView>();
            netView.SetID(new KBEngine.KBViewID(info.Id, ObjectManager.objectManager.myPlayer));
            netView.IsPlayer = false;
            ObjectManager.objectManager.AddObject(info.Id, netView);

            var par = Instantiate(Resources.Load<GameObject>("particles/drops/generic_item")) as GameObject;
            par.transform.parent = g.transform;
            par.transform.localPosition = Vector3.zero;
            com.Particle = par;
            com.num = num;

            com.StartCoroutine(WaitSound("dropgem"));
            return g;
        }

        public static void  MakeDropItem(ItemData itemData, Vector3 pos, int num)
        {
            if (NetworkUtil.IsNetMaster())
            {
                var cg = CGPlayerCmd.CreateBuilder();
                cg.Cmd = "AddEntity";
                var etyInfo = EntityInfo.CreateBuilder();
                etyInfo.ItemId = itemData.ObjectId;
                etyInfo.ItemNum = num;
                var po = NetworkUtil.ConvertPos(pos);
                etyInfo.X = po [0];
                etyInfo.Y = po [1];
                etyInfo.Z = po [2];
                etyInfo.EType = EntityType.DROP;
                cg.EntityInfo = etyInfo.Build();
                NetworkUtil.Broadcast(cg);
            }
        }

        static IEnumerator WaitSound(string s)
        {
            yield return new WaitForSeconds(0.2f);
            BackgroundSound.Instance.PlayEffect(s);
        }
    }

}