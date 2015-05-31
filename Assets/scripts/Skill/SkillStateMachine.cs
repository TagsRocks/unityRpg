using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    /// <summary>
    /// 限制所有的事件只处理一次
    /// OnHit之后不再处理OnHit 或者状态一直维持
    /// </summary>
	public class SkillStateMachine : MonoBehaviour
	{
		float lifeTime = 5;
		List<GameObject> allRunners = new List<GameObject> ();
		public bool isStop = false;

        public Vector3 InitPos = Vector3.zero;
		public GameObject attacker {
			get;
			set;
		}

		public GameObject target {
			get;
			set;
		}

		public SkillFullInfo skillFullData {
			get;
			set;
		}

		public SkillDataConfig skillDataConfig {
			get;
			set;
		}

		public int ownerLocalId = -1;
		static List<MyEvent.EventType> regEvt = new List<MyEvent.EventType> (){
			MyEvent.EventType.EventTrigger,
			MyEvent.EventType.EventMissileDie,
		};

		public void DoDamage (GameObject g)
		{
			SkillDamageCaculate.DoDamage (attacker, skillFullData, g);
		}

		void RegEvent ()
		{
            Log.AI("regevent is "+regEvt.Count);
			foreach (MyEvent.EventType e in regEvt) {
				MyEventSystem.myEventSystem.RegisterLocalEvent (ownerLocalId, e, OnEvent);
			}
		}

		void UnRegEvent ()
		{
			foreach (MyEvent.EventType e in regEvt) {
				MyEventSystem.myEventSystem.DropLocalListener (ownerLocalId, e, OnEvent);
			}
		}

        void UnRegEvent(MyEvent.EventType evt) {
            MyEventSystem.myEventSystem.DropLocalListener (ownerLocalId, evt, OnEvent);
        }

		void InitLayout (SkillDataConfig.EventItem item, MyEvent evt)
		{
            if (item.layout != null)
            {
                var g = Instantiate(item.layout) as GameObject;
                g.transform.parent = transform;

                //陷阱粒子效果 位置是 当前missile爆炸的位置
				//瞬间调整SkillLayout的方向为 攻击者的正方向
                g.transform.localPosition = InitPos;
                var y = attacker.transform.localRotation.eulerAngles.y;
                g.transform.localRotation = Quaternion.Euler(new Vector3(0, y, 0));
                g.transform.localScale = Vector3.one;
			
			
				var runner = g.AddComponent<SkillLayoutRunner>();
                runner.stateMachine = this;
                runner.Event = item;
                allRunners.Add(g);
                Log.AI ("SkillLayout " + item.layout.name);
            } else if (item.childSkillId != 0 && item.childSkillId != -1)
            {
                Log.AI("Create Child Skill "+item.childSkillId);
                SkillLogic.CreateSkillStateMachine(attacker, Util.GetSkillData(item.childSkillId, 1), evt.missile.position);
            }
			
		}

		void OnHit ()
		{
			Log.AI ("Show Skill Hit Event " + gameObject.name);
			if (skillDataConfig != null) {
				foreach (SkillDataConfig.EventItem item in skillDataConfig.eventList) {
					if (item.evt == MyEvent.EventType.EventTrigger) {
						InitLayout (item, null);
					}
				}
			}
            UnRegEvent(MyEvent.EventType.EventTrigger);
		}

        void OnMissileDie(MyEvent evt)
        {
            Log.AI("Missile Die Receive");
            foreach (SkillDataConfig.EventItem item in skillDataConfig.eventList)
            {
                if(item.evt == MyEvent.EventType.EventMissileDie) {

                    InitLayout(item, evt);
                }
            }
            UnRegEvent(MyEvent.EventType.EventMissileDie);
        }

		void OnStart ()
		{
			if (skillDataConfig != null) {
				foreach (SkillDataConfig.EventItem item in skillDataConfig.eventList) {
					if (item.evt == MyEvent.EventType.EventStart) {
						InitLayout (item, null);
					}
				}
			}
		}

		void OnEvent (MyEvent evt)
		{
			switch (evt.type) {
			case MyEvent.EventType.EventTrigger:
				OnHit ();
				break;
			case MyEvent.EventType.EventMissileDie:
                OnMissileDie(evt);
				break;
			}
		}



		void OnDestroy ()
		{
			UnRegEvent ();
		}

		void Start ()
		{
			RegEvent ();
			StartCoroutine (FinishSkill ());
			OnStart ();
		}


		IEnumerator FinishSkill ()
		{
			Log.AI ("Finish Skill Here " + gameObject.name);
			yield return new WaitForSeconds (lifeTime);
			foreach (GameObject r in allRunners) {
				GameObject.Destroy (r);
			}
			GameObject.Destroy (gameObject);
		}
		//玩家连击伤害的时候，一个动作结束则 终止这个动作的技能状态机
		public void Stop ()
		{
			isStop = true;
			MyEventSystem.myEventSystem.PushLocalEvent (attacker.GetComponent<NpcAttribute>().GetLocalId(), MyEvent.EventType.HideWeaponTrail);
		}
	}
}