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
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * Configure Level Monster Spawn Position And Monster Type And Number 
 */ 
namespace ChuMeng {
	public class SpawnTrigger : MonoBehaviour {
		public int waveNum;

		public bool Forever = false;


		public enum GroupEnum {
			Monster,
		}
		public GroupEnum Group;

		public GameObject Resource;
		public int MonsterID = -1;


		public bool reset = false;

		public bool isSpawnYet = false;
		public int Level = 1;
		public int GroupNum = 1;

		public float Radius = 0;
		public bool AddLevel = false;

		public int TotalWave = 5;

		public GameObject waveText;
		int oldWaveNum = -1;

		int curWaveNum = 0;
		void Awake() {
			if (gameObject.name.Contains ("wave")) {
				var num = Convert.ToInt32(gameObject.name.Replace("wave", ""));
				waveNum = num;
			}

			isSpawnYet = false;
			ClearChildren ();
		}
		// Use this for initialization
		void Start () {
			
		}

		bool setResourceYet = false;
		GameObject oldResource;
		GameObject showRes;
		string GetWave() {
			if (gameObject.name.Contains ("wave")) {
				var num = Convert.ToInt32(gameObject.name.Replace("wave", ""));
				waveNum = num;
			}
			return "" + waveNum;
		}


		public void UpdateEditor() {

#if UNITY_EDITOR
			if(!EditorApplication.isPlaying) {
				if(MonsterID != -1) {
					var mData = GMDataBaseSystem.database.SearchId<MonsterFightConfigData>(GameData.MonsterFightConfig, MonsterID);
					Resource = Resources.Load<GameObject>(mData.model);
				}

				if(oldResource != Resource) {
					if(showRes != null) {
						GameObject.DestroyImmediate(showRes);
						showRes = null;
					}

					foreach(Transform t in transform) {
						GameObject.DestroyImmediate(t.gameObject);
					}
				
					if(Resource != null) {
						showRes = GameObject.Instantiate(Resource) as GameObject;
						showRes.transform.parent = transform;
						showRes.transform.localPosition = Vector3.zero;
					}
					oldResource = Resource;
				}
				if(showRes != null) {
					showRes.transform.localPosition = Vector3.zero;
				}
				if(reset) {
					ClearChildren();
				}
			}else {
				if(showRes != null) {
					GameObject.Destroy(showRes);
					showRes = null;
					oldResource = null;
				}
			}
			GetWave();
			if(oldWaveNum != waveNum) {
				if(waveText != null) {
					GameObject.DestroyImmediate(waveText);
					waveText = null;
				}
				waveText = Instantiate(Resources.Load<GameObject>("TextFont")) as GameObject;
				waveText.transform.parent = transform;
				waveText.transform.localPosition = Vector3.zero;
				waveText.GetComponent<TextMesh>().text = GetWave();
				oldWaveNum = waveNum;
			}
#endif
		}
		void ClearChildren() {
			List<GameObject> g = new List<GameObject>();
			foreach(Transform t in transform) {
				g.Add(t.gameObject);
			}
			foreach(GameObject g1 in g)  {
				GameObject.DestroyImmediate(g1);
			}
			reset = false;
			showRes = null;
			oldResource = null;
		}
		/// <summary>
		/// 通过ObjectManager来生成新的怪物对象
		/// </summary>
		/// <returns>The monster.</returns>
		IEnumerator GenerateMonster() {
			for(int i = 0; i < GroupNum; i++) {
				ObjectManager.objectManager.CreateMonster(Util.GetUnitData(false, MonsterID, 0), this);
				yield return new WaitForSeconds(1);
			}
			
			curWaveNum++;
			if(Forever && curWaveNum < TotalWave) {
				waveNum++;
				isSpawnYet = false;
				if(AddLevel) {
					Level++;
				}
			}
			yield return null;
		}

		// Update is called once per frame
		void Update () {
//#if UNITY_EDITOR
			//if(showRes != null) {
			//ClearChildren();
			//}
//#endif
			if(BattleManager.battleManager == null) {
				Debug.LogError("SpawnTrigger:: battleManager Not Init");
				return;
			}

			if(!isSpawnYet && BattleManager.battleManager.waveNum == waveNum && (Resource != null || MonsterID != -1)) {
				isSpawnYet = true;
				StartCoroutine(GenerateMonster());
			}

		}
		
	}
}
