
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
using System;

namespace ChuMeng
{
	[RequireComponent(typeof(AnimationController))]
	[RequireComponent(typeof(PlayerSync))]
	public class PlayerAIController : AIBase
	{

		public CollisionFlags collisionFlags;
	
		MyAnimationEvent myAnimationEvent;

		CharacterController controller;
		/// <summary>
		/// 从配置代码文件中读取用于所有的职业角色 例如  Config.cs  Config.config.xxxx
		/// </summary>
		float KnockBackTime {
			get {
				return 0.2f;
			}
		}

		float StopKnockTime {
			get {
				return 0.1f;
			}
		}

		float KnockBackSpeed {
			get {
				return 10;
			}
		}

		float UpSpeed {
			get {
				return 1;
			}
		}

		float Gravity {
			get {
				return 20;
			}
		}
	

		AnimationController animationController;
		void Awake ()
		{
			controller = GetComponent<CharacterController> ();
			attribute = GetComponent<NpcAttribute> ();
			myAnimationEvent = GetComponent<MyAnimationEvent> ();
			animationController = GetComponent<AnimationController> ();

			ai = new HumanCharacter ();
			ai.attribute = attribute;
			ai.AddState (new HumanIdle());
			ai.AddState (new HumanMove ());
			ai.AddState (new HumanCombat ());
			ai.AddState (new HumanSkill ());
			ai.AddState (new HumanDead ());
			ai.AddState (new MonsterKnockBack ());

		}

		void Start ()
		{
			ai.ChangeState (AIStateEnum.IDLE);
		}

	}



}
