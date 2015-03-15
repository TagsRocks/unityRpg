
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class ContestController : MonoBehaviour
	{
		public static ContestController contestController;

		void Awake() {
			contestController = this;
			DontDestroyOnLoad (this);
		}

		/*
		 *  邀请切磋 
		 */ 
		public IEnumerator LoadContestInvite (int inviteId)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGContestInvite.CreateBuilder ();
			load.InvitedId = inviteId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  回复切磋邀请 
		 */ 
		public IEnumerator ContestReplayInvite (int replayinviteId,ReplayState state)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGContestReplayInvite.CreateBuilder ();
			load.ReplyInviteId = replayinviteId;
			load.ReplyState = state;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 * 推送切磋邀请
		 */ 
		public void PushContestInvite(GCPushContestInvite ContestInvite) {
			
		}

		/*
		 * 推送切磋邀请回复
		 */ 
		public void PushContestReplayInvite(GCPushContestReplayInvite ReplayInvite) {
			
		}

		/*
		 * 推送开始倒计时
		 */ 
		public void PushContestCountDown(GCPushContestCountDown CountDown) {
			
		}

		/*
		 * 推送切磋开始、结束状态(结束，击败需要复活)
		 */ 
		public void PushContestState(GCPushContestState CountState) {
			
		}

		/*
		 * 推送切磋结果[0:平,1:胜,2:输]
		 */ 
		public void PushContestResult(GCPushContestResult CountResult) {
			
		}
	}
}
