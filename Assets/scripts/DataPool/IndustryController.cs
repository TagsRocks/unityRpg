
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class IndustryController : MonoBehaviour
	{
		public static IndustryController industryController;
		void Awake() {
			industryController = this;
			DontDestroyOnLoad (gameObject);
		}

		public void UpdateEnergy (ChuMeng.GCPushPlayerEnegry data)
		{
			throw new System.NotImplementedException ();
		}
	}

}