
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

namespace ChuMeng
{
	public class BloodBar : IUserInterface
	{
		float barDisplay = 0;
		Vector2 pos = new Vector2 (20, 40);
		Vector2 size = new Vector2 (40, 10);
		Texture2D progressBarEmpty;
		Texture2D progressBarFull;
		float passTime = 0;
		float curValue = 0;

		void Awake() {
			regLocalEvt = new System.Collections.Generic.List<MyEvent.EventType> (){
				MyEvent.EventType.UnitHP,
			};
			RegEvent ();
		}
		// Use this for initialization
		void Start ()
		{
			progressBarEmpty = new Texture2D (1, 1);
			progressBarEmpty.SetPixel (0, 0, new Color (0.5f, 0.5f, 0.5f, 0.8f));
			progressBarEmpty.wrapMode = TextureWrapMode.Repeat;
			progressBarEmpty.Apply ();

			progressBarFull = new Texture2D (1, 1);
			progressBarFull.SetPixel (0, 0, new Color (0.8f, 0.2f, 0.2f, 0.8f));
			progressBarFull.wrapMode = TextureWrapMode.Repeat;
			progressBarFull.Apply ();

			//GetComponent<NpcAttribute> ().ChangeHP (0);
		}
		protected override void OnLocalEvent (MyEvent evt)
		{	
			Log.Important ("Blood bar OnEvent "+gameObject.name+" type "+evt.type+" "+evt.localID+" localId "+GetComponent<KBEngine.KBNetworkView>().GetLocalId());
			Log.Important("Init HP And Max "+GetComponent<NpcAttribute>().HP + " "+GetComponent<NpcAttribute>().HP_Max);
			if (evt.type == MyEvent.EventType.UnitHP) {
				
				SetBarDisplay (GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.HP)*1.0f/GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.HP_MAX));
			}
		}

		void OnGUI ()
		{
			if (Camera.main == null) {
				return;
			}
			Vector3 sp = Camera.main.WorldToScreenPoint (transform.position);
			pos = sp;
			//Debug.Log("blood pos "+sp);
			pos.y = Screen.height - pos.y - 25;
			pos.x -= size.x / 2;

			GUI.skin.box.normal.background = progressBarEmpty;

			//GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), progressBarFull);

			GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
			GUI.Box (new Rect (0, 0, size.x, size.y), progressBarEmpty);

			curValue = Mathf.Lerp (curValue, barDisplay, 5 * Time.deltaTime);
			GUI.DrawTexture (new Rect (0, 0, size.x * curValue, size.y), progressBarFull);
	
			GUI.EndGroup ();
				

		}

		void SetBarDisplay (float v)
		{
			barDisplay = v;
		}

		// Update is called once per frame
		void Update ()
		{
		}
	}

}