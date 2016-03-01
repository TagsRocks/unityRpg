using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class ZoneEntityManager : MonoBehaviour
    {
        public GameObject properties{
            private set;
            get;
        }

        void Awake()
        {
            properties = Util.FindChildRecursive(transform, "properties").gameObject;
            allChest = gameObject.GetComponentsInChildren<SpawnChest>(true);
            SpawnChest.MaxSpawnId = 0;
            foreach(var s in allChest) {
                s.InitSpawnId();
            }
        }

        public void EnableProperties()
        {
            properties.gameObject.SetActive(true);
        }

        public void DisableProperties()
        {
            properties.SetActive(false);
        }

        private SpawnChest[] allChest;
        public SpawnChest GetSpawnChest(int spawnId) {
            foreach(var s in allChest) {
                if(s.SpawnId == spawnId) {
                    return s;
                }
            }
            return null;
        }

    }
}
