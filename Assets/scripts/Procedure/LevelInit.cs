
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

/*
 * SaveGame Init DataController
 * LevelInit Use DataController to Init Level 
 * SelectLevelInit  Load Data from json File
 */ 
namespace ChuMeng
{
	/*
	 * LevelInit  And BattleManager
	 * 
	 * LevelInit Init Dynamic Data From DataController
	 * BattleManger Init Static Data From Level Config
	 */ 

	/// <summary>
	/// 关卡公共初始化代码
	/// </summary>
	public class LevelInit : MonoBehaviour
	{
        /*
		GameObject levelPublic;
		GameObject startPoint;
		QuestDataController quest;

		public GameObject environmentParticle;


		void InitScene() {

			Debug.Log("LevelInit::  SaveGame Exits?");
			
			//用于测试 关卡的时候 初始化一个SaveGame
			//类似于GameProcedureMain 中的流程角色 :: 用于初始化一下场景
			if (SaveGame.saveGame == null) {
				//var saveGame = Instantiate (Resources.Load<GameObject> ("levelPublic/saveGame")) as GameObject;
				var g = new GameObject();
				var saveGame = g.AddComponent<SaveGame>();
				saveGame.GetComponent<SaveGame> ().InitData ();
			} else {
			}

			
			StartCoroutine (InitDataAndPlayer ());
		}
		void Awake() {
		}

		//初始化数据 并且 初始化主UI
		IEnumerator InitDataAndPlayer() {
			//Wait For UI Awake Over
			int count = 5;
			while (count-- > 0) {
				yield return null;
			}

			Log.Sys ("LevelInit:: InitDataAndPlayer");
			NetDebug.netDebug.AddConsole ("LevelInit Initial Scene");
			//Reset BackPack Data
			//只有第一次进入游戏场景的时候才 从网络初始化背包 和 技能数据
			if (KBEngine.KBEngineApp.app != null) {
				yield return StartCoroutine(SaveGame.saveGame.InitDataFromNetwork());
			}

			NetDebug.netDebug.AddConsole ("LevelInit:: After Initial backpack");


			if (environmentParticle != null) {
				var player = ObjectManager.objectManager.GetMyPlayer();
				var par = Instantiate(environmentParticle) as GameObject;
				par.transform.parent = player.transform;
				par.transform.localPosition = Vector3.zero;	
			}

			//初始化场景UI   等待上个场景的GUI清理结束再 初始化本场景的GUI
			Log.GUI ("Push Main UI ");
			var uiName = GetUI ();
			WindowMng.windowMng.PushView (uiName, false);
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateShortCut);
		}



		void InitQuest() {
			quest = QuestDataController.questDataController;
			quest.InitQuestItem ();
		}

		// Use this for initialization
		void Start ()
		{
			InitScene ();
			InitQuest ();
		}
	
	
    */

		
	}

}