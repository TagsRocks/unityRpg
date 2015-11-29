
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    [RequireComponent(typeof(KBEngine.KBNetworkView))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(MyAnimationEvent))]
	[RequireComponent(typeof(BloodBar))]
	[RequireComponent(typeof(NpcAttribute))]
	[RequireComponent(typeof(CommonAI))]
	[RequireComponent(typeof(ShadowComponent))]
	[RequireComponent(typeof(CharacterInfo))]
	[RequireComponent(typeof(SkillInfoComponent))]
	[RequireComponent(typeof(PhysicComponent))]
	[RequireComponent(typeof(BuffComponent))]
	[RequireComponent(typeof(LogicCommand))]
	[RequireComponent(typeof(MoveController))]
	[RequireComponent(typeof(NpcEquipment))]
	public class AIBase : MonoBehaviour
	{
        
		protected NpcAttribute attribute;
		protected AICharacter ai;
        public AICharacter GetAI(){
            return ai;
        }

		public List<ChuMeng.MyEvent.EventType> regEvt = null;
		
		public void RegEvent ()
		{
			if (regEvt != null) {
				foreach (ChuMeng.MyEvent.EventType t in regEvt) {
					ChuMeng.MyEventSystem.myEventSystem.RegisterEvent (t, OnEvent);
				}
			}
			
		}
		void DropEvent()  {
			if (regEvt != null) {
				foreach(ChuMeng.MyEvent.EventType t in regEvt) {
					ChuMeng.MyEventSystem.myEventSystem.dropListener(t, OnEvent);
				}
			}
		}
		protected virtual void OnEvent(ChuMeng.MyEvent evt) {
		}
		
		protected virtual void OnDestroy() {
			DropEvent ();
		}
		public KBEngine.KBNetworkView photonView {
			get {
				return GetComponent<KBEngine.KBNetworkView> ();
			}
		}
		
		protected void AddEvent(ChuMeng.MyEvent.EventType t) {
			regEvt.Add (t);
			ChuMeng.MyEventSystem.myEventSystem.RegisterEvent (t, OnEvent);
		}
	}

}