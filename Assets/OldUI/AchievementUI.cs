using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ChuMeng{

	public class AchievementUI : IUserInterface{
	
		public GameObject mesh;

		//public AchievementType achievementType = AchievementType.metallurgy;

		public List<GameObject> item = null;
		public List<AchievementItem> achiItem = null;

		GameObject itemMesh;
		 
	
		void Awake()
		{
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> (){
				MyEvent.EventType.OpenAchievement,
			};
			RegEvent();

			SetCallback ("close", Hide);


		}

		/*标签响应的事件*/
		/*
		void OnMetallurgy(bool b){
			if (b) {
				achievementType = 0;
				UpdateFrame();
			}
		}

		void OnEquip(bool b){
			if (b) {
				achievementType = 1;
				UpdateFrame();
			}
		}

		void OnPet(bool b){
			if (b) {
				achievementType = 2;
				UpdateFrame();
			}
		}

		void OnConstellation(bool b){
			if (b) {
				achievementType = 3;
				UpdateFrame();
			}
		}

		void OnElse(bool b){
			if (b) {
				achievementType = 4;
				UpdateFrame();
			}
		}

		protected override void OnEvent(MyEvent evt){
			UpdateFrame ();		
		}
		*/

	

	}
}