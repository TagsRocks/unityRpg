
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
using System.Collections.Generic;

//TODO:临时存储的文本数据 之后要删除掉 用json文件替换
public class ItemToolTipFormat : MonoBehaviour {
	[System.Serializable]
	public class KeyValue {
		public string key;
		[Multiline()]
		public string value;
	}

	public List<KeyValue> kvText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetString(string key) {
		foreach (var kv in kvText) {
			if(kv.key == key) {
				return kv.value;
			}
		}
		return null;
	}
}
