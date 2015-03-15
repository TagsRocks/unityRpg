using UnityEngine;
using System.Collections;

namespace EventHandler
{
	public class DeadExpHandler : IEventHandler
	{
		#region implemented abstract members of IEventHandler
		public override void Init ()
		{
			regEvent = new System.Collections.Generic.List<ChuMeng.MyEvent.EventType> () {
				ChuMeng.MyEvent.EventType.DeadExp,
			};
		}
		public override void OnEvent (ChuMeng.MyEvent evt)
		{
			Log.Sys ("Dead Exp Add Event");
			var dead = evt as ChuMeng.DeadExpEvent;
			var player = ChuMeng.ObjectManager.objectManager.GetMyPlayer ();
			var attr = player.GetComponent<ChuMeng.NpcAttribute> ();
			attr.ChangeExp (dead.exp);
		}
		#endregion

	}

}