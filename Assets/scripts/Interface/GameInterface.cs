
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class GameInterface 
	{
		public static GameInterface gameInterface = new GameInterface();
	
		public void PlayerAttack() {
			var cmd = new ObjectCommand (ObjectCommand.ENUM_OBJECT_COMMAND.OC_USE_SKILL);
			cmd.skillId = ObjectManager.objectManager.GetMyPlayer ().GetComponent<SkillInfoComponent> ().GetDefaultSkillId ();
			Log.GUI ("Player Attack LogicCommand");
			ObjectManager.objectManager.GetMyPlayer ().GetComponent<LogicCommand> ().PushCommand (cmd);

            var cg = CGPlayerCmd.CreateBuilder();
            var skInfo = SkillAction.CreateBuilder();
            skInfo.Who = ObjectManager.objectManager.GetMyServerID(); 
            skInfo.SkillId = cmd.skillId;
            cg.SkillAction = skInfo.Build();
            cg.Cmd = "Skill";
            WorldManager.worldManager.GetActive().BroadcastMsg(cg);
		}


		//将背包物品装备起来
		public void PacketItemUserEquip(BackpackData item) {
			//摆摊
			//验证使用规则
			//判断等级
			var myself = ObjectManager.objectManager.GetMyPlayer ();
			if (myself != null) {
				if( item.GetNeedLevel () != -1 && myself.GetComponent<NpcAttribute>().Level < item.GetNeedLevel()) {
					var evt = new MyEvent(MyEvent.EventType.DebugMessage);
					evt.strArg = "等级不够";
					MyEventSystem.myEventSystem.PushEvent(evt);
					return;
				}
			}

			BackPack.backpack.SetSlotItem (item);
			BackPack.backpack.StartCoroutine(BackPack.backpack.UseEquipForNetwork ());
		}

		

	}
}
