/*
 * Author : Wangjunbo
 * Email : 1305201219@qq.com
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class GameInterface_Task{
		public static GameInterface_Task TaskInterface = new GameInterface_Task();



		//获取任务
		public List<TaskItem> GetTaskList(int Type){
			return TaskController.taskController.GetTask (Type);
		}

		/*
		//获取任务目标
		public string GetTaskTarGet(int type,int Id){
			return TaskController.taskController.GetTaskTarget(type,Id);
		}

		//获取任务奖励
		public List<AwardItem> GetTaskAward(int type,int Id){
			return TaskController.taskController.GetTaskAward (type,Id);
		}*/


	}


}