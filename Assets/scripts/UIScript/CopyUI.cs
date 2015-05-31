using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class CopyUI : IUserInterface
	{
		UILabel title;
		GameObject left;
		GameObject right;
		int curChapter = -1;
		List<GameObject> levels;
		int MaxLevel = 8;
		List<LevelInfo> allLevels;

		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.OpenCopyUI,
				MyEvent.EventType.UpdateCopy,
			};
			RegEvent ();

			SetCallback ("closeButton", Hide);
			left = GetName ("left");
			right = GetName("right");

			SetCallback ("left", OnLeft);
			SetCallback ("right", OnRight);
			title = GetLabel ("Title");

			levels = new List<GameObject> ();
			for (int i =1; i <= 8; i++) {
				levels.Add(GetName("Level"+i));
				SetCallback("Level"+i, OnLevel);
			}
		}

		void OnLevel(GameObject g) {
			var levId =  System.Convert.ToInt32(g.newName.Replace ("Level", ""));
			CopyController.copyController.SelectLevel (curChapter, allLevels[levId-1]);

			WindowMng.windowMng.PushView ("UI/CopyTips");	
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OpenCopyTip);
		}

		void OnLeft(GameObject g) {
			curChapter--;
			curChapter = Mathf.Max (1, curChapter);
			UpdateFrame ();
		}

		void OnRight(GameObject g) {
			curChapter++;
			curChapter = Mathf.Min (CopyController.copyController.GetMaxChapterId (), curChapter);
			UpdateFrame ();
		}

		protected override void OnEvent (MyEvent evt)
		{
			Log.GUI ("Update CopyController Update Gui ");
			UpdateFrame ();
		}

		void UpdateFrame() {
			if (!CopyController.copyController.InitYet) {
				return;
			}

			Log.Important ("Init copy UI "+curChapter);
			if (curChapter == -1) {
				curChapter = CopyController.copyController.GetCurrentChapter();
			}
			if (curChapter == -1) {
				//StartCoroutine(WaitForChapter());
				return;
			}

			title.text = CopyController.copyController.ChapterName (curChapter);
			if (curChapter == 1) {
				left.SetActive (false);
			} else {
				left.SetActive(true);
			}

			if (curChapter == CopyController.copyController.GetMaxChapterId ()) {
				right.SetActive (false);
			} else {
				right.SetActive(true);
			}

			allLevels = CopyController.copyController.GetChapterLevel (curChapter);
			//章节关卡上限
			//最后一个未通关的关卡是开放的 玩家可以进入
			bool lastUnPass = true;
			Log.Important ("Level Count "+levels.Count+ " "+allLevels.Count);
			for (int i = 0; i < levels.Count; i++) {
				if(i < allLevels.Count) {
					if(allLevels[i].levelServer.IsPass) {
						levels[i].SetActive(true);
					}else {
						if(lastUnPass) {
							Log.Important("last UnPass Level is "+i);
							lastUnPass = false;
							levels[i].SetActive(true);
						}else {
							levels[i].SetActive(false);
						}
					}
				}else {
					Log.Important("Set Unpass Level "+levels[i]);
					levels[i].SetActive(false);
				}
			}



		}
	}
}
