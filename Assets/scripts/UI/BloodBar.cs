
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

		
		float curValue = 0;
        GameObject bar;
        UISlider fill;
		void Awake() {
			
            bar = GameObject.Instantiate(Resources.Load<GameObject>("UI/BloodBar")) as GameObject;
            bar.transform.parent = WindowMng.windowMng.GetUIRoot().transform;
            Util.InitGameObject(bar);
            fill = bar.GetComponent<UISlider>();
		}
		// Use this for initialization
		void Start ()
		{
            Log.GUI("BloodBar Start Event");
            regLocalEvt = new System.Collections.Generic.List<MyEvent.EventType> (){
                MyEvent.EventType.UnitHP,
            };
            RegEvent (true); 

            GetComponent<NpcAttribute>().ChangeHP(0);
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

            /*
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
			*/	

		}

		void SetBarDisplay (float v)
		{
			barDisplay = v;
		}
        void OnDestroy(){
            barDisplay = 0;
            fill.value = 0;
            GameObject.Destroy(bar);
            //GameObject.Destroy(bar, 0.1f);
        }
		// Update is called once per frame
		void Update ()
		{
            if (Camera.main == null) {
                return;
            }
            Vector3 sp = Camera.main.WorldToScreenPoint (transform.position+new Vector3(0, 2.5f, 0));
            var uiWorldPos = UICamera.mainCamera.ScreenToWorldPoint(sp);
            uiWorldPos.z = 0;
            bar.transform.position = uiWorldPos;
            fill.value = barDisplay;
		}
	}

}