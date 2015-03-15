using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class TeamInfoUI : IUserInterface
	{
		LevelInfo selCopy = null;
		GameObject okButton;

		List<GameObject> memList;
		GameObject mem;
		void Awake() {
			mem = GetName ("member");
			memList = new List<GameObject> ();
			memList.Add (mem);

			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.UpdateTeamInfo,
				MyEvent.EventType.SelectCopy,
				MyEvent.EventType.TeamStateChange,
			};
			RegEvent ();

			SetCallback ("Copy Name", OnCopy);
			SetCallback ("okButton", OnOk);
			okButton = GetName ("okButton");
			SetCallback ("CloseButton", Hide);
			SetCallback ("LeaveButton", OnLeave);
			SetCallback ("EnterButton", OnEnter);
		}

		void OnLeave(GameObject g){
			//Hide (null);
			GameInterface_Team.teamInterface.LeaveTeam ();
		}

		void OnEnter(GameObject g) {
			Hide (null);
			GameInterface_Team.teamInterface.EnterCopy ();
		}

		void OnCopy(GameObject g) {
			WindowMng.windowMng.PushView ("UI/CopyList");
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateCopyList);
		}
		void OnOk(GameObject g) {
			var teamName = GetName ("nameInput").GetComponent<UIInput> ().value;
			var diff = Util.GetDiff(GetName ("Difficulty").GetComponent<UIPopupList> ().value);
			var copyId = selCopy.CopyId;
			GameInterface_Team.teamInterface.CreateTeam (teamName, diff, copyId);
		}


		protected override void OnEvent (MyEvent evt)
		{
			if (evt.type == MyEvent.EventType.TeamStateChange) {
				UpdateFrame();
			}else if (evt.type == MyEvent.EventType.SelectCopy) {
				selCopy = evt.levelInfo;
				UpdateFrame();
			} else {
				UpdateFrame ();
			}
		}

		void UpdateFrame() {
			Log.GUI ("team info is in team "+TeamController.teamController.IsInTeam());
			if (TeamController.teamController.IsInTeam ()) {
				Log.GUI("Init in team ui");
				GetName ("okButton").SetActive (false);
				GetName ("LeaveButton").SetActive (true);
				var myTeam = TeamController.teamController.GetMyTeam();
				//check If is Leader
				if(myTeam.IsLeader) {
					GetName ("EnterButton").SetActive (true);
				}else {
					GetName("EnterButton").SetActive(false);
				}

				GetLabel("CopyName").text = myTeam.CopyName;
				GetName("Copy Name").SetActive(false);


				GetName("nameInput").GetComponent<UIInput>().value = myTeam.TeamName;
				GetLabel("teamName").text = myTeam.TeamName;
				GetName("nameInput").GetComponent<UIInput>().enabled = false;

				GetName("Difficulty").GetComponent<UIPopupList>().enabled = false;
				GetLabel("diff").text = Util.Diffcult(myTeam.Difficult);


				GetName("MyTeamView").SetActive(true);
				var members = myTeam.Members;
				while(memList.Count < members.Count) {
					memList.Add(NGUITools.AddChild(mem.transform.parent.gameObject, mem));
				}
				Log.GUI("Team Member Number "+members.Count);
				for(int i = 0; i < members.Count; i++) {
					SetText(memList[i], "Name", members[i].PlayerName);
					SetText(memList[i], "Occupation",  Util.GetJob(members[i].Job));
					SetText(memList[i], "Level", members[i].Level.ToString());
					memList[i].SetActive(true);
				}
				for(int i = members.Count; i < memList.Count; i++) {
					memList[i].SetActive(false);
				}
				mem.transform.parent.GetComponent<UIGrid>().Reposition();

			} else {
				Log.GUI("Init Team Info "+selCopy);
				GetName ("okButton").SetActive (true);
				GetName ("LeaveButton").SetActive (false);
				GetName("EnterButton").SetActive(false);


				GetName("Copy Name").SetActive(true);
				if(selCopy != null) {
					GetLabel("CopyName").text = selCopy.LevelName;
				}else {
					GetLabel("CopyName").text = "选择副本";
				}
				GetName("nameInput").GetComponent<UIInput>().enabled = true;
				GetName("Difficulty").GetComponent<UIPopupList>().enabled = true;

				GetName("MyTeamView").SetActive(false);
			}

		}
	}

}
