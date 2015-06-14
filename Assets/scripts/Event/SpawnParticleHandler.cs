using UnityEngine;
using System.Collections;

namespace EventHandler {
	public class SpawnParticleHandler : IEventHandler {
		#region implemented abstract members of IEventHandler

		public override void OnEvent (ChuMeng.MyEvent evt)
		{
			var parName = "particles/" + evt.particle;
			Log.Ani ("Skill spawn particle "+parName);
			var p = GameObject.Instantiate (Resources.Load<GameObject> (parName)) as GameObject;
			NGUITools.AddMissingComponent<RemoveSelf> (p);
			if (evt.boneName != "") {
				p.transform.parent = ChuMeng.Util.FindChildRecursive(evt.player.transform, evt.boneName);
				//var y = evt.player.transform.localRotation.eulerAngles.y;
				//var playerForward = Quaternion.Euler (new Vector3 (0, y, 0));

				p.transform.localPosition = evt.particleOffset;
				p.transform.localRotation = Quaternion.identity;
				p.transform.localScale = Vector3.one;
			} else {
				p.transform.parent = evt.player.transform;
				//var y = evt.player.transform.localRotation.eulerAngles.y;
				//var playerForward = Quaternion.Euler (new Vector3 (0, y, 0));

				p.transform.localPosition = evt.particleOffset;
				p.transform.localRotation = Quaternion.identity;
				p.transform.localScale = Vector3.one;
			}
		}
		public override void Init ()
		{
			regEvent = new System.Collections.Generic.List<ChuMeng.MyEvent.EventType> () {
				ChuMeng.MyEvent.EventType.SpawnParticle,
			};
		}
		#endregion

	}
}
