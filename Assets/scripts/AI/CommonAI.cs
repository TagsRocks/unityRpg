
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

namespace ChuMeng {
	/// <summary>
	/// 通用逻辑需要的 数据池 和 相关逻辑代码
    /// Billboard
	/// </summary>
	public class CommonAI : MonoBehaviour {
		NpcAttribute attribute;
		//CharacterController controller;

		//float moveSpeed = 0;
		MyAnimationEvent myAnimationEvent;
		//GameObject targetObj;

		/*
		 * 攻击操作的对象
		 */ 
		GameObject targetPlayer = null;
		public void SetTargetPlayer(GameObject g) {
			targetPlayer = g;
		}

		void Awake() {
			attribute = GetComponent<NpcAttribute> ();
			//controller = GetComponent<CharacterController> ();
			myAnimationEvent = GetComponent<MyAnimationEvent> ();
			targetPlayer = null;
		}

		//TODO:: Add Move To Pos For CutScene AI
		public IEnumerator MoveToPos(Vector3 target) {
			/*
			attribute.SetRun ();

			while(true) {
				Vector3 dir = target-transform.position;
				dir.y = 0;

				var rotation = Quaternion.LookRotation(dir);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Min(1, Time.deltaTime*attribute.FastRotateSpeed));
				break;
			}

			while (true) {
				Vector3 dir = target-transform.position;
				dir.y = 0;
				if(dir.magnitude < attribute.ReachRange) {
					break;
				}

				var rotation = Quaternion.LookRotation(dir);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Min(1, Time.deltaTime*attribute.FastRotateSpeed));

				var forward = transform.TransformDirection(Vector3.forward);
				moveSpeed = Mathf.Lerp(moveSpeed, attribute.WalkSpeed, Time.deltaTime*5);
				controller.SimpleMove(forward*moveSpeed);

				yield return null;
			}
			*/
			yield return null;
		}
		/// <summary>
		/// 剧情动画杀死一个npc 
		/// </summary>
		public IEnumerator KillNpc(GameObject target) {

			string attAniName = "rslash_1";
			var targetAttribute = target.GetComponent<NpcAttribute> ();
			//targetObj = target;

			while (!targetAttribute.IsDead) {
				animation.CrossFade (attAniName);
				animation [attAniName].speed = 2;
				animation [attAniName].wrapMode = WrapMode.Once;


				while(animation.isPlaying) {
					if(myAnimationEvent.hit) {
						myAnimationEvent.hit = false;
						DoHit();
					}
					Vector3 dir = target.transform.position-transform.position;
					dir.y = 0;
					var rotation = Quaternion.LookRotation(dir);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*attribute.FastRotateSpeed);
					yield return null;
				}
			}

		}

		void DoHit() {
			//targetObj.GetComponent<MyAnimationEvent>().OnHit(gameObject, attribute.Damage);
		}

		void DoHit(float wd) {
			var ene = targetPlayer;
			if(ene != null) {
				float dist = (ene.transform.position-transform.position).magnitude;
				if(dist < attribute.AttackRange && ene.GetComponent<MyAnimationEvent>() != null) {
					//ene.GetComponent<MyAnimationEvent>().OnHit(gameObject, (int)(attribute.Damage*wd/100));
				}
			}
		}

		void DoDamage(float wd, GameObject enemy) {
			if (enemy.GetComponent<MyAnimationEvent> () != null) {
				//enemy.GetComponent<MyAnimationEvent> ().OnHit (gameObject, (int)(attribute.Damage * wd / 100));
			} else if (enemy.GetComponent<ItemDataRef> () != null) {
				enemy.GetComponent<ItemDataRef>().Break();
			}
		}

		///<summary>
		/// 怪物死亡的时候会触发一种技能 爆炸伤害附近的人
		/// </summary>
		public IEnumerator ShowDeadSkill(SkillData sd) {
			yield return null;
			/*
			if (sd.deadParticle != "") {
				var par = Instantiate(Resources.Load(sd.deadParticle)) as GameObject;
				par.transform.position = transform.position+new Vector3(0, 0.5f, 0);

				HashSet<GameObject> hurtEnemy = new HashSet<GameObject> ();

				//Around 360 
				if(sd.damageShape == SkillData.DamageShape.Circle) {
					Collider[] hitColliders = Physics.OverlapSphere(transform.position, sd.DamageShapeRadius);
					for(int i =0; i < hitColliders.Length; i++) {

						if(hitColliders[i].tag == "Enermy" || hitColliders[i].tag == "Barrel") {
							if(!hurtEnemy.Contains(hitColliders[i].gameObject)) {
								DoDamage(sd.WeaponDamagePCT, hitColliders[i].gameObject);
								hurtEnemy.Add(hitColliders[i].gameObject);
							}
						}
					}
				}

				yield return null;
			}else {
				Debug.LogError("CommonAI::ShowDeadSkill no particle for skill "+sd);
			}
			*/
		}

		/// <summary>
		/// 怪物释放一个技能
		/// </summary>
		public IEnumerator CastSkill(SkillData sd) {
			var attackAniName = sd.AnimationName;
			animation.CrossFade(attackAniName);
			animation[attackAniName].speed = 2;

			Debug.Log ("CommonAI::CastSkill "+gameObject.name);

			while(animation.isPlaying) {
				if(attribute.CheckDead())
					break;
				
				if(myAnimationEvent.hit) {
					myAnimationEvent.hit = false;
					/*
					 * Cast Missile
					 */ 
					/*
					if (sd.Missile != null) {
						GameObject bullet = new GameObject();
						var bc = bullet.AddComponent<BulletSpawn>();
						bc.skillData = sd;
						bc.attacker = gameObject;
						//bc.enemyTag = "Player";
					}else {
						DoHit(sd.WeaponDamagePCT);

						if(sd.KnockBackEffect) {
							targetPlayer.GetComponent<MyAnimationEvent>().KnockBackWho(gameObject);
						}

					}
					*/
				}
				/*
				if(sd.TurnToTarget) {
					Vector3 dir = targetPlayer.transform.position-transform.position;
					dir.y = 0;
					var rotation = Quaternion.LookRotation(dir);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*attribute.FastRotateSpeed);
				}
				*/

				yield return null;
			}
		}



	}
}


