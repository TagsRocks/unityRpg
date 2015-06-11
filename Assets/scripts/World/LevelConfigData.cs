using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//SceneId ---> ConfigData List
namespace ChuMeng {
    public class LevelConfigData  {
        static bool initYet = false;
        public static Dictionary<int, List<LevelConfig>> LevelLayout = new Dictionary<int, List<LevelConfig>>();
        public static void Init(){
            if(initYet) {
                return;
            }
            initYet = true;
            var l1 =  new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S", -1, 3),
                new LevelConfig("NS", -1, 2),
                    new LevelConfig("NS", -1, 1){useOtherZone=true, zoneId=1},
                new LevelConfig("NE", -1, 0),
                new LevelConfig("EW", 0, 0),
                new LevelConfig("NW", 1, 0),
                    new LevelConfig("NS", 1, 1){useOtherZone=true, zoneId=1},
                new LevelConfig("EXIT_S", 1, 2),
            };
            LevelLayout.Add(3, l1);

            //ZoneConfig
            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S", 0, 0){useOtherZone=true, zoneId=10},
            };
            LevelLayout.Add(2, l1);

        }
    }
}