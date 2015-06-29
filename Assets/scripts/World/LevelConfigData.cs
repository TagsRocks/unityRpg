using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//SceneId ---> ConfigData List
namespace ChuMeng
{
    public class LevelConfigData
    {
        static bool initYet = false;
        public static Dictionary<int, List<LevelConfig>> LevelLayout = new Dictionary<int, List<LevelConfig>>();

        public static void Init()
        {
            if (initYet)
            {
                return;
            }
            initYet = true;
            var l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_LM", -1, 3),
                new LevelConfig("NS_LM", -1, 2),
                    new LevelConfig("NS_LM", -1, 1){useOtherZone=true, zoneId=1},
                new LevelConfig("NE_LM", -1, 0),
                new LevelConfig("EW_LM", 0, 0),
                new LevelConfig("NW_LM", 1, 0),
                    new LevelConfig("NS_LM", 1, 1){useOtherZone=true, zoneId=1},
                new LevelConfig("EXIT_S_LM", 1, 2),
            };
            LevelLayout.Add(101, l1);

            //ZoneConfig
            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_LM", 0, 0){useOtherZone=true, zoneId=10},
            };
            LevelLayout.Add(2, l1);


            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_PB", 1, 1){useOtherZone=true, zoneId=11},

                new LevelConfig("NW_PB", 1, 0){useOtherZone=true, zoneId=12},
                new LevelConfig("EW_PB", 0, 0){useOtherZone=true, zoneId=13},
                new LevelConfig("NE_PB", -1, 0){useOtherZone=true, zoneId=14},
                new LevelConfig("NS_PB", -1, 1){useOtherZone=true, zoneId=15},
                new LevelConfig("EXIT_S_PB", -1, 2){useOtherZone=true, zoneId=16},

            };
            LevelLayout.Add(102, l1);
        }
    }
}