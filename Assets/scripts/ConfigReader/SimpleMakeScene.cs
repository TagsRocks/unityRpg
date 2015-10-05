using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
#endif

public class SimpleMakeScene : MonoBehaviour
{
    public string path;
    [ButtonCallFunc()]public bool AdjustNoAni;

    /// <summary>
    /// 导入静态场景模型
    ///调整无动画模型 拼接 Mesh 和 collision 
    /// </summary>
    public void AdjustNoAniMethod()
    {
        #if UNITY_EDITOR
        AdjustNoAniModel(path);
        #endif
    }


    public string path1;
    [ButtonCallFunc()]public bool CombineToPrefab;

    /// <summary>
    /// 特定目录下到model 和 collsion合并 
    /// </summary>
    public void CombineToPrefabMethod()
    {
        #if UNITY_EDITOR
        CombineFileAndCollisionToPrefab(path1);
        #endif
    }

    #if UNITY_EDITOR
    public string path3;
    [ButtonCallFunc()]public bool ImportAniModel;

    /// <summary>
    /// 导入NPC 或者 动画场景模型 
    /// </summary>
    public void ImportAniModelMethod()
    {
        var allModel = Path.Combine(Application.dataPath, path3);
        Debug.Log("Import Animation Model " + allModel);
        var resDir = new DirectoryInfo(allModel);

        var allFiles = resDir.GetFiles("*.fbx", SearchOption.TopDirectoryOnly);
        CreateAniModelPrefab(allFiles, resDir.Name);


        /*
        DirectoryInfo[] fileInfo = resDir.GetDirectories("*", SearchOption.TopDirectoryOnly);//("*.*", SearchOption.TopDirectoryOnly);
        foreach (DirectoryInfo file in fileInfo)
        {
            Debug.Log("Directory name " + file.FullName);
   
            var allFiles = file.GetFiles("*.fbx", SearchOption.TopDirectoryOnly);
            CreateAniModelPrefab(allFiles, file.Name);

        }
        */
    }
    GameObject CreateAniModelPrefab(FileInfo[] allFiles, string dirName)
    {
        var tar = Path.Combine("Assets/ModelPrefab", dirName + ".prefab");
        Debug.Log("CreateAniModelPrefab " + tar);
        //var tg = PrefabUtility.CreatePrefab(tar, g);
        Dictionary<string, string> aniFbx = new Dictionary<string, string>();
        AssetDatabase.StartAssetEditing();
        bool npc = false;
        foreach (var f in allFiles)
        {
            Debug.Log("fbx file is " + f.FullName);
            if (f.FullName.Contains("npc"))
            {
                npc = true;
            }
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
        var first = aniFbx.First();
        //aniFbx ["idle"]
        var prefab = PrefabUtility.CreatePrefab(tar, Resources.LoadAssetAtPath<GameObject>(first.Value));

        if (!npc)
        {
            prefab.transform.Find("Armature").localRotation = Quaternion.identity;
            prefab.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        }
        if (aniFbx.ContainsKey("collision"))
        {
            var meshCollider = prefab.AddComponent<MeshCollider>();
            var colObj = Resources.LoadAssetAtPath<GameObject>(aniFbx ["collision"]);
            meshCollider.sharedMesh = colObj.GetComponent<MeshFilter>().sharedMesh;
        }

        var aniPart = prefab.GetComponent<Animation>();
        foreach (var ani in aniFbx)
        {
            if (ani.Key != first.Key && ani.Key != "collision")
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
                if (npc)
                {
                    t.renderer.sharedMaterial.shader = Shader.Find("Custom/npcShader");
                    t.renderer.sharedMaterial.color = Color.white;
                } else
                {
                    t.renderer.sharedMaterial.shader = Shader.Find("Custom/lightMapEnv");
                }
                EditorUtility.SetDirty(t.renderer.sharedMaterial);
            }
        }

        return prefab;
    }

    #endif
    void AdjustNoAniModel(string rootPath)
    {
        Debug.Log("AdjustNoAniModel " + rootPath);
        #if UNITY_EDITOR     

        var allModel = Path.Combine(Application.dataPath, rootPath);
        var resDir = new DirectoryInfo(allModel);
        FileInfo[] fileInfo = resDir.GetFiles("*.fbx", SearchOption.TopDirectoryOnly);
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

        }
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
        #endif
    }



    #if UNITY_EDITOR
    /// <summary>
    /// 融合了MineProp.dat Prop.dat Mine.dat 三个文件的map.json 组合所有的RoomPieces 模型
    /// </summary>
    void CombineFileAndCollisionToPrefab(string p)
    {
        var mapJson = Resources.LoadAssetAtPath<TextAsset>("Assets/Config/map.json");
        var mapObj = JSON.Parse(mapJson.text).AsObject;
        AssetDatabase.StartAssetEditing();
        Debug.Log("path is " + p);
        foreach (KeyValuePair<string, JSONNode> n in mapObj)
        {
            var f = n.Value ["FILE"].Value;
            var col = n.Value ["COLLISIONFILE"].Value;

            var fpath = ConvertPath(f);
            fpath = fpath.Replace("levelsets", "levelSets");

            Debug.Log("filePath is " + fpath);

            if (fpath.Contains(p))
            {
                CombineTwo(f, col);
                //break;
            }

        }
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }
    //With Animation
    GameObject CombineTwo(string f, string col)
    {
        Debug.Log("CombineTwo " + f + " col " + col);

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

    #endif
}
