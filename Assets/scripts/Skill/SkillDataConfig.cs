using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class SkillDataConfig : MonoBehaviour
	{
        private int EvtId = 0;
		[System.Serializable]
		public class EventItem {
			//attachOwner 是带动玩家和DamageShape一起动
			//attaches 是 将粒子系统挂在玩家身上
			public MyEvent.EventType evt;
			public GameObject layout;

			//附属效果参数
			public Affix affix;

			//DamageShape 会带着玩家一起移动  向前冲击技能需要带动玩家一起运动
			public bool attachOwner = false;
            //将粒子效果附加到玩家身上
            public bool attaches = false;

			//陷阱技能会衍生出子技能
			//public SkillDataConfig childSkill;
            public int childSkillId = -1;

			//电击爆炸出现在目标身上
			public bool AttachToTarget = false;
			//设置Beam的粒子效果的GravityWell BeamTarget 的目标为Enemy的坐标
			public bool SetBeamTarget = false;
			public Vector3 BeamOffset = new Vector3 (0, 1, 0);
            //出现在目标所在位置
            public bool TargetPos = false;
            //使用上次事件标记的位置
            public bool UseMarkPos = false;

            public int EvtId = 0; //当前技能的事件ID编号

		}
		public List<EventItem> eventList;


		//循环播放idle动画 按照攻击频率来 释放技能 火焰陷阱
        public bool animationLoop = false;
        public float attackDuration = 1;

        /// <summary>
        ///初始哈每个Event事件的ID编号 
        /// </summary>
        void Awake() {
            var num = 0;
            foreach(var e in eventList) {
                e.EvtId = num++;
            }
        }
	}
}