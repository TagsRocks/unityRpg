
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using StringInject;

namespace ChuMeng
{
	public class NotifyUI : IUserInterface
	{
		public UILabel label;

		void Awake ()
		{
			label = GetLabel ("notifyLabel");
		}

	
		public void SetTime(float t) {
			var lt = Mathf.FloorToInt (t);
			var ht = new Hashtable ();
			ht.Add ("TIME", lt);
			label.text = Util.GetString ("leftTime").Inject(ht);
		}

		public void SetText(string text) {
			label.text = text;
		}
		IEnumerator WaitTime(float t) {
			yield return new WaitForSeconds (t);
			OnlyHide ();
		}

		public void SetDurationTime (float t)
		{
			StartCoroutine (WaitTime(t));
		}
	}

}