
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class TeamInfo
	{
		Team team;
		GCCreateTeam myTeam;
		CGCreateTeam createTeam;
		GCLoadTeamInfo loadTeam;
		GCPushJoinTeam joinTeam;
		public bool IsLeader {
			get {
				if(loadTeam != null) {
					var members = Members;
					var myId = ObjectManager.objectManager.GetMyServerID();
					foreach(TeamMember m in members) {
						if(m.PlayerId == myId) {
							return m.Leader;
						}
					}
				}
				return false;
			}
		}
		//我创建者的TeamId 
		//或者队员的TeamId
		//或者别人创建的TeamId
		public int TeamId {
			get {
				if (team != null) {
					return team.TeamId;
				}
				if (myTeam != null) {
					return myTeam.TeamId;
				}
				if (joinTeam != null) {
					return joinTeam.TeamId;
				}
				return -1;
			}
		}

		//别人创建的Team的信息
		public Team data {
			get {
				return team;
			}
		}

		//别人或者 我的Team的信息
		public string TeamName {
			get {
				if (team != null) {
					return team.TeamName;
				}
				if (createTeam != null) {
					return createTeam.TeamName;
				}
				return loadTeam.TeamName;
			}
		}

		public string CopyName {
			get {
				if (team != null) {
					var data = GMDataBaseSystem.database.SearchId<DungeonConfigData> (GameData.DungeonConfig, team.MapId);
					return data.name;
				}
				if (createTeam != null) {
					var data = GMDataBaseSystem.database.SearchId<DungeonConfigData> (GameData.DungeonConfig, createTeam.MapId);
					return data.name;
				}
				{
					var data = GMDataBaseSystem.database.SearchId<DungeonConfigData> (GameData.DungeonConfig, loadTeam.MapId);
					return data.name;
				}
			}
		}

		public int MapId {
			get {
				if (team != null)
					return team.MapId;
				if (createTeam != null)
					return createTeam.MapId;
				return loadTeam.MapId;
			}
		}

		public int Difficult {
			get {
				if (team != null)
					return team.Difficult;
				if (createTeam != null) {
					return createTeam.Difficult;
				}
				return loadTeam.Difficult;

			}
		}

		//获取我的Team的成员
		public List<TeamMember> Members {
			get {
				List<TeamMember> m = new List<TeamMember> ();
				if(loadTeam != null) {
					foreach (TeamMember t in loadTeam.TeamMembersList) {
						m.Add (t);
					}
				}
				return m;
			}
		}

		//别人的队伍
		public TeamInfo (Team t)
		{
			team = t;
		}

		//自己创建的队伍
		public TeamInfo (GCCreateTeam t, CGCreateTeam createTeamInfo)
		{
			createTeam = createTeamInfo;
			myTeam = t;
		}

		//加入其它人的队伍
		public void UpdateInfo (GCLoadTeamInfo lt)
		{
			loadTeam = lt;
		}
		public TeamInfo(GCPushJoinTeam t) {
			joinTeam = t;
		}
	}

	public class TeamController : MonoBehaviour
	{
		public static TeamController teamController;
		List<TeamInfo> allTeams;
		TeamInfo myTeam = null;
		//是否阻塞UI通知
		bool block = false;
		List<GCPushTeamInvite> cacheApply;
		public bool IsInTeam ()
		{
			return myTeam != null;
		}

		public void TeamDisband ()
		{
			myTeam = null;
			WindowMng.windowMng.ShowNotifyLog("队伍被解散了", 3);
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.TeamStateChange);
		}

		public void KickMember (ChuMeng.GCPushTeamMemberKicked kick)
		{
			Log.Net ("Kick Member by server "+kick.PlayerId);
			if (kick.PlayerId == ObjectManager.objectManager.GetMyServerID ()) {
				myTeam = null;
				WindowMng.windowMng.ShowNotifyLog("被踢出了队伍", 3);
				MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.TeamStateChange);
			}
		}

		public TeamInfo GetMyTeam ()
		{
			return myTeam;
		}

		void Awake ()
		{
			teamController = this;
			DontDestroyOnLoad (teamController);
			cacheApply = new List<GCPushTeamInvite> ();
			StartCoroutine (LoadTeamInfoCor ());
			StartCoroutine (NotifyApply());
		}

		public IEnumerator JoinSuc(GCPushJoinTeam team) {

			myTeam = new TeamInfo (team);
			Log.Net ("joint team id "+myTeam.TeamId);
			yield return StartCoroutine (LoadTeamInfo(myTeam.TeamId));

			WindowMng.windowMng.ShowNotifyLog ("你加入了队伍", 3);
		}

		/*
		 * 列出所有队伍信息
		 */ 
		public IEnumerator ListAllTeams ()
		{
			allTeams = new List<TeamInfo> ();

			var packet = new KBEngine.PacketHolder ();
			var list = CGListAllTeams.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));
			var data = packet.packet.protoBody as GCListAllTeams;
			foreach (Team t in data.TeamList) {
				allTeams.Add (new TeamInfo (t));
			}
		}

		public void ReceiveApply (ChuMeng.GCPushTeamInvite apply)
		{
			Log.Net ("receive Apply "+block);
			cacheApply.Add (apply);
		}
		public void BlockNotify(bool b) {
			block = b;
		}

		public IEnumerator AcceptApply (GCPushTeamInvite invite)
		{
			var packet = new KBEngine.PacketHolder ();
			var accept = CGProcessApplyTeam.CreateBuilder ();
			accept.TargetId = invite.PlayerId;
			accept.Accept = true;
			accept.TargetName = invite.PlayerName;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, accept, packet));
		}

		public IEnumerator LeaveTeam ()
		{
			var packet = new KBEngine.PacketHolder ();
			var leave = CGLeaveTeam.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, leave, packet));
			myTeam = null;
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.TeamStateChange);
		}

		public IEnumerator RejectApply (GCPushTeamInvite invite)
		{
			var packet = new KBEngine.PacketHolder ();
			var accept = CGProcessApplyTeam.CreateBuilder ();
			accept.TargetId = invite.PlayerId;
			accept.Accept = false;
			accept.TargetName = invite.PlayerName;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, accept, packet));
		}

		IEnumerator NotifyApply() {
			while (true) {
				if(cacheApply.Count > 0 && !block){
					WindowMng.windowMng.PushView("UI/ShenPi");
					var evt = new MyEvent(MyEvent.EventType.UpdateShenPi);
					evt.teamInvite = cacheApply[0];
					cacheApply.RemoveAt(0);
					MyEventSystem.myEventSystem.PushEvent(evt);
				}
				yield return new WaitForSeconds(1);
			}
		}

		public List<TeamInfo> GetTeamList ()
		{
			return allTeams;
		}

		/*
		 * 获取队伍详细信息
		 * TODO:更新队伍成员信息
		 */ 
		public IEnumerator LoadTeamInfo (int teamId)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadTeamInfo.CreateBuilder ();
			load.TeamId = teamId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));

			if (myTeam != null) {
				myTeam.UpdateInfo (packet.packet.protoBody as GCLoadTeamInfo);
			}
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.TeamStateChange);
		}

		/*
		 * 创建队伍----> Create Success Show TeamInfo other 
		 */ 
		public IEnumerator CreateTeam (string teamName, int diff, int mapId)
		{
			var packet = new KBEngine.PacketHolder ();
			var create = CGCreateTeam.CreateBuilder ();
			create.TeamName = teamName;
			create.MapId = mapId;
			//load.AllocateMode = mode;
			var create2 = create.Clone ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, create, packet));

			myTeam = new TeamInfo (packet.packet.protoBody as GCCreateTeam, create2.BuildPartial ());
			yield return StartCoroutine (LoadTeamInfo(myTeam.TeamId));
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.TeamStateChange);

		}

		IEnumerator LoadTeamInfoCor ()
		{
			while (true) {
				if (myTeam != null) {
					var tid = myTeam.TeamId;
					yield return StartCoroutine (LoadTeamInfo (tid));
				}
				yield return new WaitForSeconds (5);
			}
		}

		/*
		 * 组队邀请
		 */ 
		public IEnumerator SendTeamInvite (int targetId)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGSendTeamInvite.CreateBuilder ();
			load.TargetId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 * 处理组队邀请
		 */ 
		public IEnumerator ProcessInviteTeam (int targetid, bool accept)
		{
			var packet = new KBEngine.PacketHolder ();
			var p = CGProcessInviteTeam.CreateBuilder ();
			p.TargetId = targetid;
			p.Accept = accept;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, p, packet));
		}

		/*
		 * 申请加入队伍
		 */ 
		public IEnumerator ApplyTeam (int teamId)
		{
			var packet = new KBEngine.PacketHolder ();
			var p = CGApplyTeam.CreateBuilder ();
			p.TeamId = teamId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, p, packet));
		}


	}
}