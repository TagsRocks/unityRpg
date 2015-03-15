using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class TeamUI : IUserInterface
	{
		GameObject teamMesh;
		List<GameObject> teamsItem;
		List<TeamInfo> team;
		int curSelect;
		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.UpdateTeam,
				MyEvent.EventType.TeamStateChange,
			};

			teamMesh = GetName ("TeamMesh");
			teamsItem = new List<GameObject> ();
			teamsItem.Add (teamMesh);

			SetCallback ("Create Button", OnCreate);
			SetCallback ("Apply Button", OnApply);
			SetCallback ("CloseButton", Hide);

		}
		void OnEnable() {
			RegEvent ();
		}
		void OnDisable() {
			DropEvent ();
		}

		void OnCreate(GameObject g) {
			Hide (null);
			WindowMng.windowMng.PushView ("UI/TeamInfo");
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateTeamInfo);
		}
			
		void OnApply(GameObject g) {
			var t = team [curSelect];
			GameInterface_Team.teamInterface.ApplyTeam (t);
		}


		protected override void OnEvent (MyEvent evt)
		{
			if (!gameObject.activeSelf) {
				return;
			}
			if (evt.type == MyEvent.EventType.TeamStateChange) {
				if(TeamController.teamController.IsInTeam()){
					Hide(null);
				}
			} else {
				UpdateFrame ();
			}
		}

		void OnTeam(int tid) {
			curSelect = tid;
		}

		void UpdateFrame() {
			curSelect = 0;
			team = GameInterface_Team.teamInterface.GetTeamList ();
			Log.GUI ("Init Team List "+team.Count);
			while (teamsItem.Count < team.Count) {
				teamsItem.Add(NGUITools.AddChild(teamMesh.transform.parent.gameObject, teamMesh));
			}
			for (int i = 0; i < team.Count; i++) {
				var t = teamsItem[i];
				var name = Util.FindChildRecursive(t.transform, "TeamName");
				name.GetComponent<UILabel>().text = team[i].data.TeamName;
				SetText(t, "Copy", team[i].CopyName);
				SetText(t, "Difficulty", Util.Diffcult(team[i].Difficult));
				SetText(t, "Number", team[i].data.CurrentCount.ToString()+"/"+team[i].data.TotalCount.ToString());
				int temp = i;
				UIEventListener.Get(t).onClick = delegate(GameObject g) {
					OnTeam(temp);
				};
			}
			for (int i=team.Count; i < teamsItem.Count; i++) {
				teamsItem[i].SetActive(false);
			}
		}
	}
}
