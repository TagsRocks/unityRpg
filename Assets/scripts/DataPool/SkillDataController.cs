
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
using System.Collections.Generic;
using System;
using SimpleJSON;

/// <summary>
/// 初始化技能列表 CActionItem_Skill 管理这些技能
/// </summary>
namespace ChuMeng {
	public class SkillDataController : MonoBehaviour {
		public static SkillDataController skillDataController;

		//SkillPanel skillPanelCom;
		GameObject uiRoot;
		GameObject skillPanel;

		/*
		[System.Serializable]
		public class SkillSlot {
			public SkillData skillData;
			public int Level;
		}
		*/

		//右下角快捷栏 里面的 技能  还包括 使用药品的技能
		//TODO: 初始化结束之后 玩家 SkillinfoComponent 从这里获取快捷栏里面的技能  不包括普通攻击技能  普通技能的ID 根据单位的BaseSkill 确定
		//防御和闪避 也是固定的
		List<SkillFullInfo> skillSlots = new List<SkillFullInfo>();

		//CharacterData charData;
		int distributedSkillPoint;
		int totalSkillPoint;
		public int TotalSp {
			get {
				return totalSkillPoint;
			}
		}
		public int DistriSp {
			get {
				return distributedSkillPoint;
			}
		}

		//TODO:增加技能状态 变化接口
		//public VoidDelegate UpdateSkill;

		List<SkillFullInfo> activeSkill = new List<SkillFullInfo>();
		List<SkillFullInfo> passiveSkill = new List<SkillFullInfo>();
		public List<SkillFullInfo> activeSkillData {
			get {
				return activeSkill;
			}
		}
		public List<SkillFullInfo> passive {
			get {
				return passiveSkill;
			}
		}

		void LevelDown(int skId) {
			foreach (SkillFullInfo sk in activeSkill) {
				if(sk.skillId == skId) {
					sk.skillData = Util.GetSkillData(sk.skillId, sk.level-1);
					return;
				}
			}

			foreach (SkillFullInfo sk in passiveSkill) {
				if(sk.skillId == skId) {
					sk.skillData = Util.GetSkillData(sk.skillId, sk.level-1);
					return;
				}
			}
		}

	
		public IEnumerator DownLevelSkill(int skillId) {
			var packet = new KBEngine.PacketHolder ();
			var levelDown = CGSkillLevelDown.CreateBuilder ();
			levelDown.SkillId = skillId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, levelDown, packet));
			//skillList.LevelUpWithProps (packet.packet.protoBody as GCInjectPropsLevelUp);
			LevelDown (skillId);
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateSkill);
		}
		public SkillData GetShortSkillData(int index) {
			foreach (SkillFullInfo s in skillSlots) {
				if(s.shortSlotId == index) {
					return s.skillData;
				}
			}
			return null;
		}

		void SetShortSkillData(int skId, int index) {
			foreach (SkillFullInfo s in skillSlots) {
				if(s.skillId == skId) {
					skillSlots.Remove(s);
					break;
				}
			}
			foreach (SkillFullInfo s in skillSlots) {
				if(s.shortSlotId == index) {
					skillSlots.Remove(s);
					break;
				}
			}
			var full = new SkillFullInfo (skId, index);
			skillSlots.Add (full);
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateShortCut);
		}

		public IEnumerator SetSkillShortCut(int skId, int index) {
			var packet = new KBEngine.PacketHolder ();
			var b = CGModifyShortcutsInfo.CreateBuilder ();
			b.IdAdd = true;
			var shortInfo = ShortCutInfo.CreateBuilder ();
			shortInfo.Index = index;
			shortInfo.IndexId = index;
			shortInfo.BaseId = skId;
			shortInfo.Type = 0;
			b.SetShortCutInfo (shortInfo);

			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, b, packet));
			var ret = packet.packet.protoBody as GCModifyShortcutsInfo;
			SetShortSkillData (skId, index);
		}

		public IEnumerator InitFromNetwork() {
			Log.Net ("Init Skill slots");
			CGLoadShortcutsInfo.Builder b = CGLoadShortcutsInfo.CreateBuilder ();
			KBEngine.PacketHolder p = new KBEngine.PacketHolder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, b, p));
			var shortData = p.packet.protoBody as GCLoadShortcutsInfo;
			skillSlots = new List<SkillFullInfo> ();
			foreach (ShortCutInfo s in shortData.ShortCutInfoList) {
				var full = new SkillFullInfo(s);
				skillSlots.Add(full);
			}
			Log.Net ("Init Skill slots over "+skillSlots.Count);
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateShortCut);

			/*
			//获取特定技能面板的技能点 所有面板公用这些技能点
			CGLoadSkillPanel.Builder ls = CGLoadSkillPanel.CreateBuilder ();
			ls.SkillSort = SkillSort.ACTIVE_SKILL;
			KBEngine.PacketHolder p2 = new KBEngine.PacketHolder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, ls, p2));

			var data = p2.packet.protoBody as GCLoadSkillPanel;
			distributedSkillPoint = data.Distributed;
			totalSkillPoint = data.TotalPoint;

			foreach (SkillInfo sk in data.SkillInfosList) {
				activeSkill.Add(new SkillFullInfo(sk));
			}

			Log.Sys ("Init Active Skill "+activeSkill.Count);

			CGLoadSkillPanel.Builder ls2 = CGLoadSkillPanel.CreateBuilder ();
			ls2.SkillSort = SkillSort.PASV_SKILL;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, ls2, p2));
			data = p2.packet.protoBody as GCLoadSkillPanel;

			foreach (SkillInfo sk in data.SkillInfosList) {
				passiveSkill.Add(new SkillFullInfo(sk));
			}
			Log.Sys ("Init Passive Skill "+passiveSkill.Count);
			*/

		}




		void Awake() {
			skillDataController = this;
			DontDestroyOnLoad (gameObject);
	
		}


		//TODO: 学习一个新技能 
		void OnLearn(GameObject g) {
			if (totalSkillPoint - distributedSkillPoint > 0) {
				
			}
			/*
			if (charData.SkillPoint > 0) {
				var sk = GetSkill(g.name);
				if(sk != null) {
					if(charData.Level >= sk.skillData.LevelRequired) {
						sk.Level += 1;
						charData.SkillPoint--;
						if(UpdateSkill != null) {
							UpdateSkill(null);
						}
					}
				}
			}
			*/
		}


		//TODO: 添加技能学习 或者 降级的花费
		void LevelUpSkill(int skId) {
			foreach (SkillFullInfo sk in activeSkill) {
				if(sk.skillId == skId) {
					sk.skillData = Util.GetSkillData(skId, sk.level+1);
					break;
				}
			}
			foreach (SkillFullInfo sk in passiveSkill) {
				if(sk.skillId == skId) {
					sk.skillData = Util.GetSkillData(skId, sk.level+1);
					break;
				}
			}

		}
		/*
		 * 需要道具升级技能
		 * TODO:Push SP点数更新
		 */ 
		public IEnumerator SkillLevelUpWithSp(int skillId) {
			var packet = new KBEngine.PacketHolder ();
			var levelUp = CGSkillLevelUp.CreateBuilder ();
			levelUp.SkillId = skillId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, levelUp, packet));
			//skillList.LevelUpWithProps (packet.packet.protoBody as GCInjectPropsLevelUp);
			LevelUpSkill (skillId);
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateSkill);
		}


		/*
		 * 推送角色技能CD时间
		 * ObjectManager Player Skill Update
		 */ 
		public void UpdateCoolDown(GCPushMemberSkillCD coolDown) {
			
		}


		public void ActivateSkill(GCPushActivateSkill skill) {
		}

	}

}