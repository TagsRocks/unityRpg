using UnityEngine;
using System.Collections;

namespace ChuMeng {

	public class HumanSkill : SkillState {
		SkillStateMachine skillStateMachine;
		SkillFullInfo activeSkill;


		public override void EnterState ()
		{
			base.EnterState ();
			Log.AI ("Enter Skill State ");
			//启动技能状态机 启动动画
            activeSkill = GetAttr ().GetComponent<SkillInfoComponent> ().GetActiveSkill ();

            
            skillStateMachine = SkillLogic.CreateSkillStateMachine(GetAttr().gameObject, activeSkill.skillData, GetAttr().transform.position);
			GetAttr ().GetComponent<AnimationController> ().SetAnimationSampleRate (100);
		}
		public override void ExitState ()
		{
			GetAttr ().GetComponent<AnimationController> ().SetAnimationSampleRate (30);
			base.ExitState ();
		}

		//向当前所面朝方向进行攻击
		public override IEnumerator RunLogic ()
		{
			Log.AI ("Check Animation "+GetAttr().animation.IsPlaying(activeSkill.skillData.AnimationName));
			float passTime = 0;
            var realAttackTime = GetAttr ().ObjUnitData.AttackAniSpeed;
			var animation = GetAttr ().animation;
			var attackAniName = activeSkill.skillData.AnimationName;
            var rate = GetAttr().animation[attackAniName].length/realAttackTime;
            Log.AI("AttackAniSpeed "+rate+" realAttackTime "+realAttackTime);
            PlayAni (activeSkill.skillData.AnimationName, rate, WrapMode.Once);

			while(!quit) {

				if(CheckEvent()) {
					yield break;
				}
				if(passTime >= animation[attackAniName].length*0.8f/animation[attackAniName].speed) {
					break;
				}
				passTime += Time.deltaTime;
				yield return null;
			}
			Log.AI ("Stop SkillState ");
			skillStateMachine.Stop();
			aiCharacter.ChangeState (AIStateEnum.IDLE);
		}
	}
}
