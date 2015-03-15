
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

public class UIManager : MonoBehaviour {
	public List<GameObject> stacks;
	void Awake() {
		DontDestroyOnLoad (this.gameObject);

		stacks = new List<GameObject> ();

	}
	public void PushView(GameObject view) {
		Debug.Log ("Push View "+view+" "+stacks.Count);
		if (stacks.Count > 0) {
			stacks[stacks.Count-1].SetActive(false);
		}
		stacks.Add (view);
		view.SetActive (true);
	}
	public GameObject PopView() {
		Debug.Log ("PopView Is "+stacks.Count);
		var last = stacks[stacks.Count-1];
		stacks.RemoveAt (stacks.Count-1);
		last.SetActive (false);
		if (stacks.Count > 0) {
			stacks [stacks.Count - 1].SetActive (true);
		}
		return last;
	}

	public void PopAll() {
		Debug.Log("PopAllView ");
		foreach (GameObject g in stacks) {
			g.SetActive(false);
		}
		stacks.Clear ();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
