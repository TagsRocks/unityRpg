using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class LevelConfig
    {
        public string room;
        public int x;
        public int y;
    
        public LevelConfig(string r, int x1, int y1)
        {
            room = r;
            x = x1;
            y = y1;
        }
    }

/// <summary>
/// 流式关卡加载器，分Room加载
/// </summary>
    public class StreamLoadLevel : MonoBehaviour
    {
        int currentRoomIndex = -1;
        static Dictionary<string, string> namePieces = null;
        List<int> loadedRoom = new List<int>();
        Dictionary<int, GameObject> loadedZone = new Dictionary<int, GameObject>();
        public static StreamLoadLevel Instance = null;

        void InitNamePieces()
        {
            if (namePieces == null)
            {
                namePieces = new Dictionary<string, string>();

                var elements = new HashSet<string>(){
                "ENTRANCE",
                "EXIT",
                "EW",
                "S",
                "NE",
                "NS",
                "NW",
            };
                var rooms = Resources.Load<GameObject>("RoomList");
                foreach (var r in rooms.GetComponent<RoomList>().roomPieces)
                {
                    var ele = r.newName.ToUpper().Split(char.Parse("_"));
                    var eleStr = "";
                    bool isFirst = true;
                    foreach (var e in ele)
                    {
                        if (elements.Contains(e))
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                                eleStr += e;
                            } else
                            {
                                eleStr += "_" + e;
                            }
                        }
                    }

                    namePieces [eleStr] = r.newName;
                }
            }
        }
        /// <summary>
        /// 加载第一个房间
        /// </summary>
        /// <returns>The first room.</returns>
        public IEnumerator LoadFirstRoom()
        {
            currentRoomIndex = 0;
            loadedRoom.Add(currentRoomIndex);

            var root = new GameObject("Root_0"); //FirstRoom
            Util.InitGameObject(root);

            InitNamePieces();
            var first = configLists [0];
            var firstOffset = new Vector3(first.x*96, 9, first.y*96+48);
            root.transform.localPosition = firstOffset;
            var playerStart = GameObject.Find("PlayerStart");
            playerStart.transform.localPosition = playerStart.transform.localPosition + firstOffset;

            var piece = namePieces [first.room];
            var roomConfig = Resources.Load<GameObject>("room/" + piece);
            yield return StartCoroutine(LoadRoom(roomConfig));
            yield return null;
            yield return StartCoroutine(LoadLight(roomConfig));
            yield return StartCoroutine(LoadProps(roomConfig));
            yield return StartCoroutine(LoadZone());
        }

        /// <summary>
        /// 关卡房间相关怪物配置
        /// level_1_4_0
        /// </summary>
        /// <returns>The zones.</returns>
        IEnumerator LoadZone(){
            var name = "zones/level_1_4_"+currentRoomIndex;
            var root = GameObject.Find("Root_"+currentRoomIndex);
            var zoneConfig = Resources.Load<GameObject>(name);
            if(zoneConfig == null){
                Log.Sys("LoadZoneConfig Error "+name);
                yield break;
            }
            var zone = GameObject.Instantiate(zoneConfig) as GameObject;
            zone.transform.parent = root.transform;
            Util.InitGameObject(zone);
            loadedZone[currentRoomIndex] = zone;
            BattleManager.battleManager.AddZone(zone);
            yield return null;
        }

        IEnumerator LoadRoom(GameObject roomConfig, bool slowly=false)
        {

            var pieces = roomConfig.transform.Find("RoomPieces_data");
            var rd = pieces.GetComponent<RoomData>();
            var root = GameObject.Find("Root_"+currentRoomIndex);

            var rootOfPieces = new GameObject("RoomPieces");
            rootOfPieces.transform.parent = root.transform;
            Util.InitGameObject(rootOfPieces);

            //Batch Rooom
            foreach(var p in  rd.Prefabs){
                var r = GameObject.Instantiate(p.prefab) as GameObject;
                r.transform.parent = rootOfPieces.transform;
                r.transform.localPosition = p.pos;
                r.transform.localRotation = p.rot;
                r.transform.localScale = p.scale;
                if(slowly) {
                    yield return null;
                }
            }
            Log.Sys("LoadRoomFinish "+roomConfig.newName+" num "+rd.Prefabs.Count);
            yield return null;


        }
        IEnumerator LoadLight(GameObject roomConfig, bool slowly=false){
            var light = roomConfig.transform.Find("light_data");
            var rd = light.GetComponent<RoomData>();
            var root = GameObject.Find("Root_"+currentRoomIndex);
            var rootOfLight = new GameObject("Light");
            rootOfLight.transform.parent = root.transform;
            Util.InitGameObject(rootOfLight);
            
            //Batch Rooom
            foreach(var p in  rd.Prefabs){
                var r = GameObject.Instantiate(p.prefab) as GameObject;
                r.transform.parent = rootOfLight.transform;
                r.transform.localPosition = p.pos;
                r.transform.localRotation = p.rot;
                r.transform.localScale = p.scale;
                if(slowly){
                    yield return null;
                }
            }
            Log.Sys("LoadLightFinish "+roomConfig.newName+" num "+rd.Prefabs.Count);
            yield return null;
        }

        IEnumerator LoadProps(GameObject roomConfig, bool slowly=false){
            var light = roomConfig.transform.Find("Props_data");
            var rd = light.GetComponent<RoomData>();
            var root = GameObject.Find("Root_"+currentRoomIndex);
            var rootOfLight = new GameObject("Props");
            rootOfLight.transform.parent = root.transform;
            Util.InitGameObject(rootOfLight);
            
            //Batch Rooom
            foreach(var p in  rd.Prefabs){
                var r = GameObject.Instantiate(p.prefab) as GameObject;
                r.transform.parent = rootOfLight.transform;
                r.transform.localPosition = p.pos;
                r.transform.localRotation = p.rot;
                r.transform.localScale = p.scale;
                if(slowly) {
                    yield return null;
                }
            }
            Log.Sys("LoadPropsFinish "+roomConfig.newName+" num "+rd.Prefabs.Count);
            yield return null;
        }

        List<int> loadRequest = new List<int>();
        bool inLoad = false;

        /// <summary>
        /// 加载某个房间的邻居 
        /// 进入某个房间正在加载，而又有了加载Neibor的请求
        /// </summary>
        /// <returns>The current room neibor.</returns>
        public IEnumerator LoadRoomNeibor()
        {
            var nextRoom = currentRoomIndex+1;
            if(loadedRoom.Contains(nextRoom) || configLists.Count <= nextRoom){
                yield break;
            }
            if(inLoad){
                loadRequest.Add(currentRoomIndex+1);
                yield break;
            }

            inLoad = true;
            InitNamePieces();

            currentRoomIndex++;
            loadedRoom.Add(currentRoomIndex);

            var root = new GameObject("Root_"+currentRoomIndex); //FirstRoom
            Util.InitGameObject(root);

            var first = configLists [currentRoomIndex];
            var firstOffset = new Vector3(first.x*96, 9, first.y*96+48);
            root.transform.localPosition = firstOffset;
            
            var piece = namePieces [first.room];
            var roomConfig = Resources.Load<GameObject>("room/" + piece);
            yield return StartCoroutine(LoadRoom(roomConfig, true));
            yield return null;
            yield return StartCoroutine(LoadLight(roomConfig, true));
            yield return StartCoroutine(LoadProps(roomConfig, true));
            yield return StartCoroutine(LoadZone());
            yield return null;

            inLoad = false;

            if(loadRequest.Count > 0){
                Log.Sys("Cache Load Request");
                loadRequest.RemoveAt(0);
                StartCoroutine(LoadRoomNeibor());
            }
        }
        public IEnumerator MoveInNewRoom(){
            yield return StartCoroutine(ReleaseOldRoom());
            yield return StartCoroutine(LoadRoomNeibor());
        }
        /// <summary>
        /// 切换房间之后，进入新的房间并且激活了这个房间的怪物则释放旧的房间，同时预先加载下一个房间
        /// </summary>
        /// <returns>The old room.</returns>
        IEnumerator ReleaseOldRoom(){
            if(currentRoomIndex >= 0 && currentRoomIndex < configLists.Count){
                var g = GameObject.Find("Root_"+currentRoomIndex);
                GameObject.Destroy(g);
            }
            yield return null;
        }



        /// <summary>
        /// 切换进入某个新的房间
        /// </summary>
        /// <returns>The room.</returns>
        /// <param name="index">Index.</param>
        public IEnumerator ChangeRoom(int index)
        {
            yield return null;
       
        }

        List<LevelConfig> configLists;

        void Awake()
        {
            Instance = this;
            configLists = new List<LevelConfig>(){
            new LevelConfig("ENTRANCE_S", -1, 3),
            new LevelConfig("NS", -1, 2),
            new LevelConfig("NS", -1, 1),
            new LevelConfig("NE", -1, 0),
            new LevelConfig("EW", 0, 0),
            new LevelConfig("NW", 1, 0),
            new LevelConfig("NS", 1, 1),
            new LevelConfig("Exit_S", 1, 2),
        };
        }
    
    }
}
