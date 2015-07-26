using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    /// <summary>
    /// No Use Any More 
    /// </summary>
	public class SelectCopyUI : IUserInterface
	{
		List<GameObject> copyItem;
		List<LevelInfo> copyInfo;
		GameObject copyMesh;
		void Awake() {
			regEvt = new List<MyEvent.EventType> () {
				MyEvent.EventType.UpdateCopyList,
			};
			RegEvent ();
			copyMesh = GetName ("Copy");
			copyItem = new List<GameObject> ();
			copyItem.Add (copyMesh);
		}

		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}
		void OnCopy(int i) {
			Hide (null);
			var evt = new MyEvent (MyEvent.EventType.SelectCopy);
			evt.levelInfo = copyInfo [i];
			MyEventSystem.myEventSystem.PushEvent (evt); 
		}

		void UpdateFrame() {
			copyInfo = GameInterface_Team.teamInterface.GetAllCopyList ();
			while (copyItem.Count < copyInfo.Count) {
				copyItem.Add(NGUITools.AddChild(copyMesh.transform.parent.gameObject, copyMesh));
			}
			for (int i = 0; i < copyInfo.Count; i++) {
				SetText(copyItem[i], "CopyName", copyInfo[i].LevelName);
				copyItem[i].SetActive(true);
				int temp = i;
				UIEventListener.Get(copyItem[i]).onClick = delegate(GameObject g) {
					OnCopy(temp);
				};
			}
			for (int i=copyInfo.Count; i < copyItem.Count; i++) {
				copyItem[i].SetActive(false);
			}
			copyMesh.transform.parent.GetComponent<UIGrid> ().Reposition ();
		}
	}
}
