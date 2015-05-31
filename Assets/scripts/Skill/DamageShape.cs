using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	/// <summary>
	/// 技能的伤害碰撞模型
	/// </summary>
	public class DamageShape : MonoBehaviour
	{
		public enum Shape
		{
			Sphere,
			Angle,
		}
		//TODO:增加角度攻击类型 近战的攻击
		public Shape shape = Shape.Sphere;
		//正负多少角度攻击
		//普通物理 攻击 以及 火焰陷阱的 攻击使用了这个 Angle
		public float angle = 0; //< 90

		SkillLayoutRunner runner;
		//伤害持续的时间通过enable确定 
		//如果是一开始 DamageShape 就要生效，那么就设置Enable 为true，否则用动画Timeline 控制enable什么时候为true

		//火焰陷阱喷出的火焰 只造成一次伤害，伤害之后，则失效，因此默认DamageShape enable为true 且e 为true，
		public bool enable;
		public float radius = 2f;
		string enemytTag;
		float cosAngle;
		//是否只产生一次伤害  
		public bool Once = false;
		bool damageYet = false;
		HashSet<GameObject> hurtEnemy = new HashSet<GameObject> ();
		Vector3 InitPosition;


		//向前冲击技能需要 DamageShape 和玩家的位置重合在一起
		public bool SyncWithPlayer = false;
		//是否造成伤害
		public bool NoHurt = false;

		//是否会移动主角 多长时间 多长距离
		//public bool needMove = false;
		//public float distance = 0;
		//冲击移动速度
		public float speed = 6;
		bool enableYet = false;

		public float Distance = 6;
		Vector3 targetPos;
		//触发Hit事件的时候 是否停止移动  冲击技能
		//public bool hitStop = false;
		void Awake ()
		{
			//enable = false;
			/*
			var g = GameObject.CreatePrimitive (PrimitiveType.Cube);
			g.transform.parent = transform;
			g.transform.localScale = new Vector3(0.5f, 0.5f, radius);
			g.transform.localPosition = new Vector3(0, 0, radius/2);
			g.collider.enabled = false;
			*/
		}

		void Start ()
		{
			runner = transform.parent.GetComponent<SkillLayoutRunner> ();
			enemytTag = ObjectManager.GetEnemyTag (runner.stateMachine.attacker.tag);
			cosAngle = Mathf.Cos (angle);
			if (runner != null && runner.Event.attachOwner) {
				InitPosition = runner.transform.position;
				targetPos = InitPosition+transform.forward*Distance;
			}

		}

	


		//TODO::玩家和怪物不会发生碰撞 只有玩家和墙体才回发生碰撞 导致 暂停，简化碰撞计算
		void Update ()
		{

			if (Once && damageYet) {
				return;
			}
			if (enable && !runner.stateMachine.isStop) {

				if (!NoHurt) {
					Collider[] hitColliders;
					if (SyncWithPlayer) {
						hitColliders = Physics.OverlapSphere (runner.stateMachine.attacker.transform.position, radius, 1 << (int)GameLayer.Npc);
					} else {
						hitColliders = Physics.OverlapSphere (transform.position, radius, 1 << (int)GameLayer.Npc);
					}
				 

					for (int i = 0; i < hitColliders.Length; i++) {
						if (hitColliders [i].tag == enemytTag) {
							if (!hurtEnemy.Contains (hitColliders [i].gameObject)) {
								if (shape == Shape.Sphere) {
									DoDamage (hitColliders [i].gameObject);
									hurtEnemy.Add (hitColliders [i].gameObject);
								} else if (shape == Shape.Angle) {
									Log.AI ("DamageHit " + runner.stateMachine.name + " " + hitColliders [i].name);
									var dir = hitColliders [i].gameObject.transform.position - transform.position;
									var cos = Vector3.Dot (dir.normalized, transform.forward);
									if (cos > cosAngle) {
										DoDamage (hitColliders [i].gameObject);
										hurtEnemy.Add (hitColliders [i].gameObject);
									}
								}
							}
						}
					}
					Log.AI ("Check Damage Shape " + runner.stateMachine.name);
					damageYet = true;
				}

				//同时也移动玩家 
				//TODO: 移动失败则DamageShape也停止移动
				if (runner != null && runner.Event.attachOwner) {
					Log.Sys ("Move Attack With DamageShape");
					if(!enableYet) {
						StartCoroutine(MoveOwner());
					}
				}
				enableYet = true;
			}
		}

		IEnumerator MoveOwner() {
			Log.Sys ("enter Move State");
			var ret = runner.stateMachine.attacker.GetComponent<PhysicComponent> ().EnterSkillMoveState ();
			if (!ret) {
				yield break;
			}

			/*
			var sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sp.transform.position = targetPos;
			sp.collider.isTrigger = true;
			
			var sp2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			sp2.transform.position = InitPosition;
			sp2.collider.isTrigger = true;
			*/

			float diff = 0;
			float halfDist = Distance / 2.0f;

			do{
				diff = (targetPos - runner.stateMachine.attacker.transform.position).sqrMagnitude;
				var newSpeed = speed;
				if(diff < halfDist) {
					newSpeed = diff/halfDist*speed;
				}

				bool suc = runner.MoveOwner (targetPos, newSpeed);
				yield return null;
			}while(diff > 0.1f && !runner.stateMachine.isStop && enable);

			/*
			var phy = runner.stateMachine.attacker.GetComponent<PhysicComponent> ();
			while (phy.GetLastSpeed() > 0.1f) {
				runner.MoveOwner(targetPos, 0);
				yield return null;
			}
			*/


			Log.Sys ("ExitSkillMove ");
			//var localId = runner.stateMachine.attacker.GetComponent<KBEngine.KBNetworkView>().GetLocalId();
			runner.stateMachine.attacker.GetComponent<PhysicComponent> ().ExitSkillMove ();
		}

		/*
		 * 伤害计算过程
			1：伤害对象判定
			2：伤害数值确定
		 */ 
		//Todo:根据技能决定是否 击退
		void DoDamage (GameObject g)
		{
			runner.DoDamage (g);
		}

	}

}