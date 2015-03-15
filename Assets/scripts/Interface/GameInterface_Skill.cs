using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class GameInterface_Skill
	{
		public static GameInterface_Skill skillInterface = new GameInterface_Skill();
		public List<SkillFullInfo> GetActiveSkill() {
			return SkillDataController.skillDataController.active;
		}

		public List<SkillFullInfo> GetPassitiveSkill() {
			return SkillDataController.skillDataController.passive;
		}

		public void SkillLevelUp(int skId) {
			SkillDataController.skillDataController.StartCoroutine (SkillDataController.skillDataController.SkillLevelUpWithSp(skId));
		}

		public void ResetSkill(int skId) {
			SkillDataController.skillDataController.StartCoroutine(SkillDataController.skillDataController.DownLevelSkill(skId));
		}

		public void SetSkillShortCut(int skId, int index) {
			SkillDataController.skillDataController.StartCoroutine(SkillDataController.skillDataController.SetSkillShortCut (skId, index));
		}

		public SkillData GetSkillData(int skillId, int level) {
			return Util.GetSkillData (skillId, level);
		}

		public SkillData GetShortSkillData(int shortId) {
			return SkillDataController.skillDataController.GetShortSkillData (shortId);
		}

		public string GetSkillDesc(SkillData sk) {
			var str = sk.SkillDes+"\n";
			str += string.Format("额外增加{0}武器伤害", sk.WeaponDamagePCT);
			//其它效果
			return str;
		}
		public int GetLeftSp() {
			return SkillDataController.skillDataController.TotalSp - SkillDataController.skillDataController.DistriSp;
		}

		public int DistriSp() {
			return SkillDataController.skillDataController.DistriSp;
		}

		public static void OnSkill (int skIndex)
		{
			ObjectManager.objectManager.GetMyPlayer ().GetComponent<MyAnimationEvent> ().OnSkill(SkillDataController.skillDataController.GetShortSkillData (skIndex));
		}
	}
}
