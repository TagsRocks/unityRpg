using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class SkillLayoutRunner : MonoBehaviour
	{
		public SkillStateMachine stateMachine {
			get;
			set;
		}

		public SkillDataConfig.EventItem Event {
			get;
			set;
		}

		public void DoDamage (GameObject g)
		{
			Log.AI ("SkillLayout DoDamage "+Event.affix.effectType);
			if (Event.affix.effectType != Affix.EffectType.None && Event.affix.target == Affix.TargetType.Enemy) {
				g.GetComponent<BuffComponent>().AddBuff(Event.affix, stateMachine.attacker);
			}
			stateMachine.DoDamage (g);
		}

		//玩家先在处于一个Skill状态 玩家先在处于一个Skill状态 技能时间1s钟 玩家才能结束状态
		public bool MoveOwner (Vector3 position, float speed)
		{
			stateMachine.attacker.GetComponent<PhysicComponent> ().SkillMove (position, speed);
			return true;
		}

		void Start ()
		{
			if (Event.attaches) {
				Log.AI ("attach Particle to player");
				transform.parent = stateMachine.attacker.transform;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
			}
			if (Event.affix.effectType != Affix.EffectType.None && Event.affix.target == Affix.TargetType.Self) {
				stateMachine.attacker.GetComponent<BuffComponent> ().AddBuff (Event.affix);
			}
			

			var skillConfig = Event.layout.GetComponent<SkillLayoutConfig> ();
			if (skillConfig != null) {
				var particle = Event.layout.GetComponent<SkillLayoutConfig> ().particle;
				//NGUITools.AddMissingComponent<RemoveSelf> (particle);
				Log.Sys ("Init Particle for skill Layout " + particle + " " + skillConfig + " bone name " + skillConfig.boneName);
				if (particle != null) {
					var g = GameObject.Instantiate (particle) as GameObject;
					var xft = g.GetComponent<XffectComponent>();
					xft.enabled = false;
					NGUITools.AddMissingComponent<RemoveSelf> (g);

					if (skillConfig.boneName != "") {
						Log.Sys ("add particle to bone " + skillConfig.boneName);
						//g.transform.parent = transform;
						var par = Util.FindChildRecursive(stateMachine.attacker.transform, skillConfig.boneName);
                        if(par == null) {
                            par = stateMachine.attacker.transform;
                        }

						//g.transform.parent =  
						g.transform.localPosition = skillConfig.Position+par.transform.position;
						g.transform.localRotation = Quaternion.identity;
						g.transform.localScale = Vector3.one;

					} else {
						if (Event.AttachToTarget) {
							if (stateMachine.target != null) {
								g.transform.parent = stateMachine.target.transform;
								g.transform.localPosition = skillConfig.Position;
								g.transform.localRotation = Quaternion.identity;
								g.transform.localScale = Vector3.one;
							}
						} else {
							g.transform.parent = transform;
							g.transform.localPosition = skillConfig.Position;
							g.transform.localRotation = Quaternion.identity;
							g.transform.localScale = Vector3.one;
						}
					}

					//火焰哨兵的激光直接给目标造成伤害
					if(Event.SetBeamTarget ) {
						Log.AI("SetBeamTarget is "+stateMachine.target.transform.position);
						var bt = Util.FindChildrecursive<BeamTarget>(g.transform);
						bt.transform.position = stateMachine.target.transform.position+Event.BeamOffset;
						//DoDamage(stateMachine.target);
					}

					StartCoroutine(EnableXft(xft));
				}
			}
		}
		IEnumerator EnableXft(XffectComponent xft) {
			yield return null;
			xft.enabled = true;
		}
	}
}
