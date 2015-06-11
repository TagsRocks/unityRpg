
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class WindowMng
	{
		//Resource UI 目录下面
		//Name ---> UI/Name
		public enum WindowName
		{
		}
		static WindowMng wm = null;
		public static WindowMng windowMng {
			get{
				if(wm == null) {
					wm = new WindowMng();
				}
				return wm;
			}
		}

		GameObject background = null;
		GameObject alphaBlock; 


		GameObject uiRoot = null;
		public GameObject GetUIRoot() {
			if (uiRoot == null) {
				uiRoot = GameObject.FindGameObjectWithTag ("UIRoot");
			}
			return uiRoot;
		}
		GameObject back = null;
		List<GameObject> stack;
		List<GameObject>  alphaStack;

		Dictionary<string, GameObject> uiMap = new Dictionary<string, GameObject> ();
		public WindowMng() {
			stack = new List<GameObject> ();
			alphaStack = new List<GameObject> ();

			background = Resources.Load<GameObject> ("UI/Background");
			alphaBlock = Resources.Load<GameObject> ("UI/alphaBlock");

			MyEventSystem.myEventSystem.RegisterEvent (MyEvent.EventType.MeshShown, OnEvent);
			MyEventSystem.myEventSystem.RegisterEvent (MyEvent.EventType.MeshHide, OnEvent);

			MyEventSystem.myEventSystem.RegisterEvent (MyEvent.EventType.ChangeScene, OnEvent);
			MyEventSystem.myEventSystem.RegisterEvent (MyEvent.EventType.EnterScene, OnEvent);
		}


		void OnEvent(MyEvent evt) {
			//界面需要显示一个Mesh对象
			//TODO: FakeUI在MeshShow的时候需要初始化一下装备显示
			Log.GUI ("In Come Event window manager "+evt.type+" "+evt.intArg);
			if (evt.type == MyEvent.EventType.MeshShown) {
				FakeObjSystem.fakeObjSystem.OnUIShown (evt.intArg, evt.rolesInfo);
			} else if (evt.type == MyEvent.EventType.MeshHide) {
				FakeObjSystem.fakeObjSystem.OnUIHide (evt.intArg);
			} else if (evt.type == MyEvent.EventType.ChangeScene) {
				//切换场景需要弹出所有的UI
				//ClearView ();
			} else if (evt.type == MyEvent.EventType.EnterScene) {
				//进入场景，清理loading页面
				ClearView();
			}
		}

		void ClearView() {
			Log.GUI ("Window Manager Clear View");
			uiRoot = null;
			stack.Clear ();
			alphaStack.Clear ();
			uiMap.Clear();
			back = null;
		}
		public int GetCurUILayer() {
			return (int)UIDepth.Window + stack.Count * 2;
		}

		//替换掉当前显示的UI类似于切换场景
		public GameObject ReplaceView(string viewName, bool needAlpha = true) {
			Log.GUI ("ReplaceView");
			while (stack.Count > 0) {
				PopView();
			}

			return PushView (viewName, needAlpha);
		}


		public GameObject PushView(string viewName, bool needAlpha = true) {
			Log.Important ("Push UI View "+viewName+" "+needAlpha);
			if (uiRoot == null) {
				uiRoot = GameObject.FindGameObjectWithTag ("UIRoot");
			}
			if (needAlpha) {
				if (back == null) {
					back = NGUITools.AddChild (uiRoot, background);
				} else {
					if (stack.Count == 0) {
						back.SetActive (true);
					}
				}
			}

			GameObject bag;
			if (uiMap.TryGetValue (viewName, out bag)) {
				bag.SetActive (true);
			} else {
				bag = NGUITools.AddChild (uiRoot, Resources.Load<GameObject> (viewName));
				uiMap[viewName] = bag;
			}
			if (bag == null) {
				Debug.LogError("can't Find UI "+viewName);
			}
			if (needAlpha) {
				var alpha = NGUITools.AddChild (uiRoot, alphaBlock);
				alpha.GetComponent<UIPanel> ().depth = (int)UIDepth.Window + stack.Count * 10;
				alphaStack.Add (alpha);
			} else {
				alphaStack.Add(null);
			}

			var allPanel = Util.GetAllPanel (bag);
			int oldDepth = allPanel [0].depth;
			foreach (UIPanel p in allPanel) {
				p.depth = (int)UIDepth.Window+stack.Count*10+1+(p.depth-oldDepth);
			}

			//bag.GetComponent<UIPanel> ().depth = (int)UIDepth.Window+stack.Count*10+1;
			stack.Add (bag);
			Log.GUI ("Push UI "+bag.name);
			foreach(GameObject g in stack) {
				Log.GUI("Stack UI is "+g.name);
			}
			return bag;
		}

		public GameObject PushTopNotify(string viewName) {
			if (uiRoot == null) {
				uiRoot = GameObject.FindGameObjectWithTag ("UIRoot");
			}
			GameObject bag;
			if (uiMap.TryGetValue (viewName, out bag)) {
				bag.SetActive (true);
			} else {
				bag = NGUITools.AddChild (uiRoot, Resources.Load<GameObject> (viewName));
				uiMap[viewName] = bag;
			}
			if (bag == null) {
				Debug.LogError("can't Find UI "+viewName);
			}

			
			var allPanel = Util.GetAllPanel (bag);
			int oldDepth = allPanel [0].depth;
			foreach (UIPanel p in allPanel) {
				p.depth = (int)UIDepth.Window+100+1+(p.depth-oldDepth);
			}
			
			Log.GUI ("Push Notify UI "+bag.name);
			foreach(GameObject g in stack) {
				Log.GUI("Stack UI is "+g.name);
			}
			return bag;
		}
		public void PopView() {
			Log.Important ("UI Layer "+stack.Count+" alphaCount "+alphaStack.Count+" backactive "+back);
			var top = stack[stack.Count-1];
			stack.RemoveAt (stack.Count - 1);
			var alpha = alphaStack [alphaStack.Count - 1];
			alphaStack.RemoveAt (alphaStack.Count - 1);
			if (alpha != null) {	
				GameObject.Destroy (alpha);
			}
			top.SetActive (false);

			//除了主UI其它UI才有Back遮挡
			if (stack.Count == 1) {
				back.SetActive (false);
			}
		}

		public void ShowNotifyLog(string text, float time = 3) {
			var g = PushTopNotify ("UI/NotifyLog");
			g.GetComponent<NotifyUI> ().SetText (text);
			g.GetComponent<NotifyUI> ().SetDurationTime (time);
		}

	}

}