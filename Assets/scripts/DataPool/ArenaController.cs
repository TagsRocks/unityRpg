
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng {
	public class ArenaController : MonoBehaviour {
		public static ArenaController arenaControoler;
		void Awake() {
			arenaControoler = this;
			DontDestroyOnLoad (gameObject);
		}
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
