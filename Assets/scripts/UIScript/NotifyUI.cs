
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
		UILabel label;
        UISprite bg;
		void Awake ()
		{
			label = GetLabel ("notifyLabel");
            bg = GetSprite("BG");
		}

	
		public void SetTime(float t) {
			var lt = Mathf.FloorToInt (t);
			var ht = new Hashtable ();
			ht.Add ("TIME", lt);
			label.text = Util.GetString ("leftTime").Inject(ht);
		}

		public void SetText(string text) {
			label.text = text;
            var w = label.printedSize.x;
            Log.GUI("textSize "+w);
            bg.width = (int)Mathf.Max((w+100), 310);

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