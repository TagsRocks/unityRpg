﻿
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
		//[HideInInspector]
		public List<GameObject>enemyList;
		public DungeonData DungeonConfig;
		//public AstarPath PathInfo;
		public bool StartSpawn = false;
		public int MaxWave = 5;

        public List<GameObject> Zones;

		public int currentZone = -1;
		// Use this for initialization
		public GameObject exitZone;
		public List<SpawnTrigger> allWaves;
		public GameObject enterZone;

		public bool levelOver = false;

        [ButtonCallFunc()]
        public bool killAll = false;
        public void killAllMethod(){
            foreach(var e in enemyList){
                var npc = e.GetComponent<NpcAttribute>();
                npc.ChangeHP(-npc.HP_Max);

            }
        }


		void Awake ()
		{
            Zones = new List<GameObject>();

			levelOver = false;
			battleManager = this;
            allWaves = new List<SpawnTrigger>();

			Debug.Log ("BattleManager:: init UI ");
			enemyList = new List<GameObject> ();

			//TODO:增加场景中到处跑的小动物 和场景中可破坏的物体
			//DungeonConfig.InitCreep (PathInfo);
			//DungeonConfig.InitProps (PathInfo);

			currentZone = -1;

			for (int i = 1; i < Zones.Count; i++) {
				var zone = Zones [i];
				zone.transform.Find ("properties").gameObject.SetActive (false);
			}
			if (Zones.Count > 0) {
				InitZone ();
			}

		}

        /// <summary>
        /// 初始化Room怪物信息
        /// </summary>
        /// <param name="z">The z coordinate.</param>
        public void AddZone(GameObject z){

            Zones.Add(z);
            InitZoneState(z);
            if(currentZone == -1){
                currentZone = 0;
                InitZone();
            }
        }
        void InitZoneState(GameObject z){
            z.transform.Find("properties").gameObject.SetActive(false);
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
			yield return WorldManager.worldManager.StartCoroutine(WorldManager.worldManager.ChangeScene (2, false));
		}

		void OnPlayerDead (GameObject g)
		{
			levelOver = true;
			StartCoroutine (ShowFail ());
			
		}
		void InitZone ()
		{
            Log.Sys("InitZone Properties ");
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
            Log.Sys("GotoNextZone");
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
            var protectWall = Zones[currentZone].transform.Find("protectWall");
            protectWall.gameObject.SetActive(false);

			//Wait For Player pass Zone Enter New Zone
			while (!ezone.Enter) {
				yield return null;
			}
			//Can't GoBack
			if (exitZone != null) {
				//exitZone.SetActive (true);
				exitZone.GetComponent<ExitWall>().CloseDoor();
			}


            if(protectWall != null){
                protectWall.GetComponent<ProtectWall>().ShowWall();
            }


			InitZone ();
            StartCoroutine(StreamLoadLevel.Instance.MoveInNewRoom());

        }

		//wait for a while to rest goto next wave
		IEnumerator NextWave ()
		{
            Log.Sys("NextWave Start");
			yield return new WaitForSeconds (3);
			if (levelOver) {
				yield break;
			}

			waveNum++;
            Log.Sys("NewWaveNum "+waveNum + " MaxWave "+MaxWave);
			if (waveNum >= MaxWave) {
				currentZone++;
                Log.Sys("currentZone zoneCount "+currentZone+" count "+Zones.Count);
				if (currentZone < Zones.Count) {
					yield return StartCoroutine (GotoNextZone ());
				} else {
					Log.Sys("BattleManager::NextWave No Wave Battle Finish "+MaxWave);
					yield return StartCoroutine (LevelFinish ());
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The finish.</returns>
		IEnumerator LevelFinish ()
		{
            Log.Sys("LevelFinish ");
            float leftTime = 5f;
            var notify = WindowMng.windowMng.ShowNotifyLog("", 5.2f).GetComponent<NotifyUI>();
			while (leftTime > 0) {
				//notify.SetTime (leftTime);
                notify.SetText(string.Format("退出副本倒计时{0}s", (int)leftTime));
				leftTime -= Time.deltaTime;
				yield return null;
			}
            notify.SetText(string.Format("退出副本倒计时{0}s", (int)0));


			var victoryUI = WindowMng.windowMng.PushView ("UI/victory").GetComponent<VictoryUI>();
			while (!victoryUI.con) {
				yield return null;
			}
			WindowMng.windowMng.PopView ();

			yield return WorldManager.worldManager.StartCoroutine(WorldManager.worldManager.ChangeScene (2, false));
		}

        /// <summary>
        /// 怪物死亡事件 触发下一波怪
        /// </summary>
        /// <param name="go">Go.</param>
		public void EnemyDead (GameObject go)
		{
            Log.Sys("MonsterDead "+go.name+" list "+enemyList.Count);
			enemyList.Remove (go);
			if (enemyList.Count > 0) {
				return;
            }

			enemyList.Clear ();
			StartCoroutine (NextWave ());
		}



	}
}