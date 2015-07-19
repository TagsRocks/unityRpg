using UnityEngine;
using System.Collections;
using UnityEditor;
using SimpleJSON;
using System.IO;
using System.Collections.Generic;
using ChuMeng;



[CustomEditor(typeof(MakeScene))]
public class MakeSceneEditor : Editor
{
    //string dir = "";
    string layoutStr = "";
    //string modelStr = "";
    //string lightStr = "";

    GameObject CreateAProp(GameObject root, JSONClass jobj)
    {
        
        Debug.Log("file is " + jobj.ToString());  
        var px = jobj ["POSITIONX"].AsFloat;
        var py = jobj ["POSITIONY"].AsFloat;
        var pz = jobj ["POSITIONZ"].AsFloat;
        var file = jobj ["LAYOUT FILE"].Value;

        var rx = jobj ["RIGHTX"].AsFloat;
        var ry = jobj ["RIGHTY"].AsFloat;
        var rz = jobj ["RIGHTZ"].AsFloat;

        var fx = jobj ["FORWARDX"].AsFloat;
        var fy = jobj ["FORWARDY"].AsFloat;
        var fz = jobj ["FORWARDZ"].AsFloat;


        var levelPrefab = Path.Combine(Application.dataPath, "levelPrefab");
        var prefabList = new DirectoryInfo(levelPrefab);
        FileInfo[] fileInfo = prefabList.GetFiles("*.prefab", SearchOption.AllDirectories);

        var fname = Path.GetFileName(file);
        Debug.Log("create Prefab " + fname);
        int lastNameLen = 0;
        FileInfo bestMatch = null;
        foreach (var p in fileInfo)
        {
            Debug.Log("Check Prefab " + p.Name);
            var pn = p.Name.Replace(".prefab", "").ToLower();
            var fn = fname.ToLower();
            if (pn.Length > lastNameLen && fn.Contains(pn))
            {
                lastNameLen = pn.Length;
                bestMatch = p;


            }
        }
        if (bestMatch != null)
        {
            Debug.Log("Find File " + fname);
            var assPath = bestMatch.FullName.Replace(Application.dataPath, "Assets");
            var g = PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath<GameObject>(assPath)) as GameObject;
            g.transform.parent = root.transform;
            g.transform.position = new Vector3(-px, py, pz);
            g.transform.localScale = Vector3.one;

            //LevelPrefab No Need To Multiply -90 because LevelPrefab combine RoomPieces With Particles
            //OtherPrefab Need To Rotate Only Room Pieces
            //Adjust Rotate
            var rot = Quaternion.LookRotation(new Vector3(fx, fy, fz), Vector3.up);
            var rot2 = Quaternion.Euler(new Vector3(rot.eulerAngles.x, -rot.eulerAngles.y, rot.eulerAngles.z));
            g.transform.localRotation = rot2;//* Quaternion.Euler(new Vector3(-90, 0, 0));

            return g;
        }
        Debug.Log("Not Find " + fname);
        return null;
    }

    RoomData.RoomPosRot GetPrefabConfig(GameObject g)
    {
        var pb = new RoomData.RoomPosRot();
        pb.prefab = PrefabUtility.GetPrefabParent(g) as GameObject;
        pb.pos = g.transform.localPosition;
        pb.rot = g.transform.localRotation;
        return pb;
    }

    void HandlePropsNode(GameObject root, JSONClass jobj, GameObject saveData)
    {
        if (jobj ["DESCRIPTOR"].Value == "Layout Link")
        {
            var g = CreateAProp(root, jobj);
            if (g != null)
            {
                var rd = saveData.GetComponent<RoomData>();
                var pb = GetPrefabConfig(g);
                rd.Prefabs.Add(pb);
            }
        }
    }
    /// <summary>
    /// 将场景中的Layout Link 组件放置到场景里面 levelSets/xxx.layout 
    /// </summary>
    /// <param name="jarr">Jarr.</param>
    void MakeProps(JSONClass jobj)
    {
        var root = new GameObject("Props");
        var saveData = new GameObject("Props_data");
        saveData.AddComponent<RoomData>();

        Util.InitGameObject(root);

        int count = 0;
        int fail = 0;

        VoidDelegate hl = delegate(JSONClass node)
        {
            HandlePropsNode(root, node, saveData);
        };
        TranverseLightNode(jobj, hl);

    }
    public delegate void VoidDelegate(JSONClass obj);

    public delegate GameObject GameObjectDelegate(string name);

    void TranverseTree(JSONClass jobj, VoidDelegate handler)
    {
        if (jobj ["DESCRIPTOR"].Value == "Room Piece")
        {
            handler(jobj);
        }
        foreach (JSONNode n in jobj["children"].AsArray)
        {
            TranverseTree(n.AsObject, handler);
        }
    }

    /// <summary>
    /// 遍历Layout json 文件每个Tree node
    /// </summary>
    /// <param name="jobj">Jobj.</param>
    /// <param name="handler">Handler.</param>
    void TranverseLightNode(JSONClass jobj, VoidDelegate handler)
    {
        handler(jobj);
        foreach (JSONNode n in jobj["children"].AsArray)
        {
            TranverseLightNode(n.AsObject, handler);
        }
    }


    /// <summary>
    /// 增加灯光节点
    /// </summary>
    void HandleLightNode(GameObject root, JSONClass jobj, GameObject saveData)
    {
        Debug.Log("Descriptor " + jobj ["DESCRIPTOR"].Value);
        if (jobj ["DESCRIPTOR"].Value == "Light")
        {
            var n = jobj;
            var ln = Path.GetFileName(jobj ["FILE"].Value);
            Debug.Log("light file name " + ln);
            var pb = "Assets/lightPrefab/" + ln.Replace(".MESH", ".prefab");
            var lobj = Resources.LoadAssetAtPath(pb, typeof(GameObject)) as GameObject;
            var copyobj = PrefabUtility.InstantiatePrefab(lobj) as GameObject;
            copyobj.transform.parent = root.transform;
            copyobj.transform.localPosition = new Vector3(-n ["POSITIONX"].AsFloat, n ["POSITIONY"].AsFloat, n ["POSITIONZ"].AsFloat);
            copyobj.transform.localScale = new Vector3(n ["SCALE X"].AsFloat, n ["SCALE Z"].AsFloat, 1);
            var rot = Quaternion.Euler(new Vector3(0, n ["ANGLE"].AsFloat, 0));
            copyobj.transform.localRotation = rot * Quaternion.Euler(new Vector3(-90, 0, 0));


            var rd = saveData.GetComponent<RoomData>();
            var pb1 = new RoomData.RoomPosRot();
            pb1.prefab = lobj;
            pb1.pos = copyobj.transform.localPosition;
            pb1.rot = copyobj.transform.localRotation;
            pb1.scale = copyobj.transform.localScale;
            rd.Prefabs.Add(pb1);
        }

    }

    /// <summary>
    /// 根据LevelConfig 生成一个关卡
    /// </summary>
    void MakeLevel()
    {
        var r = GameObject.Find("root");
        if (r != null)
        {
            GameObject.DestroyImmediate(r);
        }
        
        var root = new GameObject("root");
        root.transform.localPosition = Vector3.zero;
        root.transform.localRotation = Quaternion.identity;
        root.transform.localScale = Vector3.one;
        
        
        var config = new List<LevelConfig>(){
            new LevelConfig("ENTRANCE_S", -1, 3),
            new LevelConfig("NS", -1, 2),
            new LevelConfig("NS", -1, 1),
            new LevelConfig("NE", -1, 0),
            new LevelConfig("EW", 0, 0),
            new LevelConfig("NW", 1, 0),
            new LevelConfig("NS", 1, 1),
            new LevelConfig("Exit_S", 1, 2),
        };
        
        //var load = Resources.LoadAssetAtPath("Assets/scenes/1X1_NS.unity", typeof(UnityEngine.SceneAsset));
        //Debug.Log("load scene is "+load);
        var roomPath = Path.Combine(Application.dataPath, "room");
        var resDir = new DirectoryInfo(roomPath);
        FileInfo[] fileInfo = resDir.GetFiles("*.prefab", SearchOption.AllDirectories); 
        List<GameObject> nameToGameObject = new List<GameObject>();
        
        foreach (FileInfo f in fileInfo)
        {
            Debug.Log("fileName " + f.FullName);
            Debug.Log("DataPath " + Application.dataPath);
            var pa = f.FullName.Replace(Application.dataPath, "Assets");
            
            var pre = Resources.LoadAssetAtPath<GameObject>(pa);
            
            nameToGameObject.Add(pre);
        }
        Debug.Log("prefab num " + nameToGameObject.Count);
        foreach (LevelConfig lc in config)
        {
            var namePart = lc.room.ToLower().Split(char.Parse("_"));
            bool gotPrefab = false;
            GameObject insPrefab = null;
            foreach (GameObject g in nameToGameObject)
            {
                var prefabElement = g.name.ToLower().Split(char.Parse("_"));
                bool find = true;
                foreach (string n in namePart)
                {
                    if (!checkIn(n, prefabElement))
                    {
                        find = false;
                        break;
                    }
                }
                if (find)
                {
                    insPrefab = g;
                    gotPrefab = true;
                    Debug.Log("find part " + lc.room + " " + g.name);
                    break;
                }
            }
            if (!gotPrefab)
            {
                Debug.Log("not find " + lc.room);
            } else
            {
                var newG = GameObject.Instantiate(insPrefab) as GameObject;
                newG.transform.parent = root.transform;
                newG.transform.localPosition = new Vector3(lc.x * 96, 0, lc.y * 96 + 48);
                newG.transform.localScale = Vector3.one;
                newG.transform.localRotation = Quaternion.identity;
            }
        }
    }
    
    void handleRoomPiece(GameObject root, JSONClass mapObj, JSONClass jobj, GameObjectDelegate getG, GameObject saveData)
    {
        var guid = jobj ["GUID"].Value;
        var fileInfo = mapObj [guid].AsObject;
        var fn = fileInfo ["FILE"].Value;
        if (fn == "")
        {
            Debug.Log("NotFindRoomPiece " + jobj ["NAME"] + " guid " + jobj ["GUID"]);
        } else
        {
            var realFn = Path.GetFileName(fn);
            var lowFn = realFn.ToLower().Replace(".mesh", "");
            var g = getG(lowFn);
            if (g != null)
            {
                var roomData = saveData.GetComponent<RoomData>();
                var pb = new RoomData.RoomPosRot();
                pb.prefab = PrefabUtility.GetPrefabParent(g) as GameObject;
                roomData.Prefabs.Add(pb);

                //var n = new JSONClass();
                //roomData.TempJsonObjects.Add(n);

                var px = jobj ["POSITIONX"].AsFloat;
                var py = jobj ["POSITIONY"].AsFloat;
                var pz = jobj ["POSITIONZ"].AsFloat;
                var fx = jobj ["FORWARDX"].AsFloat;
                var fy = jobj ["FORWARDY"].AsFloat;
                var fz = jobj ["FORWARDZ"].AsFloat;
                var oldRot = g.transform.localRotation.eulerAngles.y;
                g.transform.parent = root.transform;
                g.transform.localPosition = new Vector3(-px, py, pz);
                var localPos = g.transform.localPosition;
                /*
                n["localPosition"][0].AsFloat = localPos.x;
                n["localPosition"][1].AsFloat = localPos.y;
                n["localPosition"][2].AsFloat = localPos.z;
                */
                pb.pos = localPos;

                g.transform.localScale = Vector3.one;

                var rot = Quaternion.LookRotation(new Vector3(fx, fy, fz), Vector3.up);
                var rot2 = Quaternion.Euler(new Vector3(rot.eulerAngles.x, -rot.eulerAngles.y, rot.eulerAngles.z));
                g.transform.localRotation = rot2 * Quaternion.Euler(new Vector3(-90, 0, 0));
                var localRot = g.transform.localRotation;
                /*
                n["localRotation"][0].AsFloat = localRot.x;
                n["localRotation"][1].AsFloat = localRot.y;
                n["localRotation"][2].AsFloat = localRot.z;
                n["localRotation"][3].AsFloat = localRot.w;
                */
                pb.rot = localRot;
                /*
                if(oldRot != 0){

                }else {
                    g.transform.localRotation = rot2 ;//* Quaternion.Euler(new Vector3(-90, 0, 0));
                }
                */

            } else
            {
                Debug.Log("NotFindPrefab  " + realFn);
            }
        }
    }

    Dictionary<string, GameObject> nameCache = new Dictionary<string, GameObject>();

    GameObject GetPrefab(string name, List<FileInfo[]> dirs)
    {
        if (nameCache.ContainsKey(name))
        {
            return nameCache [name];
        }

        int lastNameLen = 0;
        FileInfo bestMatch = null;
        foreach (var fi in dirs)
        {
            foreach (var f in fi)
            {
                var pn = f.Name.Replace(".prefab", "").ToLower();
                if (pn.Length > lastNameLen && name.Contains(pn))
                {
                    lastNameLen = pn.Length;
                    bestMatch = f;
                }
            }
        }
        if (bestMatch != null)
        {
            var assPath = bestMatch.FullName.Replace(Application.dataPath, "Assets");
            var g = PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath<GameObject>(assPath)) as GameObject;
            return g;
        }
        return null;
    }
    void MakeLightOuter(string lightStr){
        var light = Resources.LoadAssetAtPath("Assets/Config/" + lightStr + ".json", typeof(TextAsset)) as TextAsset;
        var larr = JSON.Parse(light.text) as JSONClass;
        Debug.Log("ReadFile " + lightStr);
        MakeLight(larr);
    }
    /// <summary>
    /// 组合场景里面的灯光
    /// </summary>
    /// <param name="jobj">Jobj.</param>
    void MakeLight(JSONClass larr)
    {
        GameObject lightObj = GameObject.Find("light");
        if (lightObj != null)
        {
            GameObject.DestroyImmediate(lightObj);
        }
        lightObj = new GameObject("light");
        var saveData = new GameObject("light_data");
        saveData.AddComponent<RoomData>();

        VoidDelegate hl = delegate(JSONClass node)
        {
            HandleLightNode(lightObj, node, saveData);
        };
        TranverseLightNode(larr, hl);
    }

    /// <summary>
    /// 组合场景里面的Room Pieces
    /// </summary>
    /// <param name="jobj">Jobj.</param>
    void MakeRoomPieces(JSONClass jobj)
    {
        var mapJson = Resources.LoadAssetAtPath<TextAsset>("Assets/Config/map.json");
        var mapObj = JSON.Parse(mapJson.text).AsObject;
        var root = new GameObject("RoomPieces");
        Util.InitGameObject(root);
        int count = 0;

        var saveData = new GameObject("RoomPieces_data");
        saveData.AddComponent<RoomData>();

        var resPath = Path.Combine(Application.dataPath, "levelPrefab");
        var dir = new DirectoryInfo(resPath);

        //var levelPrefab = dir.GetFiles("*.prefab", SearchOption.TopDirectoryOnly);


        resPath = Path.Combine(Application.dataPath, "prefabs");
        dir = new DirectoryInfo(resPath);
        var prefabs = dir.GetFiles("*.prefab", SearchOption.TopDirectoryOnly);

        resPath = Path.Combine(Application.dataPath, "prefabs/props");
        dir = new DirectoryInfo(resPath);
        var propsPrefab = dir.GetFiles("*.prefab", SearchOption.TopDirectoryOnly);


        GameObjectDelegate gg = delegate (string name)
        {
            return GetPrefab(name, new List<FileInfo[]>(){ prefabs, propsPrefab});
        };
        VoidDelegate hd = delegate(JSONClass obj)
        {
            handleRoomPiece(root, mapObj, obj, gg, saveData);
            count++;
        };

        TranverseTree(jobj, hd);
        //saveData.GetComponent<RoomData>().SaveJson();

        Debug.Log("ReadRoomPiece " + count);
    }

    /// <summary>
    /// 将media/xxx.mesh 转化成Assets/xxx.fbx
    /// </summary>
    /// <returns>The path.</returns>
    /// <param name="f">F.</param>
    string ConvertPath(string f)
    {
        var fpath = Path.Combine("Assets", f.Replace("media/", ""));
        fpath = fpath.Replace(".mesh", ".fbx");
        return fpath;
    }

    /// <summary>
    /// 无动画模型处理
    /// </summary>
    /// <param name="ass">Ass.</param>
    void AdjustModelImport(string ass)
    {
        var import = ModelImporter.GetAtPath(ass) as ModelImporter;
        Debug.Log("import is " + import);
        import.globalScale = 1;
        import.importAnimation = false;
        import.animationType = ModelImporterAnimationType.None;
        AssetDatabase.WriteImportSettingsIfDirty(ass);
    }
    //With Animation
    GameObject CombineTwo(string f, string col)
    {
        var fpath = ConvertPath(f);
        var g = Resources.LoadAssetAtPath<GameObject>(fpath);
        if (g != null)
        {
            if (!g.name.Contains("@"))
            {
                //AdjustModelImport(fpath);
                Debug.Log("Combine " + f);
                Debug.Log("ColFile " + col);
                GameObject cg = null;
                if (col != "")
                {
                    var cp = ConvertPath(col);
                    cg = Resources.LoadAssetAtPath<GameObject>(cp);
                    if (cg != null)
                    {
                        //AdjustModelImport(cp);
                    }
                }

                var fn = Path.GetFileName(fpath);
                var prefab = fn.Replace(".fbx", ".prefab");
                var oldPrefab = Resources.LoadAssetAtPath<GameObject>(prefab);
                if (oldPrefab == null)
                {
                
                    var tar = Path.Combine("Assets/prefabs", prefab);
                    var tg = PrefabUtility.CreatePrefab(tar, g);
                    if (cg != null)
                    {
                        var meshCollider = tg.AddComponent<MeshCollider>();
                        meshCollider.sharedMesh = cg.GetComponent<MeshFilter>().sharedMesh;
                    }
                    
                    return tg;
                } else
                {
                    Debug.Log("old prefab exists " + prefab);
                }
            }


        }
        return null;
    }

    /// <summary>
    /// 融合了MineProp.dat Prop.dat Mine.dat 三个文件的map.json 组合所有的RoomPieces 模型
    /// </summary>
    void CombineFileAndCollisionToPrefab()
    {
        var mapJson = Resources.LoadAssetAtPath<TextAsset>("Assets/Config/map.json");
        var mapObj = JSON.Parse(mapJson.text).AsObject;
        AssetDatabase.StartAssetEditing();
        foreach (KeyValuePair<string, JSONNode> n in mapObj)
        {
            var f = n.Value ["FILE"].Value;
            var col = n.Value ["COLLISIONFILE"].Value;
            CombineTwo(f, col);

        }
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty modelStr = serializedObject.FindProperty("modelStr");
        SerializedProperty dataConfigStr = serializedObject.FindProperty("dataConfigStr");
        SerializedProperty lightStr = serializedObject.FindProperty("lightStr");
        MakeScene.makeScene = target as MakeScene;


        //According to Prop.dat.json  Combine levelsets/props Model
        dataConfigStr.stringValue = GUILayout.TextField(dataConfigStr.stringValue);
        if(GUILayout.Button("一键生成房间数据")){
            var md = Resources.LoadAssetAtPath("Assets/Config/" + dataConfigStr.stringValue + ".json", typeof(TextAsset)) as TextAsset;
            var jobj = JSON.Parse(md.text).AsObject;
            MakeRoomPieces(jobj);
            MakeLight(jobj);
            MakeProps(jobj);
            var roomName = dataConfigStr.stringValue.Replace(".layout", "");
            var g = new GameObject(roomName);
            Util.InitGameObject(g);
            var rp = GameObject.Find("RoomPieces_data");
            var lp = GameObject.Find("light_data");
            var pp = GameObject.Find("Props_data");
            rp.transform.parent = g.transform;
            lp.transform.parent = g.transform;
            pp.transform.parent = g.transform;

            var resName = "Assets/Resources/room/"+g.name+".prefab";
            PrefabUtility.CreatePrefab(resName, g);
            GameObject.DestroyImmediate(g);
            var res = Resources.LoadAssetAtPath<GameObject>(resName);
            var roomList = Resources.Load<GameObject>("RoomList");
            var rl = roomList.GetComponent<RoomList>();
            rl.AddRoom(res);
            EditorUtility.SetDirty(rl);
            AssetDatabase.Refresh();





            var pieces = GameObject.Find("RoomPieces");
            var props = GameObject.Find("Props");
            var light = GameObject.Find("light");
            var root = new GameObject(roomName+"_structure");
            Util.InitGameObject(root);
            pieces.transform.parent = root.transform;
            props.transform.parent = root.transform;
            light.transform.parent = root.transform;
            PrefabUtility.CreatePrefab("Assets/room/"+root.name+".prefab", root);
            /*
            GameObject.DestroyImmediate();
            GameObject.DestroyImmediate();
            GameObject.DestroyImmediate();
            */
        }
        if (GUILayout.Button("根据Layout.json结合map.json获取所有Room Pieces"))
        {
            var md = Resources.LoadAssetAtPath("Assets/Config/" + dataConfigStr.stringValue + ".json", typeof(TextAsset)) as TextAsset;
            var jobj = JSON.Parse(md.text).AsObject;
            MakeRoomPieces(jobj);
        }
        if (GUILayout.Button("将LayoutLink 的props从levelPrefab中组合"))
        {
            Debug.Log("ReadData is " + dataConfigStr.stringValue);
            var md = Resources.LoadAssetAtPath("Assets/Config/" + dataConfigStr.stringValue + ".json", typeof(TextAsset)) as TextAsset;
            var jobj = JSON.Parse(md.text).AsObject;
            MakeProps(jobj);

        }
        if (GUILayout.Button("组合文件和碰撞为一个Prefab"))
        {
            CombineFileAndCollisionToPrefab();
        }

        if (GUILayout.Button("组合模型Props"))
        {
            var md = Resources.LoadAssetAtPath("Assets/Config/" + dataConfigStr.stringValue + ".json", typeof(TextAsset)) as TextAsset;
            var jobj = JSON.Parse(md.text).AsObject;
            int count = 0;
            foreach (KeyValuePair<string, JSONNode> N in jobj)
            {
                count++;
                var content = N.Value.AsObject;
                var fileName = content ["filename"];
                var colName = content ["collisionfile"];
                Debug.Log("fileName:" + fileName + " " + colName.ToString());
                if (colName.ToString() != "\"null\"")
                {
                    string fn = null;
                    fn = Path.GetFileName(fileName);
                    
                    
                    var fbx = fn.Replace(".mesh", ".fbx");
                    var prefab = fn.Replace(".mesh", ".prefab");
                    var oldFile = Path.Combine("Assets/levelsets/props", fbx);
                    Debug.Log("oldFile:" + oldFile);
                    var g = Resources.LoadAssetAtPath<GameObject>(oldFile);
                    if (g == null)
                    {
                        Debug.Log("load fbx error:" + fbx);
                    } else
                    {
                        var tar = Path.Combine("Assets/prefabs/props", prefab);
                        Debug.Log("Prefab is " + tar + " " + fbx + " GameObje: " + g);
                        var tg = PrefabUtility.CreatePrefab(tar, g);
                        var meshCollider = tg.AddComponent<MeshCollider>();
                    
                        var colN = Path.GetFileName(colName);
                        colN = Path.Combine("Assets/levelsets/props", colN.Replace(".mesh", ".fbx"));
                        var gcol = Resources.LoadAssetAtPath<GameObject>(colN);
                        meshCollider.sharedMesh = gcol.GetComponent<MeshFilter>().sharedMesh;
                    }
                    //break;
                } else
                {
                    string fn = Path.GetFileName(fileName);
                    var fbx = fn.Replace(".mesh", ".fbx");
                    var prefab = fn.Replace(".mesh", ".prefab");
                    Debug.Log("Create Collision null:" + fn);
                    var oldFile = Path.Combine("Assets/levelsets/props", fbx);
                    var tar = Path.Combine("Assets/prefabs/props", prefab);
                    var g = Resources.LoadAssetAtPath<GameObject>(oldFile);
                    if (g == null)
                    {
                        Debug.Log("load fbx error:" + fbx);   
                    } else
                    {
                        var tg = PrefabUtility.CreatePrefab(tar, g);
                    }
                }
            }
            Debug.Log("Export Count:" + count);
        }

        //According to Mine.data.json  To COmbine levelsets/Mine  Model 
        if (GUILayout.Button("组合模型Mine"))
        {
            var md = Resources.LoadAssetAtPath("Assets/Config/Mine.dat.json", typeof(TextAsset)) as TextAsset;
            var jobj = JSON.Parse(md.text).AsObject;
            int count = 0;
            foreach (KeyValuePair<string, JSONNode> N in jobj)
            {
                count++;
                var content = N.Value.AsObject;
                var fileName = content ["filename"];
                var colName = content ["collisionfile"];
                Debug.Log("fileName:" + fileName + " " + colName.ToString());
                if (colName.ToString() != "\"null\"")
                {
                    string fn = null;
                    fn = Path.GetFileName(fileName);
                 

                    var fbx = fn.Replace(".mesh", ".fbx");
                    var prefab = fn.Replace(".mesh", ".prefab");
                    var oldFile = Path.Combine("Assets/levelsets/mine", fbx);
                    Debug.Log("oldFile:" + oldFile);
                    var g = Resources.LoadAssetAtPath<GameObject>(oldFile);
                    var tar = Path.Combine("Assets/prefabs", prefab);
                    Debug.Log("Prefab is " + tar + " " + fbx + " GameObje: " + g);
                    var tg = PrefabUtility.CreatePrefab(tar, g);
                    var meshCollider = tg.AddComponent<MeshCollider>();

                    var colN = Path.GetFileName(colName);
                    colN = Path.Combine("Assets/levelsets/mine", colN.Replace(".mesh", ".fbx"));
                    var gcol = Resources.LoadAssetAtPath<GameObject>(colN);
                    meshCollider.sharedMesh = gcol.GetComponent<MeshFilter>().sharedMesh;
                    //break;
                } else
                {
                    string fn = Path.GetFileName(fileName);
                    var fbx = fn.Replace(".mesh", ".fbx");
                    var prefab = fn.Replace(".mesh", ".prefab");
                    Debug.Log("Create Collision null:" + fn);
                    var oldFile = Path.Combine("Assets/levelsets/mine", fbx);
                    var tar = Path.Combine("Assets/prefabs", prefab);
                    var g = Resources.LoadAssetAtPath<GameObject>(oldFile);
                    //var tg = 
                        PrefabUtility.CreatePrefab(tar, g);
                    //tg.AddComponent<BoxCollider>();
                }
            }
            Debug.Log("Export Count:" + count);
        }

        layoutStr = GUILayout.TextField(layoutStr);
        //According to Mine Model and  layout file to combine A layouts/mine/xxx.layout Scene

        if (GUILayout.Button("组合场景"))
        {
            var md = Resources.LoadAssetAtPath("Assets/Config/Mine.dat.json", typeof(TextAsset)) as TextAsset;
            var jobj = JSON.Parse(md.text).AsObject;

            var layout = Resources.LoadAssetAtPath<TextAsset>("Assets/Config/" + layoutStr + ".json");
            var jlist = JSON.Parse(layout.text).AsArray;
            Debug.Log("ArrayLength " + jlist.Count);
            int notFindCount = 0;
            int allPieces = 0;
            var r = GameObject.Find("root");
            if (r != null)
            {
                GameObject.DestroyImmediate(r);
            }

            var root = new GameObject("root");
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            ///
            /// ogre unity 
            /// 坐标系差别
            /// x 轴方向相反
            /// ogre为 右手坐标系
            /// unity为左手坐标系
            /// 因此旋转direction不同
            ///
            foreach (JSONNode n in jlist)
            {
                var obj = n.AsObject;
                var pieces = obj ["pieces"].AsArray;
                Debug.Log("pieces :" + pieces.Count);
                allPieces = pieces.Count;

                Dictionary<string, int> pieceId = new Dictionary<string, int>();
                foreach (JSONNode p in pieces)
                {
                    var pobj = p.AsObject;
                    var gid = pobj ["guid"].Value;

                    var meshFile = jobj [gid];
                    if (meshFile == null)
                    {
                        //Debug.Log("Not Find gid:"+gid+" "+gid.GetType());
                        notFindCount++;
                    } else
                    {
                        var fileName = meshFile ["filename"];
                        float px = -pobj ["posx"].AsFloat;
                        float py = pobj ["posy"].AsFloat;
                        float pz = pobj ["posz"].AsFloat;

                        float fx = pobj ["forx"].AsFloat;
                        float fy = pobj ["fory"].AsFloat;
                        float fz = pobj ["forz"].AsFloat;

                        float rx = pobj ["rix"].AsFloat;
                        float ry = pobj ["riy"].AsFloat;
                        float rz = pobj ["riz"].AsFloat;

                        var fn = Path.GetFileName(fileName.Value);
                        var prefab = fn.Replace(".mesh", ".prefab");
                        var oldFile = Path.Combine("Assets/prefabs", prefab);
                        Debug.Log("instantiate :" + oldFile);
                        var g = GameObject.Instantiate(Resources.LoadAssetAtPath<GameObject>(oldFile)) as GameObject;
                        int co = 0;
                        pieceId.TryGetValue(gid, out co);

                        g.name = g.name + "_" + co;
                        pieceId [gid] = co + 1;
                        g.transform.parent = root.transform;
                        g.transform.localPosition = new Vector3(px, py, pz);
                        //g.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(fx, fy, fz))*Quaternion.Euler(new Vector3(-90, 0, 0));
                        //-90 0 0 
                        var rot = Quaternion.LookRotation(new Vector3(fx, fy, fz), Vector3.up);
                        var rot2 = Quaternion.Euler(new Vector3(rot.eulerAngles.x, -rot.eulerAngles.y, rot.eulerAngles.z));
                        g.transform.localRotation = rot2 * Quaternion.Euler(new Vector3(-90, 0, 0));// g.transform.localRotation;
                        g.transform.localScale = Vector3.one;
                    }
                }
            }
            Debug.Log("notFind:" + notFindCount);
            Debug.Log("allPieces:" + allPieces);
        }


        ///According to Layout.json to Test whether all Room Pieces is in Mine.ata.json
        if (GUILayout.Button("Test"))
        {
            var md = Resources.LoadAssetAtPath("Assets/Config/Mine.dat.json", typeof(TextAsset)) as TextAsset;
            var jobj = JSON.Parse(md.text).AsObject;
            var layout = Resources.LoadAssetAtPath<TextAsset>("Assets/Config/1X1ENTRANCE_E.json");
            var jlist = JSON.Parse(layout.text).AsArray;
            Debug.Log("ArrayLength " + jlist.Count);

            int notFindCount = 0;
            int allPieces = 0;

            foreach (JSONNode n in jlist)
            {
                var obj = n.AsObject;
                var pieces = obj ["pieces"].AsArray;
                Debug.Log("pieces :" + pieces.Count);
                allPieces = pieces.Count;
                
                Dictionary<string, int> pieceId = new Dictionary<string, int>();
                foreach (JSONNode p in pieces)
                {
                    var pobj = p.AsObject;
                    var gid = pobj ["guid"].Value;
                    
                    var meshFile = jobj [gid];
                    if (meshFile == null)
                    {
                        Debug.Log("Not Find gid:" + gid);
                        notFindCount++;
                    } else
                    {

                    }
                }
            }
            Debug.Log("notFind:" + notFindCount);
            Debug.Log("allPieces:" + allPieces);
        }

        //Export Root GameObject to Assets/room/xxxx.unity prefab file
        if (GUILayout.Button("导出Scene中的root为room Prefab"))
        {
            var root = GameObject.Find("root");
            if (root != null)
            {
                var path = EditorApplication.currentScene.Split(char.Parse("/"));
                var sceneName = path [path.Length - 1];
                PrefabUtility.CreatePrefab(Path.Combine("Assets/room", sceneName.Replace(".unity", ".prefab")), root);

            }
        }

        //According to LevelCOnfig to Construct a Big Level
        if (GUILayout.Button("根据配置文件构建关卡"))
        {
            MakeLevel();
        }


        //No Use
        modelStr.stringValue = GUILayout.TextField(modelStr.stringValue);
        if (GUILayout.Button("组合模型和粒子效果为一个场景组件"))
        {
            var config = Path.Combine("Assets/Config", modelStr.stringValue + ".json");
            var txt = Resources.LoadAssetAtPath<TextAsset>(config);
            var js = JSON.Parse(txt.text);
            Debug.Log(js);
            var arr = js ["children"] [0] ["children"].AsArray;
            var objects = GetAllObject(modelStr.stringValue, arr);

        }

        //Import Animation Model From Directory each SubDirectory  Combine all Fbx in a subdirectory
        if (GUILayout.Button("导入带动画模型"))
        {
            var allModel = Path.Combine(Application.dataPath, modelStr.stringValue);
            var resDir = new DirectoryInfo(allModel);
            DirectoryInfo[] fileInfo = resDir.GetDirectories("*", SearchOption.TopDirectoryOnly);//("*.*", SearchOption.TopDirectoryOnly);
            foreach (DirectoryInfo file in fileInfo)
            {
                Debug.Log("Directory name " + file.FullName);
   
                var allFiles = file.GetFiles("*.fbx", SearchOption.TopDirectoryOnly);
                CreateAniModelPrefab(allFiles, file.Name);

            }
        }


        //Import all Model in   modelStr Directory
        if (GUILayout.Button("调整某个目录下所有的无动画模型"))
        {
            AdjustNoAniModel(modelStr.stringValue);

        }


        //调整modelStr 目录下所有的模型的shader为默认LightMapEnvshader
        if (GUILayout.Button("设置shader 为Custom/light"))
        {
            Debug.Log(Application.dataPath);
            
            var allModel = Path.Combine(Application.dataPath, modelStr.stringValue);
            var resDir = new DirectoryInfo(allModel);
            FileInfo[] fileInfo = resDir.GetFiles("*.fbx", SearchOption.AllDirectories);
            AssetDatabase.StartAssetEditing();
            foreach (FileInfo file in fileInfo)
            {
                Debug.Log("file is " + file.Name + " " + file.Name);
                
                var ass = file.FullName.Replace(Application.dataPath, "Assets");
                var res = Resources.LoadAssetAtPath<GameObject>(ass);
                res.renderer.sharedMaterial.shader = Shader.Find("Custom/lightMapEnv");
                EditorUtility.SetDirty(res.renderer.sharedMaterial);

                Debug.Log("import change state ");
                AssetDatabase.WriteImportSettingsIfDirty(ass);
            }
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();

        }


        //设置modelStr 下面对象为LightLayer
        if (GUILayout.Button("调整prefab的layer属性 LightLayer"))
        {
            var allModel = Path.Combine(Application.dataPath, modelStr.stringValue);
            var resDir = new DirectoryInfo(allModel);
       
            FileInfo[] fileInfo = resDir.GetFiles("*.fbx", SearchOption.AllDirectories);
            //AssetDatabase.StartAssetEditing();
            foreach (FileInfo file in fileInfo)
            {
                var ass = Path.Combine("Assets/" + modelStr.stringValue, file.Name);
                var res = Resources.LoadAssetAtPath<GameObject>(ass);
                var tar = Path.Combine("Assets/lightPrefab", file.Name.Replace(".fbx", ".prefab"));
                var tg = PrefabUtility.CreatePrefab(tar, res);
                tg.layer = 8;


            }
            //AssetDatabase.StopAssetEditing();
            //AssetDatabase.Refresh();
        }


        //根据light.json 文件生成light GameObject 以及所有light的位置
        //lightStr = GUILayout.TextField(lightStr);
        lightStr.stringValue = GUILayout.TextField(lightStr.stringValue);

        if (GUILayout.Button("读取生成所有的light位置"))
        {
            MakeLightOuter(lightStr.stringValue);
        }

        if (GUILayout.Button("读取粒子配置文件生成一个粒子"))
        {
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 将模型的localScale = 1 animation为null
    /// </summary>
    /// <param name="rootPath">Root path.</param>
    void AdjustNoAniModel(string rootPath)
    {
        Debug.Log(Application.dataPath);
        
        var allModel = Path.Combine(Application.dataPath, rootPath);
        var resDir = new DirectoryInfo(allModel);
        FileInfo[] fileInfo = resDir.GetFiles("*.fbx", SearchOption.AllDirectories);
        AssetDatabase.StartAssetEditing();
        foreach (FileInfo file in fileInfo)
        {
            Debug.Log("file is " + file.Name + " " + file.Name);
            
            //var ass = Path.Combine("Assets/" + modelStr.stringValue, file.Name);
            var ass = file.FullName.Replace(Application.dataPath, "Assets");
            var import = ModelImporter.GetAtPath(ass) as ModelImporter;
            Debug.Log("import is " + import);
            import.globalScale = 1;
            import.importAnimation = false;
            import.animationType = ModelImporterAnimationType.None;
            
            Debug.Log("import change state " + import);
            AssetDatabase.WriteImportSettingsIfDirty(ass);

            //ChangeShader
            /*
            var res = Resources.LoadAssetAtPath<GameObject>(ass);
            res.renderer.sharedMaterial.shader = Shader.Find("Custom/lightMapEnv");
            EditorUtility.SetDirty(res.renderer.sharedMaterial);
            
            Debug.Log("import change state ");
            AssetDatabase.WriteImportSettingsIfDirty(ass);
            */
            
        }
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// An Animation Model in A Certain Directory
    /// namePattern = xxx@idle xxx@run xxx@what.fbx Collision xxx
    /// CombineTo:  
    /// 
    /// AllFiles: list
    ///    if @ exist AnimationFile Later handle
    /// NormalFIle: 
    ///    handleNow
    /// ModelPrefab
    /// </summary>
    /// <returns>The ani model prefab.</returns>
    /// <param name="allFiles">All files.</param>
    /// <param name="dirName">Dir name.</param>
    GameObject CreateAniModelPrefab(FileInfo[] allFiles, string dirName)
    {
        var tar = Path.Combine("Assets/ModelPrefab", dirName + ".prefab");
        //var tg = PrefabUtility.CreatePrefab(tar, g);
        Dictionary<string, string> aniFbx = new Dictionary<string, string>();
        AssetDatabase.StartAssetEditing();
        foreach (var f in allFiles)
        {
            Debug.Log("fbx file is " + f.FullName);
            var path = f.FullName.Replace(Application.dataPath, "Assets");
            var import = ModelImporter.GetAtPath(path) as ModelImporter;
            if (path.Contains("@"))
            {
                //AnimationFIle
                import.globalScale = 1;
                import.importAnimation = true;
                import.animationType = ModelImporterAnimationType.Legacy;
                var namePart = path.Split('@');
                var aniName = namePart [1].Replace(".fbx", "");
                aniFbx.Add(aniName, path);


            } else
            {
                //COllision File
                import.globalScale = 1;
                import.importAnimation = false;
                import.animationType = ModelImporterAnimationType.None;
                aniFbx.Add("collision", path);
            }
            AssetDatabase.WriteImportSettingsIfDirty(path);
        }
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();


        //Use First Animation FBX idle as base
        var prefab = PrefabUtility.CreatePrefab(tar, Resources.LoadAssetAtPath<GameObject>(aniFbx ["idle"]));
        prefab.transform.Find("Armature").localRotation = Quaternion.identity;
        prefab.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        var meshCollider = prefab.AddComponent<MeshCollider>();
        var colObj = Resources.LoadAssetAtPath<GameObject>(aniFbx ["collision"]);
        meshCollider.sharedMesh = colObj.GetComponent<MeshFilter>().sharedMesh;
        var aniPart = prefab.GetComponent<Animation>();
        foreach (var ani in aniFbx)
        {
            if (ani.Key != "idle" && ani.Key != "collision")
            {
                var aniObj = Resources.LoadAssetAtPath<GameObject>(ani.Value);
                var clip = aniObj.animation.clip;
                aniPart.AddClip(clip, clip.name);
            }
        }

        //AssetDatabase.StartAssetEditing();
        foreach (Transform t in prefab.transform)
        {
            if (t.renderer != null)
            {
                Debug.Log("render is " + t.name);
                t.renderer.sharedMaterial.shader = Shader.Find("Custom/lightMapEnv");
                EditorUtility.SetDirty(t.renderer.sharedMaterial);
            }
        }

        return prefab;
    }

    GameObject GetAllObject(string levelPrefabName, JSONArray objects, bool needCreatePrefab = true)
    {
        List<JSONNode> nameToParticle = new List<JSONNode>();
        JSONNode model = null;

        var g = new GameObject(levelPrefabName.Replace(".layout", ""));
        //var linkPrefab = g.AddComponent<LinkPrefab>();

        var tarName = levelPrefabName.Replace(".layout", ".prefab");
        var tarPath = Path.Combine("Assets/levelPrefab", tarName);

        Util.InitGameObject(g);

        Debug.Log(" all Children " + objects.Count);
        Debug.Log(objects.ToString());

        foreach (JSONNode n in objects)
        {
            var prop = n ["children"] [0].AsObject;
            var desc = prop ["DESCRIPTOR"].Value;

            if (desc == "Layout Link Particle")
            {
                nameToParticle.Add(prop);
                Debug.Log("particle " + prop ["LAYOUT FILE"]);
                var assetFile = prop ["LAYOUT FILE"].Value.Replace("MEDIA", "Assets").Replace(".LAYOUT", ".prefab");
                Debug.Log("assetFile " + assetFile);

                var par = Resources.LoadAssetAtPath<GameObject>(assetFile);
                var particle = GameObject.Instantiate(par) as GameObject;

                particle.transform.parent = g.transform;
                Util.InitGameObject(particle);
                var px = -prop ["POSITIONX"].AsFloat;
                var py = prop ["POSITIONY"].AsFloat;
                var pz = prop ["POSITIONZ"].AsFloat;
                particle.transform.localPosition = new Vector3(px, py, pz);

                //linkPrefab.linkPrefab.Add(new LinkPrefab.LinkPrefabProp(){child=par, });
            } else if (desc == "Unit Trigger" || desc == "Generic Model")
            {
                model = Path.GetFileName(prop ["FILE"].Value).Replace(".MESH", "").ToLower();

                var modelDir = Path.Combine(Application.dataPath, "ModelPrefab");
                var resDir = new DirectoryInfo(modelDir);
                FileInfo[] files = resDir.GetFiles("*.prefab", SearchOption.AllDirectories);

                GameObject prefab = null;
                
                Debug.Log("read model " + model);
                foreach (var f in files)
                {
                    if (f.Name.Contains(model))
                    {
                        prefab = Resources.LoadAssetAtPath<GameObject>(f.FullName.Replace(Application.dataPath, "Assets"));
                        break;
                    }
                }

                var px = -prop ["POSITIONX"].AsFloat;
                var py = prop ["POSITIONY"].AsFloat;
                var pz = prop ["POSITIONZ"].AsFloat;

                if (prefab != null)
                {
                    var child = GameObject.Instantiate(prefab) as GameObject;
                    child.transform.parent = g.transform;
                    //Util.InitGameObject(child);
                    child.transform.localPosition = new Vector3(px, py, pz);
                    child.transform.localScale = Vector3.one;
                }
            } else if (desc == "Group")
            {
                var createGroup = GetAllObject("group", n ["children"] [1] ["children"].AsArray, false);
                createGroup.transform.parent = g.transform;
                Util.InitGameObject(createGroup);
            } else if (desc == "Room Piece")
            {
            }
        }
        if (needCreatePrefab)
        {
            PrefabUtility.CreatePrefab(tarPath, g, ReplacePrefabOptions.ConnectToPrefab);
        }
        return g;
    }

    bool checkIn(string s, string[] group)
    {
        foreach (string s1 in group)
        {
            if (s == s1) 
                return true;
        }
        return false;
    }
}
