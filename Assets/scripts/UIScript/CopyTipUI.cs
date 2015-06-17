using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class CopyTipUI : IUserInterface
	{
		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.OpenCopyTip,
			};
			RegEvent ();

			SetCallback ("Enterinto Button", OnEnter);
			SetCallback ("Sweep Button", OnEnter);
		}

		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}

		void UpdateFrame() {
			GetLabel ("title").text = CopyController.copyController.SelectLevelInfo.levelLocal.name;
			GetLabel ("fight_ability").text = CopyController.copyController.SelectLevelInfo.levelLocal.fight_ability.ToString();
			GetLabel ("level").text = CopyController.copyController.SelectLevelInfo.levelLocal.levelLimit.ToString();
			GetLabel ("power").text = CopyController.copyController.SelectLevelInfo.levelLocal.power.ToString();


		}

		//点击进入副本按钮
		void OnEnter(GameObject g) {
            //进入场景切换流程逻辑
			//WorldManager.worldManager.StartCoroutine (WorldManager.worldManager.ChangeScene(CopyController.copyController.SelectLevelInfo.levelLocal.id, false));
            WorldManager.worldManager.WorldChangeScene(CopyController.copyController.SelectLevelInfo.levelLocal.id, false);

		}

		void OnSweep(GameObject g) {
		}
	}
}
