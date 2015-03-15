using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class GameInterface_Tips
	{
		public static GameInterface_Tips tipsInterface = new GameInterface_Tips();
		public void EnterScene() {
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OkEnter);
		}
	}

}