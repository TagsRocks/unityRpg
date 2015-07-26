
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng {
	public class PacketUI : IUserInterface {
		public enum AttributePage {
			Knapsack,
			Property,
			Skill,
			Primordial,
		}
		AttributePage curSelect = AttributePage.Knapsack;

		void Awake() {

			SetCallback ("CloseButton", Hide);

			SetCheckBox ("KnapsackButton", OnKnap);
			SetCheckBox ("PropertyButton", OnProp);
			SetCheckBox ("SkillButton", OnSkill);
			SetCheckBox ("primordial spiritButton", OnPri);
		}

		void OnKnap(bool b) {
			if (b) {
				curSelect = AttributePage.Knapsack;
				UpdateFrame();
			}
		}

		void OnProp(bool b) {
			if (b) {
				curSelect = AttributePage.Property;
				UpdateFrame();
			}
		}
		void OnSkill(bool b) {
			if (b) {
				curSelect = AttributePage.Skill;
				UpdateFrame();
				MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateSkill);
			}
		}
		void OnPri(bool b) {
			if (b) {
				curSelect = AttributePage.Primordial;
				UpdateFrame();
			}
		}

		void UpdateFrame () {
			Log.Important ("Update Attribuet Packet Frame");
			GetName ("Leftequip").SetActive (false);
			GetName ("RightKnapsack").SetActive (false);
			GetName ("RightProperty").SetActive (false);
			GetName ("Skill_panel").SetActive (false);
			GetName ("Shop_Panel").SetActive (false);

			if (curSelect == AttributePage.Knapsack) {
				GetName ("Leftequip").SetActive (true);
				GetName ("RightKnapsack").SetActive (true);
			} else if (curSelect == AttributePage.Property) {
				GetName ("Leftequip").SetActive (true);
				GetName ("RightProperty").SetActive (true);
				MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.OpenAttr);
			} else if (curSelect == AttributePage.Skill) {
				GetName ("Skill_panel").SetActive (true);
			} else if (curSelect == AttributePage.Primordial) {
				Log.Critical("Not implement");
			}
		}

	}
}
