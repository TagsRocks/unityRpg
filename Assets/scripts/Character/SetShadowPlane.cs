﻿
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


public class SetShadowPlane : MonoBehaviour {
	public GameObject plane;

	// Update is called once per frame
	void Update () {
		for(int i = 0; i < renderer.materials.Length; i++)
		{
			renderer.materials[i].SetMatrix("_World2Receiver", plane.renderer.worldToLocalMatrix);
		}
	}
}
