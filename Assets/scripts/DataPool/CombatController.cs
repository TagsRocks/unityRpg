
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class CombatController : MonoBehaviour
	{
		public static CombatController combatController;
		public GCCombat combatInfo;

		void Awake() {
			combatController = this;
			DontDestroyOnLoad (this);
		}
		/*
		 * 加载  斗魂塔 Combat信息
		 */ 
		public IEnumerator LoadCombatInfo ()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGCombat.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			combatInfo = (packet.packet.protoBody as GCCombat);
			//combatInfo.

		}

		public List<CombatLayerInfo> GetItemList()
		{
			Debug.Log ("CombatlayerinfoList??");
			List<CombatLayerInfo> list = new List<CombatLayerInfo> ();
			for (int i = 0; i<combatInfo.CombatlayerinfoCount; i++) 
			{
				list.Add(combatInfo.CombatlayerinfoList[i]);
				Debug.Log(".........:"+combatInfo.CombatlayerinfoList[i].LayerName);
			}
			return list;
		}

	}
}
