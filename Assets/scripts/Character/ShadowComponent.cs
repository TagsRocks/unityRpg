
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

public class ShadowComponent : MonoBehaviour {
	GameObject shadowPlane;
	/*
	 * Npc Attribute Create ShadowPlane for Monster or Player
	 */ 
	void CreateShadowPlane() {
		if (shadowPlane == null) {
			GameObject p = GameObject.CreatePrimitive (PrimitiveType.Plane);
			shadowPlane = p;
			p.newName = "shadowPlane";
			p.transform.parent = transform;
			p.transform.localScale = Vector3.one;
			p.transform.localRotation = Quaternion.identity;
			p.renderer.enabled = false;
			DestroyImmediate (p.collider);
			p.transform.localPosition = Vector3.zero;
		
			foreach (Transform c in transform) {
				if (c.renderer != null) {
					SetShadowPlane sp = NGUITools.AddMissingComponent<SetShadowPlane> (c.gameObject);
					sp.plane = p;
				}
			}
		}
	}

	public void HideShadow() {
		shadowPlane.transform.localPosition = Vector3.up*-100;
	}

	void Awake() {
		CreateShadowPlane ();
	}
	/*
	 *  
	 */
	public void AdjustLightPos(Vector3 pos) {
		var me = transform.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer r in me) {
			r.material.SetVector("_LightDir", transform.position-pos);
		}
	}

	/*
	 * Add Shadow Plane for new Equipment like:Armor Chest Weapon
	 */ 
	public void SetShadowPlane(GameObject g) {
		SetShadowPlane sp = NGUITools.AddMissingComponent<SetShadowPlane>(g);
		sp.plane = shadowPlane;
	}
}
