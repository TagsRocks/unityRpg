
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
using System.Collections.Generic;


namespace ChuMeng
{
	/// <summary>
	/// 关卡相关配置信息
	/// </summary>
	public class BattleManager : KBEngine.MonoBehaviour
	{
		/*
		 * Current Wave
		 */ 
		public int waveNum = 0;
		public static BattleManager battleManager;
		[HideInInspector]
		public List<GameObject>enemyList;
		public DungeonData DungeonConfig;
		//public AstarPath PathInfo;
		public bool StartSpawn = false;
		public int MaxWave = 5;

		public List<GameObject> Zones;

		public int currentZone = 0;
		// Use this for initialization
		public GameObject exitZone;
		public List<SpawnTrigger> allWaves;
		public GameObject enterZone;

		public bool levelOver = false;

		//public FailUI failUI;

		public bool CheckDead = true;

		void Awake ()
		{
			levelOver = false;
			battleManager = this;

			Debug.Log ("BattleManager:: init UI ");
			enemyList = new List<GameObject> ();

			//TODO:增加场景中到处跑的小动物 和场景中可破坏的物体
			//DungeonConfig.InitCreep (PathInfo);
			//DungeonConfig.InitProps (PathInfo);

			currentZone = 0;

			for (int i = 1; i < Zones.Count; i++) {
				var zone = Zones [i];
				zone.transform.Find ("properties").gameObject.SetActive (false);
			}
			if (Zones.Count > 0) {
				InitZone ();
			}

		}

		IEnumerator Start ()
		{
			while (ObjectManager.objectManager == null) {
				yield return null;
			}
			regEvt = new List<MyEvent.EventType> (){
				MyEvent.EventType.PlayerDead,
			};
			RegEvent ();
		}

		protected override void OnEvent (MyEvent evt)
		{
			if (evt.type == MyEvent.EventType.PlayerDead) {
				OnPlayerDead(null);
			}
		}

		IEnumerator ShowFail ()
		{
			var failUI = WindowMng.windowMng.PushView ("UI/FailPanel").GetComponent<FailUI>();
			while (!failUI.quit) {
				yield return null;
			}
			WindowMng.windowMng.PopView ();

			//从副本回到主城
			yield return WorldManager.worldManager.StartCoroutine(WorldManager.worldManager.ChangeScene (1, false));
		}

		void OnPlayerDead (GameObject g)
		{
			levelOver = true;
			StartCoroutine (ShowFail ());
			
		}

		void InitZone ()
		{
			allWaves.Clear ();

			var prop = Zones [currentZone].transform.Find ("properties");
			var ex = Util.FindChildRecursive (Zones [currentZone].transform, "exitZone");
			if (ex != null) {
				exitZone = ex.gameObject;
			}

			waveNum = 0;
			MaxWave = 0;
			foreach (Transform t in prop) {
				var spawn = t.gameObject.GetComponent<SpawnTrigger> ();
				allWaves.Add (spawn);	
				if (spawn.waveNum >= MaxWave) {
					MaxWave = spawn.waveNum + 1;
				}
			}

			prop.gameObject.SetActive (true);
		}
		/*
		 * Disappear Wall Between
		 */ 
		IEnumerator GotoNextZone ()
		{
			if (exitZone != null) {
				//exitZone.SetActive (false);
				exitZone.GetComponent<ExitWall>().ZoneClear();
			}

			var ez = Zones [currentZone].transform.Find ("enterZone");
			if (ez == null) {
				Debug.LogError ("BattleManager::Next Zone has No EnterZone");
			}
			enterZone = ez.gameObject;
			var ezone = enterZone.GetComponent<EnterZone> ();

			//Wait For Player pass Zone Enter New Zone
			while (!ezone.Enter) {
				yield return null;
			}
			//Can't GoBack
			if (exitZone != null) {
				//exitZone.SetActive (true);
				exitZone.GetComponent<ExitWall>().CloseDoor();
			}
			InitZone ();
		}

		//wait for a while to rest goto next wave
		IEnumerator NextWave ()
		{
			yield return new WaitForSeconds (3);
			if (levelOver) {
				yield break;
			}

			waveNum++;
			if (waveNum >= MaxWave) {
				currentZone++;
				if (currentZone < Zones.Count) {
					yield return StartCoroutine (GotoNextZone ());
				} else {
					Debug.Log ("BattleManager::NextWave No Wave Battle Finish");
					yield return StartCoroutine (LevelFinish ());
				}
			}
		}

		IEnumerator LevelFinish ()
		{
			var notify = WindowMng.windowMng.PushView ("UI/NotifyLog").GetComponent<NotifyUI>();
			float leftTime = 5;
			while (leftTime > 0) {
				notify.SetTime (leftTime);
				leftTime -= Time.deltaTime;
				yield return null;
			}
			WindowMng.windowMng.PopView ();
			var victoryUI = WindowMng.windowMng.PushView ("UI/victory").GetComponent<VictoryUI>();
			while (!victoryUI.con) {
				yield return null;
			}
			WindowMng.windowMng.PopView ();

			yield return WorldManager.worldManager.StartCoroutine(WorldManager.worldManager.ChangeScene (1, false));
		}

		public void EnemyDead (GameObject go)
		{
			enemyList.Remove (go);
			if (enemyList.Count > 0)
				return;

			enemyList.Clear ();
			StartCoroutine (NextWave ());
		}



	}
}