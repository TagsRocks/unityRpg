﻿using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class MonsterIdle : IdleState {
		bool birthYet = false;
		float directionChangeInterval = 3;
		float RunSpeed = 5;
		
		//一次性初始化代码
		public override void EnterState() {
			base.EnterState ();
			SetAttrState(CharacterState.Idle);
			aiCharacter.SetIdle ();
		}
		
		IEnumerator Birth() {
			if (CheckAni ("spawn")) {
				SetAttrState (CharacterState.Birth);
				
				PlayAni ("spawn", 2, WrapMode.Once);
				
				if (GetAttr ().ObjUnitData.SpawnEffect != "") {
					GameObject g = GameObject.Instantiate (Resources.Load<GameObject> (GetAttr ().ObjUnitData.SpawnEffect)) as GameObject;
                    g.transform.parent = SaveGame.saveGame.EffectMainNode.transform;
                    g.transform.position = GetAttr ().transform.position;
					NGUITools.AddMissingComponent<RemoveSelf>(g);
				}
				yield return GetAttr ().StartCoroutine (Util.WaitForAnimation (GetAttr ().animation));
				
				SetAttrState (CharacterState.Idle);
				SetAni ("idle", 1, WrapMode.Loop);
			} else {
				SetAni("idle", 1, WrapMode.Once);
				GameObject g = null;
				if(GetAttr().ObjUnitData.SpawnEffect != "") {
					g = GameObject.Instantiate (Resources.Load<GameObject> (GetAttr ().ObjUnitData.SpawnEffect)) as GameObject;
				}else {
					g = GameObject.Instantiate(Resources.Load<GameObject>("particles/playerskills/impsummon")) as GameObject;
				}
                g.transform.parent = SaveGame.saveGame.EffectMainNode.transform;
				g.transform.position = GetAttr ().transform.position;
				NGUITools.AddMissingComponent<RemoveSelf>(g);
				yield return GetAttr ().StartCoroutine (Util.WaitForAnimation (GetAttr ().animation));

				SetAttrState (CharacterState.Idle);
				SetAni ("idle", 1, WrapMode.Loop);
			}
			birthYet = true;
            var rd = Random.Range(1, 3);
            BackgroundSound.Instance.PlayEffect("batmanspawn"+rd);
		}
		
		public override IEnumerator RunLogic ()
		{
			if (!birthYet) {
				yield return GetAttr ().StartCoroutine (Birth ());
			}
			yield return GetAttr().StartCoroutine(NewHeading());
			
			Log.AI ("State Logic Over "+type);
		}
		
		bool CheckTarget() {
			if (CheckEvent ()) {
				return true;
			}

			GameObject player = ObjectManager.objectManager.GetMyPlayer();
			if (player && !player.GetComponent<NpcAttribute> ().IsDead) {
				float distance = (player.transform.position - GetAttr ().transform.position).magnitude;
				if (distance < GetAttr ().ApproachDistance) {
					aiCharacter.ChangeState (AIStateEnum.COMBAT);
					return true;
				}
			}
			return false;
		}
		
		IEnumerator NewHeadingRoutine() {
			while(!quit) {
				var heading = Random.Range(0, 360);
				var targetRotation = new Vector3(0, heading, 0);
				Quaternion qua = Quaternion.Euler(targetRotation);
				Vector3 dir = (qua*Vector3.forward);
				
				RaycastHit hitInfo;
				if(!Physics.Raycast(GetAttr().transform.position, dir, out hitInfo, 3)) {
					break;
				}
				yield return null;
			}
		}
		
		IEnumerator NewHeading() {
			while(!quit) {
				aiCharacter.SetIdle();
				float passTime = Random.Range(1, 3);
				while(passTime > 0) {
					if(CheckTarget()) {
						yield break;
					}
					passTime -= Time.deltaTime;
					yield return null;
				}
				yield return GetAttr().StartCoroutine(NewHeadingRoutine());


				aiCharacter.SetRun();
				passTime = directionChangeInterval;
				while(passTime > 0) {
					if(CheckTarget()) {
						yield break;
					}
					passTime -= Time.deltaTime;
					
					var forward = GetAttr().transform.TransformDirection(Vector3.forward);

					GetAttr().GetComponent<PhysicComponent>().MoveSpeed(forward*RunSpeed);
					yield return null;
				}
				
				yield return null;
			}
		}
		
	}

}