using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//SceneId ---> ConfigData List
namespace ChuMeng
{
    public class EnvConfig {
        public string waterBottom;
        public string waterFace;
        public float offY = 8.5f;
    }
    public class LevelConfigData
    {
        static bool initYet = false;
        public static Dictionary<int, List<LevelConfig>> LevelLayout = new Dictionary<int, List<LevelConfig>>();
        public static Dictionary<string, EnvConfig> envConfig = new Dictionary<string, EnvConfig>();

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

            /*
            //ZoneConfig MainCity
            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_LM", 0, 0){useOtherZone=true, zoneId=10},
            };
            LevelLayout.Add(2, l1);
            */

            l1 = new List<LevelConfig>(){
                new LevelConfig("TOWN", 0, 0){useOtherZone=true, zoneId=70, type="town"},
            };
            LevelLayout.Add(2, l1);

            //Level 2
            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_PB", 0, 0){useOtherZone=true, zoneId=11},
                new LevelConfig("NW_PB", 0, -1){useOtherZone=true, zoneId=12},
                new LevelConfig("NE_PB", -1, -1){useOtherZone=true, zoneId=14},
                new LevelConfig("EXIT_S_PB", -1, 0){useOtherZone=true, zoneId=16},

            };
            LevelLayout.Add(102, l1);

            //Level 3
            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_E_KG", 0, 0){useOtherZone=true, zoneId=17},
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



            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_E_LM", 0, 0){useOtherZone=true, zoneId=50, type="suntemple"},
                new LevelConfig("EW_LM", 1, 0){useOtherZone=true, zoneId=51, type="suntemple"},
                new LevelConfig("EXIT_W_LM", 2, 0){useOtherZone=true, zoneId=52, type="suntemple"},
                //new LevelConfig("EW_PB", -1, 0){useOtherZone=true, zoneId=43},
                //new LevelConfig("EXIT_E_KG", -2, 0){useOtherZone=true, zoneId=44},
            };
            LevelLayout.Add(201, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_S_LM", 0, 0){useOtherZone=true, zoneId=53, type="suntemple"},
                new LevelConfig("NS_LM", 0, -1){useOtherZone=true, zoneId=54, type="suntemple"},
                new LevelConfig("EXIT_N_LM", 0, -2){useOtherZone=true, zoneId=55, type="suntemple"},
            };
            LevelLayout.Add(202, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_W_LM", 0, 0){useOtherZone=true, zoneId=56, type="suntemple"},
                new LevelConfig("SE_LM", -1, 0){useOtherZone=true, zoneId=57, type="suntemple"},
                new LevelConfig("EXIT_N_LM", -1, -1){useOtherZone=true, zoneId=58, type="suntemple"},
            };
            LevelLayout.Add(203, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_E_LM", 0, 0){useOtherZone=true, zoneId=59, type="suntemple"},
                new LevelConfig("SW_LM", 1, 0){useOtherZone=true, zoneId=60, type="suntemple"},
                new LevelConfig("EXIT_N_LM", 1, -1){useOtherZone=true, zoneId=61, type="suntemple"},
            };
            LevelLayout.Add(204, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_N_LM", 0, 0){useOtherZone=true, zoneId=62, type="suntemple"},
                new LevelConfig("NS_LM", 0, 1){useOtherZone=true, zoneId=63, type="suntemple"},
                new LevelConfig("EXIT_S_LM", 0, 2){useOtherZone=true, zoneId=64, type="suntemple"},
            };
            LevelLayout.Add(205, l1);



            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_N_LM", 0, 0){useOtherZone=true, zoneId=65, type="suntemple"},
            };
            LevelLayout.Add(3, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_N_PB", 0, 0){useOtherZone=true, zoneId=69, type="suntemple"},
            };
            LevelLayout.Add(4, l1);

            l1 = new List<LevelConfig>(){
                new LevelConfig("ENTRANCE_W_LM", 0, 0){useOtherZone=true, zoneId=66, type="suntemple"},
                new LevelConfig("EW_LM", -1, 0){useOtherZone=true, zoneId=67, type="suntemple"},
                new LevelConfig("EXIT_E_LM", -2, 0){useOtherZone=true, zoneId=68, type="suntemple"},
            };
            LevelLayout.Add(206, l1);



            envConfig.Add("suntemple", new EnvConfig(){
                waterBottom = "skyboxes/stemple_lake_light",
                waterFace = "skyboxes/stemple_water",
            });


        }
    }
}