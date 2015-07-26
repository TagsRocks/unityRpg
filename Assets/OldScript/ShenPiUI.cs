using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class ShenPiUI : IUserInterface
	{
		GCPushTeamInvite invite;
		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.UpdateShenPi,
			};
			RegEvent ();
			SetCallback ("okButton", OnOk);
			SetCallback ("cancelButton", OnCancel);
		}

		void OnOk(GameObject g) {
			Hide (null);
			TeamController.teamController.StartCoroutine (TeamController.teamController.AcceptApply(invite));
		}

		void OnCancel(GameObject g) {
			Hide (null);
			TeamController.teamController.StartCoroutine (TeamController.teamController.RejectApply(invite));
		}

		void OnEnable() {
			TeamController.teamController.BlockNotify (true);
		}
		void OnDisable() {
			TeamController.teamController.BlockNotify (false);
		}

		protected override void OnEvent (MyEvent evt)
		{
			if (evt.type == MyEvent.EventType.UpdateShenPi) {
				invite = evt.teamInvite;
				UpdateFrame ();
			}
		}

		void UpdateFrame() {
			GetLabel ("title").text = invite.PlayerName+"想要加入你的队伍，同意？";
		}

	}
}
