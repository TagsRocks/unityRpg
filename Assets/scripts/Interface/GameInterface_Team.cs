using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ChuMeng
{
	public class GameInterface_Team
	{
		public static GameInterface_Team teamInterface = new GameInterface_Team();
		public List<TeamInfo> GetTeamList() {
			return TeamController.teamController.GetTeamList ();
		}

		public void ApplyTeam(TeamInfo t) {
			TeamController.teamController.StartCoroutine (TeamController.teamController.ApplyTeam(t.TeamId));
		}

		public List<LevelInfo> GetAllCopyList ()
		{
			return CopyController.copyController.GetAllCopy ();
		}

		public void CreateTeam(string teamName, int diff, int mapId) {
			TeamController.teamController.StartCoroutine (TeamController.teamController.CreateTeam(teamName, diff, mapId));
		}

		public void LeaveTeam ()
		{
			TeamController.teamController.StartCoroutine (TeamController.teamController.LeaveTeam ());
		}

		public void EnterCopy ()
		{
			throw new System.NotImplementedException ();
		}
	}
}
