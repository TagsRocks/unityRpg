using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class RoomList : MonoBehaviour {
    public List<GameObject> roomPieces = new List<GameObject>();
    [ButtonCallFunc()] public bool LoadAllRooms;
    public void LoadAllRoomsMethod(){
        roomPieces.Clear();

        var prefabList = new DirectoryInfo(Path.Combine(Application.dataPath, "Resources/room"));
        FileInfo[] fileInfo = prefabList.GetFiles("*.prefab", SearchOption.AllDirectories);
        foreach(var p in fileInfo){
            var n = p.FullName.Replace(Application.dataPath, "Assets");
            Debug.Log(n);
            var go = Resources.LoadAssetAtPath<GameObject>(n);
            roomPieces.Add(go);
        }
#if UNITY_EDITOR
        //EditorUtility.SetDirty(gameObject);
        EditorUtility.SetDirty(this);
        AssetDatabase.Refresh();

#endif
    }
    public void AddRoom(GameObject g){
        roomPieces.Add(g);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
