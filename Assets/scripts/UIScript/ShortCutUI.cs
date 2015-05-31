using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	/// <summary>
	/// 设置技能快捷键的UI
	/// 
	/// 			5
	///			6		1
	///		7		2
	///	8		3
	///		4
	/// 
	/// 对应槽的编号 0 - 7
	/// </summary>
	public class ShortCutUI : IUserInterface
	{
		int skillId;
		int level;

		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.OpenShortCut,
			};
			RegEvent ();

			for(int i = 1; i <= 8; i++) {
				SetCallback("SkillButton"+i.ToString(), OnSkillButton);
			}
		}

		void OnSkillButton(GameObject g) {
			var id = System.Convert.ToInt32(g.name.Replace ("SkillButton", ""));
			GameInterface_Skill.skillInterface.SetSkillShortCut (skillId, id);
			//Close UI
			Hide (null);
		}

		//初始化当前的 技能  快捷键配置
		protected override void OnEvent (MyEvent evt)
		{
			if (evt.type == MyEvent.EventType.OpenShortCut) {
				skillId = evt.intArg;
				UpdateFrame();
			}
		}

		void UpdateFrame() {
			for (int i = 1; i <= 8; i++) {
				var but = GetName("SkillButton"+i.ToString());
			
				var icon = Util.FindChildRecursive(but.transform, "icon");
				var shortCut = GameInterface_Skill.skillInterface.GetShortSkillData(i-1);
				if(shortCut != null) {
					icon.gameObject.SetActive(true);
					Util.SetIcon(icon.GetComponent<UISprite>(), shortCut.sheet, shortCut.icon);
				}else {
					icon.gameObject.SetActive(false);
				}

			}
		}
	}


}
