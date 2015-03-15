﻿
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
	//多种类型子弹子类 bulletType
	public class Bullet : MonoBehaviour
	{
		bool isDie = false;
		public SkillData skillData;
		public GameObject attacker;
		public MissileData missileData;
		public int LeftRicochets = 0;
		float velocity;
		float maxDistance;
		Vector3 initPos;
		string enemyTag;
		//忽略碰撞检测的时间用于新生成的子弹逃离碰撞区域
		//弹射忽略掉上一个 对象即可 不用sleep 时间
		//float sleepTime = 0;
		Collider lastColobj = null;
		//static float IgnoreTime = 0.2f;
		//子弹相对于发射者的位置偏移
		public Vector3 OffsetPos;
		GameObject activeParticle;
		//TODO: 支持玩家或者怪物发射的子弹或者技能 根据attacker的tag Player 还是Enemy 确定其它Tag
		bool HitYet = false;
		void Start ()
		{
			LeftRicochets = missileData.NumRicochets;
			velocity = missileData.Velocity;
			maxDistance = missileData.MaxDistance;
			initPos = transform.position;
			enemyTag = SkillLogic.GetEnemyTag (attacker.tag);

			Log.AI ("bullet enemy is " + enemyTag);

			var playerForward = Quaternion.Euler (new Vector3 (0, 0 + attacker.transform.rotation.eulerAngles.y, 0));
			if (missileData.ReleaseParticle != null) {
				GameObject par = Instantiate (missileData.ReleaseParticle) as GameObject;
				NGUITools.AddMissingComponent<RemoveSelf> (par);
				//par.transform.parent = transform;
				par.transform.parent = ObjectManager.objectManager.transform;
				par.transform.localPosition = attacker.transform.localPosition + playerForward * OffsetPos;
				par.transform.localRotation = playerForward;
			}
			
			if (missileData.ActiveParticle != null) {
				GameObject par = Instantiate (missileData.ActiveParticle) as GameObject;
				activeParticle = par;
				par.transform.parent = transform;
				par.transform.localPosition = Vector3.zero;
				par.transform.localRotation = Quaternion.identity;
			}
		}
    
		//TODO: 子弹可以和四种对象碰撞：
		/*  
        1自己   自己的宠物
        2其它玩家  其它玩家的宠物
        3怪物
        4墙体
5: 事件类型碰撞体不参与技能 例如入口和出口的碰撞体？ （忽略这种 假设所有都会碰撞）


对于子弹的发射者来讲
按照角色分成三种：
1：友方
2：敌方
3：中立（墙体）
        */
		void FixedUpdate ()
		{
			if (isDie) {
				return;
			}
			//if (sleepTime <= 0) {
			//子弹不仅仅和动态对象碰撞不和 环境 墙体 碰撞
			Collider[] col = Physics.OverlapSphere (transform.position, missileData.Radius, 1 << (int)GameLayer.Npc);
			foreach (Collider c in col) {
				//和多个不同的敌人目标碰撞
				if (c != lastColobj) {
					lastColobj = c;
					OnTriggerEnter (c);
					break;
				}
			}

		}
		void DoDamage(Collider other){
			if (other.tag == enemyTag && !missileData.DontHurtObject) {
				SkillDamageCaculate.DoDamage (attacker, new SkillFullInfo (skillData), other.gameObject);
			}
		}

		void AOEDamage() {
			Collider[] col = Physics.OverlapSphere (transform.position, missileData.AOERadius, 1 << (int)GameLayer.Npc);
			foreach (Collider c in col) {
				DoDamage (c);
			}
		}
		// Update is called once per frame
		void Update ()
		{
			if (isDie) {
				return;
			}

			if (missileData.maxTurnRate > 0) {
				transform.position += transform.forward * velocity * Time.deltaTime;
				transform.localRotation = Quaternion.Euler (new Vector3 (0, transform.localRotation.eulerAngles.y + Time.deltaTime * Random.Range (-missileData.maxTurnRate, missileData.maxTurnRate), 0));
				//根据目标位置进行旋转计算
			}
			if (missileData.GotoPosition) {
				transform.position += transform.forward * velocity * Time.deltaTime;
			} else {
				transform.position += transform.forward * velocity * Time.deltaTime;
			}

			if ((transform.position - initPos).sqrMagnitude >= maxDistance * maxDistance) {
				if (missileData.DieParticle != null) {
					Log.AI ("bullet die " + gameObject.name);
					GameObject g = Instantiate (missileData.DieParticle) as GameObject;
					NGUITools.AddMissingComponent<RemoveSelf> (g);
					g.transform.parent = ObjectManager.objectManager.transform;
					g.transform.position = transform.position;

				}
				CreateCameraShake();

				isDie = true;
				//正常死亡也有AOE效果
				if(missileData.IsAOE) {
					AOEDamage();
				}

				if (missileData.DieWaitTime > 0) {
					Log.Sys ("Wait for time bullet to die");
					GameObject.Destroy (gameObject, missileData.DieWaitTime);
				} else {
					GameObject.Destroy (gameObject);
				}


				var evt = new MyEvent (MyEvent.EventType.EventMissileDie);
				evt.missile = transform;
				Log.Sys ("Push Missile Die Event " + attacker.GetComponent<KBEngine.KBNetworkView> ().GetLocalId ());
				MyEventSystem.myEventSystem.PushLocalEvent (attacker.GetComponent<KBEngine.KBNetworkView> ().GetLocalId (), evt);
			}
		}

		void CreateHitParticle() {
			if (missileData.HitParticle != null) {
				GameObject g = Instantiate (missileData.HitParticle) as GameObject;
				NGUITools.AddMissingComponent<RemoveSelf> (g);
				g.transform.position = transform.position;
				g.transform.parent = ObjectManager.objectManager.transform;

			}
			CreateCameraShake();
		}

		//Remove Self
		void CreateCameraShake() {
			if(missileData.shakeData != null) {
				var shakeObj = new GameObject("CameraShake");
				shakeObj.transform.parent = ObjectManager.objectManager.transform;
				var shake = shakeObj.AddComponent<CameraShake>();
				shake.shakeData = missileData.shakeData;
				shake.autoRemove = true;
			}
		}

		/*
         *客户端表现粒子和服务器计算伤害的数值分离开来
         */ 
		//TODO:撞击墙壁如何处理
		void OnTriggerEnter (Collider other)
		{
			Log.AI ("Bullet collider enemy " + other.name + " " + other.tag);
			if (other.tag == enemyTag) {
				//攻击多个目标只释放一次 DieParticle
				CreateHitParticle();

				//计算随机的弹射 反射方向
				if (LeftRicochets > 0) {
					Log.AI ("Generate new bullet " + LeftRicochets);
					LeftRicochets--;
					initPos = transform.position;
					//子弹应该冲入怪物群中
					transform.localRotation = Quaternion.Euler (new Vector3 (0, transform.localRotation.eulerAngles.y  + Random.Range (-missileData.RandomAngle, missileData.RandomAngle), 0));
					//sleepTime = IgnoreTime;
				} else {
					//地震是穿透的因此只会等待粒子自然死亡
					//非穿透性子弹 
					if (!missileData.piercing) {
						GameObject.Destroy (gameObject);
					}
				}

				//伤害多个目标
				if(missileData.IsAOE) {
					AOEDamage();
				}else {//只伤害一个目标
					//TODO:通过SkillDamageCaculate计算伤害 Level Information
					DoDamage(other);

				}

				//非穿透性子弹
				if (!missileData.piercing) {
					var evt = new MyEvent (MyEvent.EventType.EventMissileDie);
					evt.missile = transform;
					MyEventSystem.myEventSystem.PushLocalEvent (attacker.GetComponent<KBEngine.KBNetworkView> ().GetLocalId (), evt);
				}
				//TODO::撞击其它玩家如何处理
			} else if (other.tag == attacker.tag) { 
			} else {//装到墙体 或者建筑物 等对象身上 则 反射  Not used
				Log.AI ("Bullet colldier with Wall " + gameObject.name);
				if (missileData.HitParticle != null) {
					GameObject g = Instantiate (missileData.HitParticle) as GameObject;
					NGUITools.AddMissingComponent<RemoveSelf> (g);
					g.transform.position = transform.position;
					g.transform.parent = ObjectManager.objectManager.transform;
				}

				if (LeftRicochets > 0) {
					Log.AI ("Generate new bullet " + LeftRicochets);
					LeftRicochets--;
					initPos = transform.position;
					transform.localRotation = Quaternion.Euler (new Vector3 (0, transform.localRotation.eulerAngles.y + 180 + Random.Range (-missileData.RandomAngle, missileData.RandomAngle), 0));
					//sleepTime = IgnoreTime;
				} else {
					if (!missileData.piercing) {
						GameObject.Destroy (gameObject);
					}
				}
				if (!missileData.piercing) {
					var evt = new MyEvent (MyEvent.EventType.EventMissileDie);
					evt.missile = transform;
					MyEventSystem.myEventSystem.PushLocalEvent (attacker.GetComponent<KBEngine.KBNetworkView> ().GetLocalId (), evt);
				}
			}

		}

	}

}
