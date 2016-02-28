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

        void Start()
        {
            var player = ObjectManager.objectManager.GetMyPlayer();
            /*
            var c = gameObject.AddComponent<CharacterController>();
            c.center = new Vector3(0, 0.2f, 0);
            c.radius = 0.1f;
            c.height = 0.5f;
            Physics.IgnoreCollision(c, player.GetComponent<CharacterController>());
            */
            var c = gameObject.AddComponent<SphereCollider>();
            c.center = new Vector3(0, 1, 0);
            c.radius = 2;
            c.isTrigger = true;
        }

        void OnTriggerEnter(Collider other) 
        {
            if(!pickYet && other.tag == GameTag.Player) {
                StartCoroutine(PickItem());
            }
        }

        IEnumerator PickItem() {
            pickYet = true;

            BackgroundSound.Instance.PlayEffect("pickup");
            GameObject.Destroy (gameObject);
            GameInterface_Backpack.PickItem(itemData, num);
            yield break;
        }

        public static GameObject MakeDropItem(ItemData itemData, Vector3 pos, int num)
        {
            var g = Instantiate(Resources.Load<GameObject>(itemData.DropMesh)) as GameObject;
            var com = g.AddComponent<DropItemStatic>();
            com.itemData = itemData;
            g.transform.position = pos;

            var par = Instantiate(Resources.Load<GameObject>("particles/drops/generic_item")) as GameObject;
            par.transform.parent = g.transform;
            par.transform.localPosition = Vector3.zero;
            com.Particle = par;
            com.num = num;

            com.StartCoroutine(WaitSound("dropgem"));
            return g;
        }

        static IEnumerator WaitSound(string s)
        {
            yield return new WaitForSeconds(0.2f);
            BackgroundSound.Instance.PlayEffect(s);
        }
    }

}