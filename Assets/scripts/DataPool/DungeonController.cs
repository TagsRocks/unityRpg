
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

/*
 * 副本功能接口
 */ 

namespace ChuMeng
{
	public class DungeonController : MonoBehaviour
	{
		public static DungeonController dungeonController;

		void Awake ()
		{
			dungeonController = this;
			DontDestroyOnLoad (gameObject);
		}

		/*
		 * 进入副本
		 */ 
		public IEnumerator enterDungeon(int dungeonBaseId) {
			var packet = new KBEngine.PacketHolder ();
			var load = CGEnterDungeon.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, load, packet));

		}

		/*
		 * 退出副本
		 */ 
		public IEnumerator exitDungeon(){
			var packet = new KBEngine.PacketHolder ();
			var load = CGExitDungeon.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, load, packet));
		}

		/*
		 * 强制退出副本
		 */ 

		public void CoerceExitDungeon(GCPushCoerceExitDungeon exit) {
		}
	}
}
