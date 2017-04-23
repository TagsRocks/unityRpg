using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class TestLoad : MonoBehaviour {
    public string what;

    [ButtonCallFunc()]
    public bool LoadData;
	public void LoadDataMethod()
    {
        Debug.LogError(Application.dataPath);
        var ch = System.IO.Path.DirectorySeparatorChar;
        var result = Application.dataPath.Replace('/', ch);
        Debug.LogError(result);

        //var load = AssetDatabase.LoadAssetAtPath<GameObject>(what);
        //Debug.LogError(load);
    }

    [ButtonCallFunc()]
    public bool TestFind;
    public void TestFindMethod()
    {
        var a = "assets/levelsets/z1tundra/z1tundra_snow/tundra_niche_basecap01.fbx";
        var b = "levelsets/z1tundra/z1tundar_snow";
        Debug.LogError(a.Contains(b));
    }

    [ButtonCallFunc()]
    public bool TPre;
    public void TPreMethod()
    {
        var resPath = Path.Combine(Application.dataPath, "prefabs");
        var dir = new DirectoryInfo(resPath);
        var propsPrefab = dir.GetFiles("*.prefab", SearchOption.AllDirectories);
        Debug.LogError("PrefabsNumber:" + propsPrefab.Length);
    }
}
