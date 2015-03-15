
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

namespace ChuMeng
{
/*
 * Object Command List
 */ 
	public class ObjectCommand
	{
		public enum ENUM_OBJECT_COMMAND
		{
			INVALID = -1,
			OC_MOVE,
			OC_UPDATE_IMPACT, //Buff Update
			OC_USE_SKILL,
		}
		public int skillId;

		public ENUM_OBJECT_COMMAND commandID;
		public int logicCount;
		public uint startTime;

		public Vector3 targetPos;

		public GCPushUnitAddBuffer buffInfo;

		public ObjectCommand() {
		}
		public ObjectCommand(ENUM_OBJECT_COMMAND cmd) {
			commandID = cmd;
		}
	}



	/*
	 * Process Network Command 
	 * Change Character State
	 */ 
	/// <summary>
	/// 执行网络命令在特定的玩家 或者 怪兽身上
	/// 将网络命令转化成本地命令 在对象身上执行
	/// </summary>
	public class LogicCommand : MonoBehaviour
	{
		NpcAttribute attribute;
		MoveController mvController;

		List<ObjectCommand> commandList = new List<ObjectCommand> ();
		ObjectCommand currentLogicCommand = null;
		float logicSpeed = 1;

		/*
		 * NetWork Command Input
		 */ 
		public void PushCommand(ObjectCommand cmd) {
			Log.Important ("Push Command What "+cmd.commandID);
			commandList.Add (cmd);
		}

		bool DoLogicCommand (ObjectCommand cmd)
		{
			Log.AI ("DoLogicCommad "+cmd.commandID);
			currentLogicCommand = cmd;

			bool ret = false;
			switch (cmd.commandID) {
			case ObjectCommand.ENUM_OBJECT_COMMAND.OC_MOVE:
				StartCoroutine (Move (cmd));
				break;
			case ObjectCommand.ENUM_OBJECT_COMMAND.OC_USE_SKILL:
				EnterUseSkill(cmd);
				break;
			default:
				Debug.LogError("LogicCommand:: UnImplement Command "+cmd.commandID.ToString());
				break;
			}
			return ret;
		}

		//所有命令执行函数需要注意命令执行结束的时候 设定当前命令为null
		void EnterUseSkill(ObjectCommand cmd) {
			//判断是否可以使用技能

			//向MyAnimationEvent 注入消息
			var msg = new MyAnimationEvent.Message (MyAnimationEvent.MsgType.DoSkill);
			msg.cmd = cmd;
			GetComponent<MyAnimationEvent> ().InsertMsg (msg);

			currentLogicCommand = null;
		}

		/*
		 * Multiple Node List Move next after Next
		 * ref: PlayerMovementController
		 * Run Move Command One By One
		 */ 
		IEnumerator Move(ObjectCommand cmd) {
		
			while (true) {
				Vector3 mypos = transform.position;
				Vector3 tarPos = cmd.targetPos;
				float dx = tarPos.x-mypos.x;
				float dz = tarPos.z-mypos.z;

				//Debug.Log("LogicCommand::Move mypos tarPos "+tarPos);
				//Debug.Log("LogicCommand::Move "+mypos);

				Vector2 vdir = new Vector2(dx, dz);
				/*
				 * Near Target then Stop Move
				 * Command: Move To target
				 */ 
				if(vdir.sqrMagnitude < 0.1f) {
					mvController.vcontroller.inputVector.x = 0;
					mvController.vcontroller.inputVector.y = 0;
					break;
				}

				vdir.Normalize();
				//Debug.Log("LogicCommand vdir "+vdir+" "+mvController.camRight);

				Vector2 mRight = new Vector2(mvController.camRight.x, mvController.camRight.z);
				Vector2 mForward = new Vector2(mvController.camForward.x, mvController.camForward.z);

				float hval = Vector2.Dot(vdir, mRight);
				float vval = Vector2.Dot(vdir, mForward);

				//Debug.Log("LogicCommand hval vval"+hval+" "+vval);
				mvController.vcontroller.inputVector.x = hval;
				mvController.vcontroller.inputVector.y = vval;
			
				yield return null;
			}

			currentLogicCommand = null;
		}


		/*
		 * There Are New Command  And Current Is Idle
		 */ 
		IEnumerator CommandHandle() {
			while (true) {
				if (commandList.Count > 0) {
					if(currentLogicCommand == null) {
						var cmd = commandList[0];
						commandList.RemoveAt (0);
						
						int logicCommandCount = commandList.Count;
						logicSpeed = logicCommandCount * 0.5f + 1.0f;
						DoLogicCommand(cmd);
					}
				}
				yield return null;
			}
		}

		void Start() {
			attribute = GetComponent<NpcAttribute>();
			mvController = GetComponent<MoveController>();
			//mvController.vcontroller = new VirtualController();

			StartCoroutine (CommandHandle());
		}
	}
}
