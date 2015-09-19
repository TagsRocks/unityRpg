using UnityEngine;
using System.Collections;

namespace EventHandler {
	public class SpawnParticleHandler : IEventHandler {
		#region implemented abstract members of IEventHandler

		public override void OnEvent (ChuMeng.MyEvent evt)
		{
            GameObject p;
            if(string.IsNullOrEmpty(evt.particle)) {
                p = GameObject.Instantiate (evt.particle2) as GameObject;
            }else {
    			var parName = "particles/" + evt.particle;
                Log.Ani ("Skill spawn particle "+parName);
    			p = GameObject.Instantiate (Resources.Load<GameObject> (parName)) as GameObject;
            }
			NGUITools.AddMissingComponent<RemoveSelf> (p);
			if (!string.IsNullOrEmpty( evt.boneName)) {
				p.transform.parent = ChuMeng.Util.FindChildRecursive(evt.player.transform, evt.boneName);
				//var y = evt.player.transform.localRotation.eulerAngles.y;
				//var playerForward = Quaternion.Euler (new Vector3 (0, y, 0));

				p.transform.localPosition = evt.particleOffset;
				p.transform.localRotation = Quaternion.identity;
				p.transform.localScale = Vector3.one;
			} else {
				//p.transform.parent = evt.player.transform;
				//var y = evt.player.transform.localRotation.eulerAngles.y;
				//var playerForward = Quaternion.Euler (new Vector3 (0, y, 0));
                var sync = p.AddComponent<ChuMeng.SyncPosWithTarget>();
                sync.target = evt.player;

				p.transform.localPosition = evt.particleOffset;
				p.transform.localRotation = Quaternion.identity;
				p.transform.localScale = Vector3.one;

                /*
                var xft = p.GetComponent<XffectComponent>();
                xft.enabled = false;
                var npc = evt.player.GetComponent<ChuMeng.NpcAttribute>();
                if(npc != null) {
                    npc.StartCoroutine(EnableXft(xft));
                }
                */
			}
		}
        IEnumerator EnableXft(XffectComponent xft) {
            yield return null;
            xft.enabled = true;
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
