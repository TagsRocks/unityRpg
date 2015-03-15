
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng {
	public class SoulTower : MonoBehaviour {
		public static SoulTower soulTower;
		void Awake() {
			soulTower = this;
			DontDestroyOnLoad (gameObject);
		}
	}

}