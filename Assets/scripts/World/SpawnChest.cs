using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class SpawnChest : MonoBehaviour
    {
        public  static int MaxSpawnId = 0;
        public int SpawnId;
        public int rateToSpawn = 100;
        //宝箱模型ID
        public int ChestId = 36;

        bool isSpawnYet = false;
        public int waveNum = 0;
        //宝箱所召唤的Boss的ID
        public int MonsterID = 2011;

        public void InitSpawnId() {
            SpawnId = MaxSpawnId++;
        }

        void Awake() {
            foreach(Transform t in transform){
                t.gameObject.SetActive(false);
            }
        }
        void OnEnable() {
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
            if(isSpawnYet) {
                yield break;
            }
            var player = ObjectManager.objectManager.GetMyPlayer();
            while(player == null) {
                player = ObjectManager.objectManager.GetMyPlayer();
                yield return null;
            }

            var world = WorldManager.worldManager.GetActive();
            if(world.IsNet){
                var attr = ObjectManager.objectManager.GetMyAttr();
                if(!attr.IsMaster) {
                    yield break;
                }
            }

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
