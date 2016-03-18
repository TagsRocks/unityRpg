using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class RainSystem : MonoBehaviour
    {
        //public GameObject drop;
        //public GameObject splash;
        // Use this for initialization
        void Start()
        {
            StartCoroutine(RainTracePlayer());
        }

        IEnumerator RainTracePlayer() {
            var drop = Resources.Load<GameObject>("particles/puddle_drop");
            var splash = Resources.Load<GameObject>("particles/puddle_splash");

            var drop2 = GameObject.Instantiate(drop) as GameObject;
            var splash2 = GameObject.Instantiate(splash) as GameObject;
            var player = ObjectManager.objectManager.GetMyPlayer();
            while(player == null) {
                player = ObjectManager.objectManager.GetMyPlayer();
                yield return null;
            }

            while(true) {
                drop2.transform.position = player.transform.position;
                splash2.transform.position = player.transform.position+new Vector3(0, 0.1f, 0);
                yield return null;
            }
        }
	
    }

}