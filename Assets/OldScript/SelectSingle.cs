using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	/// <summary>
	/// 选择单人还是多人
	/// </summary>
	public class SelectSingle : IUserInterface
	{
		void Awake() {
			SetCallback ("LeftSelect", OnSingle);
			SetCallback ("RightSelect", OnMultiple);
			SetCallback ("CloseButton", Hide);
		}
		void OnSingle(GameObject g) {
			Hide (null);
			WindowMng.windowMng.PushView ("UI/CopyMap");
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OpenCopyUI);
		}

		void OnMultiple(GameObject g) {
			Hide (null);
			if (TeamController.teamController.IsInTeam ()) {
				WindowMng.windowMng.PushView("UI/TeamInfo");
				MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateTeamInfo);
			} else {
				WindowMng.windowMng.PushView ("UI/Team");
				MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateTeam);
			}
		}


	}
}
