using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class SpawnChest : MonoBehaviour
    {
        public int rateToSpawn = 100;
        public int ChestId = 36;

        bool isSpawnYet = false;
        public int waveNum = 0;

        void Awake() {
            foreach(Transform t in transform){
                t.gameObject.SetActive(false);
            }
        }
        void Start()
        {
            StartCoroutine(CheckToSpawn());
        }

        bool CheckOk()
        {
            if (BattleManager.battleManager == null)
            {
                return false;
            }
            if (WorldManager.worldManager.station != WorldManager.WorldStation.Enter)
            {
                return false;
            }

            if(BattleManager.battleManager.waveNum == waveNum) {
                return true;
            }
            return false;
        }

        void DoSpawn() {
            var unitData = Util.GetUnitData(false, ChestId , 0);
            ObjectManager.objectManager.CreateChest(unitData, this);
        }


        IEnumerator CheckToSpawn()
        {
            while (true)
            {
                if(CheckOk()) {
                    break;
                }
                yield return new WaitForSeconds(1);
            }
            isSpawnYet = true;
            var rate = Random.Range(0, 100);
            if(rate < rateToSpawn) {
                DoSpawn();
            }
        }

    }
}
