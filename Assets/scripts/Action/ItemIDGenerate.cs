
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using ChuMeng;
using System.Collections.Generic;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
/*
 * Generate Item ID For Resource/units/items
 */ 
public class ItemIDGenerate : MonoBehaviour {
	[ButtonCallFunc()]
	public bool GenID;
	[ButtonCallFunc()]
	public bool ClearID;

	public List<ItemData> itemDataList;

	//Keep Data In ItemDataList
	void Awake() {
		DontDestroyOnLoad (this.gameObject);
	}

	public void GenIDMethod() {

#if !UNITY_WEBPLAYER
		itemDataList.Clear ();
		var parPath = Path.Combine (Application.dataPath, "Resources/units/items");
		var resPath = Path.Combine(Application.dataPath, "Resources")+Path.DirectorySeparatorChar;
		var resDir = new DirectoryInfo (parPath);

		FileInfo[] fileInfo = resDir.GetFiles ("*.*", SearchOption.AllDirectories);
		/*
		foreach (FileInfo file in fileInfo) {
			Debug.Log("file is "+file+" "+file.FullName);
			//Debug.Log(file.Extension);
			if(file.Extension == ".prefab") {
				//Debug.Log("name "+file.Name);
				var fname = file.FullName;
				//var fname = Path.GetFileNameWithoutExtension(file.FullName);
				var npath = fname.Replace(resPath, "");
				npath = npath.Replace(".prefab", "");
				Debug.Log(npath);
				//var who = Path.GetFileName(Path.GetDirectoryName(npath));
				var item = Resources.Load<GameObject>(npath);
				itemDataList.Add(item.GetComponent<ItemData>());
			}
		}
		*/

		int maxId = 0;
		foreach(ItemData it in itemDataList) {
			if(it.ObjectId >= maxId) {
				maxId = it.ObjectId+1;
			}
		}

		foreach(ItemData it in itemDataList) {
			/*
			if(it.ObjectId == -1) {
				#if UNITY_EDITOR
				AssetDatabase.StartAssetEditing();
				it.ObjectId = maxId++;
				EditorUtility.SetDirty(it);
				AssetDatabase.StopAssetEditing();
				#endif
			}
			*/
		}
#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif
#endif
	}

	public void ClearIDMethod() {
		#if !UNITY_WEBPLAYER
		itemDataList.Clear ();
		var parPath = Path.Combine (Application.dataPath, "Resources/units/items");
		var resPath = Path.Combine(Application.dataPath, "Resources")+Path.DirectorySeparatorChar;
		var resDir = new DirectoryInfo (parPath);
		
		FileInfo[] fileInfo = resDir.GetFiles ("*.*", SearchOption.AllDirectories);
		foreach (FileInfo file in fileInfo) {
			Debug.Log("file is "+file+" "+file.FullName);
			/*
			if(file.Extension == ".prefab") {
				var fname = file.FullName;
				var npath = fname.Replace(resPath, "");
				npath = npath.Replace(".prefab", "");
				Debug.Log(npath);
				var item = Resources.Load<GameObject>(npath);
				item.GetComponent<ItemData>().ObjectId = -1;
				itemDataList.Add(item.GetComponent<ItemData>());
			}
			*/
		}
		
		int maxId = 0;
		foreach(ItemData it in itemDataList) {
			if(it.ObjectId >= maxId) {
				maxId = it.ObjectId+1;
			}
		}
		/*
		foreach(ItemData it in itemDataList) {
			if(it.ObjectId == -1) {

#if UNITY_EDITOR
				AssetDatabase.StartAssetEditing();
				//it.ObjectId = maxId++;
				EditorUtility.SetDirty(it);
				AssetDatabase.StopAssetEditing();
#endif

			}
		}
		*/
		#endif
	}

	public ItemData GetItem(int oid) {
		foreach (ItemData it in itemDataList) {
			if(it.ObjectId == oid) {
				return it;
			}
		}
		return null;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
