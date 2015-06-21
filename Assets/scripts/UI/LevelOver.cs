
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
namespace ChuMeng {
	public class LevelOver : KBEngine.MonoBehaviour {
		public int waveNum = 5;
		bool isSpawnYet = false;
		public GameObject Resource;

		public float WaitTime = 2;
		GameObject player;
		//GameObject uiRoot;

		void Awake() {
			//uiRoot = GameObject.FindGameObjectWithTag("UIRoot");

		}
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if (!isSpawnYet && BattleManager.battleManager.waveNum == waveNum) {
				isSpawnYet = true;
				StartCoroutine(GenResource());
			}
		}
		IEnumerator GenResource() {
			yield return new WaitForSeconds (WaitTime);
			player = GameObject.FindGameObjectWithTag("Player");

			var g = Instantiate(Resource) as GameObject;
			g.transform.position = transform.position;

		}

		void OnOk(GameObject g) {
			//Application.LoadLevel("villageScene");
			//quit Battle Scene
			GameObject.FindObjectOfType<SaveGame> ().SaveFile ();

			GameObject.FindObjectOfType<LoadingUI> ().ShowLoad ("villageScene");
		}
		/*
		 * 
		 */
		void OnTriggerEnter(Collider col) {
			if (col.tag != "Player") {
				return;
			}
			Debug.Log ("Level Over Trigger");
			if (isSpawnYet) {
				var tips = WindowMng.windowMng.PushView("UI/tips").GetComponent<TipsPanel>();
				tips.SetContent(Util.GetString("quitLevel"));
				tips.SetOk(OnOk);
			}
		}
	}

}