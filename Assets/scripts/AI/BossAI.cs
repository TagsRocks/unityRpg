
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

namespace ChuMeng
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(MyAnimationEvent))]
	[RequireComponent(typeof(BloodBar))]
	[RequireComponent(typeof(NpcAttribute))]
	[RequireComponent(typeof(Seeker))]
	[RequireComponent(typeof(ShadowComponent))]

	public class BossAI : MonoBehaviour
	{
		public float ApproachDistance = 20;

		//bool inStunned = false;

		public float WalkSpeed = 2f;
		public float RunSpeed = 5;
		public float FastRotateSpeed = 10;

		public float directionChangeInterval = 1;
		public float maxHeadingChange = 30;
		MyAnimationEvent myAnimationEvent;

		//CharacterController controller;
		float heading;
		Vector3 targetRotation;

		GameObject targetPlayer;
		//GameObject bloodBomb;
		//GameObject swordHit;
		GameObject deathBlood;
		
		NpcAttribute attribute;
		BloodBar bloodBar;
		CollisionFlags collisionFlags;

		Seeker seeker;
		bool findPath = false;
		//Pathfinding.Path path;

		string attackAniName;

		void Awake() {
            /*
			bloodBomb = Resources.Load<GameObject> ("particles/monsters/player/bloodHit");
			swordHit = Resources.Load<GameObject> ("particles/swordhit");
            */
            deathBlood = Resources.Load<GameObject> ("particles/swordhit");
			seeker = GetComponent<Seeker>();

			attribute = GetComponent<NpcAttribute>();
			bloodBar = GetComponent<BloodBar>();


			myAnimationEvent = GetComponent<MyAnimationEvent>();
			attribute._characterState = CharacterState.Idle;
			//controller = GetComponent<CharacterController>();
			heading = Random.Range(0, 360);
			transform.eulerAngles = new Vector3(0, heading, 0);

			animation.Play("idle");
			animation ["idle"].wrapMode = WrapMode.Loop;
			//GetComponent<ShadowComponent>().CreateShadowPlane();

		}
		void OnEnable() {
			seeker.pathCallback += OnPathComplete;
		}
		void OnPathComplete(Pathfinding.Path _p) {
			//Debug.Log("path is "+_p);
			//path = _p;
			findPath = true;
		}

		// Use this for initialization
		void Start ()
		{
			StartCoroutine(FindTarget());
		}

		IEnumerator WaitForFindPath() {
			while(!findPath)
				yield return null;
		}

		IEnumerator CastSkill(SkillData sd) {
			attackAniName = sd.AnimationName;
			animation.CrossFade(attackAniName);
			animation[attackAniName].speed = 2;


			Debug.Log ("start Cast skill is "+sd);
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
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*FastRotateSpeed);
				}
				*/
				yield return null;
			}
		}

		/*
		IEnumerator AroundMoveAttack() {
			attribute._characterState = CharacterState.Around;
			targetPlayer = GameObject.FindGameObjectWithTag("Player");
			while (true) {
				if(attribute.CheckDead()){
					break;
				}

				//seeker.StartPath(transform.position, tarPos);
				//yield return StartCoroutine(WaitForFindPath());

				float passTime = 0;
				animation.CrossFade("run");
				animation["run"].speed = 2f;
				animation["run"].wrapMode = WrapMode.Loop;

				//FindPath
				while(true) {
					if(attribute.CheckDead())
						break;

					Vector3 dir = targetPlayer.transform.position-transform.position;
					dir.y = 0;
					var rotation = Quaternion.LookRotation(dir);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Min(1, Time.deltaTime*FastRotateSpeed));

					if(dir.magnitude < attribute.AttackRange) {
						break;
					}
					var forward = transform.TransformDirection(Vector3.forward);
					controller.SimpleMove(forward*RunSpeed);
					yield return null;
				}
				SkillData sd = attribute.GetRandomSkill();
				Debug.Log("Cast Skill is "+sd);
				if(sd != null) {
					yield return StartCoroutine(CastSkill(sd));
				}else {
					if(animation.GetClip("attack1")) {
						attackAniName = "attack1";
					}else {
						attackAniName = "attack";
					}
					animation.CrossFade(attackAniName);
					animation[attackAniName].speed = 2;

					while(animation.isPlaying) {
						if(attribute.CheckDead())
							break;

						if(myAnimationEvent.hit) {
							myAnimationEvent.hit = false;
							DoHit(100);
						}

						Vector3 dir = targetPlayer.transform.position-transform.position;
						dir.y = 0;
						var rotation = Quaternion.LookRotation(dir);
						transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*FastRotateSpeed);


						yield return null;
					}
				}

				yield return null;
			}

			if (!attribute.IsDead) {
				attribute._characterState = CharacterState.Idle;
			}
		}
		*/

		void DoHit(float wd) {
			GameObject ene = GameObject.FindGameObjectWithTag("Player");
			if(ene != null) {
				float dist = (ene.transform.position-transform.position).magnitude;
				if(dist < attribute.AttackRange && ene.GetComponent<MyAnimationEvent>() != null) {
					//ene.GetComponent<MyAnimationEvent>().OnHit(gameObject, (int)(attribute.Damage*wd/100));
				}
			}
		}


		IEnumerator FindTarget() {
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			while (true) {
				if(attribute.CheckDead())
					break;

				if(player && (attribute._characterState == CharacterState.Idle || attribute._characterState == CharacterState.Around) ) {
					float distance = (player.transform.position-transform.position).magnitude;
					if(distance < ApproachDistance) {
						//yield return StartCoroutine(AroundMoveAttack());
					}
				}else {
				}
				yield return null;
			}
		}
	
		IEnumerator WaitForAnimation() {
			while (animation.isPlaying) {
				yield return null;
			}
		}

		IEnumerator ShowDead() {
			if(attribute.IsDead) {
				yield break;
			}
			attribute.IsDead = true;
			
			bloodBar.enabled = false;
			attribute._characterState = CharacterState.Dead;
			
			
			GameObject g = GameObject.Instantiate(deathBlood) as GameObject;
			g.transform.position = transform.position;
			
			
			if(animation.GetClip("die")) {
				animation.CrossFade("die");
				yield return StartCoroutine(WaitForAnimation());
			}

			
			yield return this.StartCoroutine(Util.SetBurn(gameObject));
			
			yield return null;
			GameObject.Destroy(gameObject);
		}

		//TODO::修复Boss的逻辑AI  使用状态机处理
		bool CheckOnHit() {
			/*
			if (attribute.IsDead)
				return false;
			
			if(myAnimationEvent.onHit) {
				myAnimationEvent.DoDamage();


				GameObject g = GameObject.Instantiate (bloodBomb) as GameObject;
				g.transform.position = transform.position;
				
				GameObject g2 = GameObject.Instantiate (swordHit) as GameObject;
				g2.transform.position = transform.position;

				return true;
			}
*/
			return false;
		}


		// Update is called once per frame
		void Update ()
		{
			CheckOnHit ();
			if (attribute._characterState == CharacterState.Idle && attribute.CheckDead ()) {
				StartCoroutine(ShowDead());
			}

			if (attribute._characterState == CharacterState.Idle) {
				animation.CrossFade("idle");
			}
		}
	}

}