
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
	public class QuestComplete : MonoBehaviour {
		GameObject questFinish;
		UILabel big;
		UILabel small;
		void Awake() {
			questFinish = Util.FindChildRecursive (transform, "QuestFinish").gameObject;
			big = Util.FindChildRecursive(questFinish.transform, "big").GetComponent<UILabel>();
			small = Util.FindChildRecursive (questFinish.transform, "small").GetComponent<UILabel> ();
			questFinish.SetActive (false);
		}
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		IEnumerator WaitRemove() {
			yield return new WaitForSeconds (2);
			questFinish.SetActive (false);
		}

		public void ShowQuestComplete(QuestData qd) {
			small.text = qd.QuestDisplayName;
			questFinish.SetActive (true);
			StartCoroutine (WaitRemove());
		}
	}

}