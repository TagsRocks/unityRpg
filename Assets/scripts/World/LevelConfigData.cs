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
            //Level 1
            var l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_LM", 0, 0){useOtherZone=true, zoneId=0},
                //new LevelConfig("NS_LM", -1, 2),
                //    new LevelConfig("NS_LM", -1, 1){useOtherZone=true, zoneId=1},
                new LevelConfig("NE_LM", 0, -1){useOtherZone=true, zoneId=3},
                //new LevelConfig("EW_LM", 0, 0),
                new LevelConfig("NW_LM", 1, -1){useOtherZone=true, zoneId=5},
                //    new LevelConfig("NS_LM", 1, 1){useOtherZone=true, zoneId=26},
                new LevelConfig("EXIT_S_LM", 1, 0){useOtherZone=true, zoneId=7},
            };
            LevelLayout.Add(101, l1);

            //ZoneConfig MainCity
            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_LM", 0, 0){useOtherZone=true, zoneId=10},
            };
            LevelLayout.Add(2, l1);

            //Level 2
            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_PB", 0, 0){useOtherZone=true, zoneId=11},

                new LevelConfig("NW_PB", 0, -1){useOtherZone=true, zoneId=12},
                //new LevelConfig("EW_PB", 0, 0){useOtherZone=true, zoneId=13},
                new LevelConfig("NE_PB", -1, -1){useOtherZone=true, zoneId=14},
                //new LevelConfig("NS_PB", -1, 1){useOtherZone=true, zoneId=15},
                new LevelConfig("EXIT_S_PB", -1, 0){useOtherZone=true, zoneId=16},

            };
            LevelLayout.Add(102, l1);
            //Level 3
            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_E_KG", 0, 0){useOtherZone=true, zoneId=17},

                //new LevelConfig("EW_LM", 1, 0){useOtherZone=true, zoneId=18},
                new LevelConfig("SW_LM", 1, 0){useOtherZone=true, zoneId=25},
                new LevelConfig("NE_LM", 1, -1){useOtherZone=true, zoneId=19},
                new LevelConfig("EXIT_W_LM", 2, -1){useOtherZone=true, zoneId=20},

            };
            LevelLayout.Add(103, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_W_LM", 0, 0){useOtherZone=true, zoneId=21},

                new LevelConfig("SE_LM", -1, 0){useOtherZone=true, zoneId=22},
                new LevelConfig("NW_PB", -1, -1){useOtherZone=true, zoneId=23},
                new LevelConfig("EXIT_E_KG", -2, -1){useOtherZone=true, zoneId=24, flip=false},

            };
            LevelLayout.Add(104, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_N_PB", 0, 0){useOtherZone=true, zoneId=27},
                new LevelConfig("SE_PB", 0, 1){useOtherZone=true, zoneId=28},
                new LevelConfig("EXIT_W_PB", 1, 1){useOtherZone=true, zoneId=29},
                /*
                new LevelConfig("EXIT_E_KG", -2, -1){useOtherZone=true, zoneId=30, flip=false},
                */
            };
            LevelLayout.Add(105, l1);


            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_E_PB", 0, 0){useOtherZone=true, zoneId=30},
                new LevelConfig("SW_PB", 1, 0){useOtherZone=true, zoneId=31},
                new LevelConfig("EXIT_N_LM", 1, -1){useOtherZone=true, zoneId=32},
            };
            LevelLayout.Add(106, l1);


            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_W_LM", 0, 0){useOtherZone=true, zoneId=33},
                new LevelConfig("SE_PB", -1, 0){useOtherZone=true, zoneId=34},
                new LevelConfig("EXIT_N_LM", -1, -1){useOtherZone=true, zoneId=35},
            };
            LevelLayout.Add(107, l1);


            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_LM", 0, 0){useOtherZone=true, zoneId=36},
                new LevelConfig("NS_PB", 0, -1){useOtherZone=true, zoneId=37},
                new LevelConfig("EXIT_N_LM", 0, -2){useOtherZone=true, zoneId=38},
            };
            LevelLayout.Add(108, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_N_PB", 0, 0){useOtherZone=true, zoneId=39},
                new LevelConfig("NS_LM", 0, 1){useOtherZone=true, zoneId=40},
                new LevelConfig("EXIT_S_PB", 0, 2){useOtherZone=true, zoneId=41},
            };
            LevelLayout.Add(109, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_W_LM", 0, 0){useOtherZone=true, zoneId=42},
                new LevelConfig("EW_PB", -1, 0){useOtherZone=true, zoneId=43},
                new LevelConfig("EXIT_E_KG", -2, 0){useOtherZone=true, zoneId=44},
            };
            LevelLayout.Add(110, l1);

        }
    }
}