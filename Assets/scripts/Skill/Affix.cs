using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ChuMeng {
	[System.Serializable]
	public class Affix {
		//效果类型
		public enum EffectType {
			None,
			SummonDuration, //召唤物持续时间 
			DefenseAdd, //防御增加
			KnockBack, //击退怪物
		}
		public EffectType effectType = EffectType.None;
		public float Duration = 10;//Buff Time 

		public GameObject UnitTheme;//buff 期间的单位粒子效果

		//TODO:防御增加的数值 从数值表中读取
		//根据技能等级 以10%的比例上升 或者相对于某条数值曲线 上面取值
		public int addDefense = 0;

		public enum TargetType {
			Self,
			Pet,
			Enemy,
		}

		//附加buff到攻击者还是宠物身上
		public TargetType target = TargetType.Self;
	}
}
