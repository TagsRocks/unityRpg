
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
	
		public void PlayerUseSkill(int skillId) {
			
		}

		public void PlayerAttack() {
			var cmd = new ObjectCommand (ObjectCommand.ENUM_OBJECT_COMMAND.OC_USE_SKILL);
			cmd.skillId = ObjectManager.objectManager.GetMyPlayer ().GetComponent<SkillInfoComponent> ().GetDefaultSkillId ();
			Log.GUI ("Player Attack LogicCommand");
			ObjectManager.objectManager.GetMyPlayer ().GetComponent<LogicCommand> ().PushCommand (cmd);
			//ObjectManager.objectManager.GetMyPlayer ().GetComponent<MyAnimationEvent> ().InsertMsg (new MyAnimationEvent.Message(MyAnimationEvent.MsgType.DoAttack));
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

		//获取特定的装备槽的Item
		public ActionItem EnumAction(ItemData.EquipPosition type) {
			return ActionSystem.actionSystem.EnumAction (type);
		}

	}
}
