
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
	public class PotionUI : MonoBehaviour {
		[HideInInspector]
		public GameObject hpPotion;
		[HideInInspector]
		public GameObject mpPotion;
		UILabel hpNum;
		UILabel mpNum;
		//public VoidDelegate OnHp;
		//public VoidDelegate OnMp;

		void Awake() {
			var potion = Util.FindChildRecursive(transform, "HpMpPotion");
			hpPotion = Util.FindChildRecursive(potion, "hpButton").gameObject;
			hpNum = Util.FindChildRecursive(potion, "hpNum").gameObject.GetComponent<UILabel>();
			//UIEventListener.Get(hpPotion).onClick += 

			mpPotion = Util.FindChildRecursive (potion, "mpButton").gameObject;
			mpNum = Util.FindChildRecursive (potion, "mpNum").gameObject.GetComponent<UILabel>() ;

			//var bp = GameObject.FindObjectOfType (typeof(BackPack)) as BackPack;
			//bp.UpdateBackpack += UpdateHpMp;
		}

		public void SetHpNum(int n) {
			hpNum.text = string.Format ("{0}", n);
		}
		public void SetMpNum(int n) {
			mpNum.text = string.Format ("{0}", n);
		}

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}

}