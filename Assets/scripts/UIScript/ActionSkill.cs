using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class ActionSkill : IUserInterface
	{
		SkillSort selSkill = SkillSort.ACTIVE_SKILL;
		List<GameObject> skillMeshes = new List<GameObject>();
		GameObject SkillMesh;

		int curSkill = 0;
		List<SkillFullInfo> allSkill = null;
		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> (){
				MyEvent.EventType.UpdateSkill,
			};
			RegEvent ();

			SetCheckBox ("MailSkillButton", OnActive);
			SetCheckBox ("PassSkillButton", OnPass);
			SetCheckBox ("ElseSkillButton", OnElse);

			SkillMesh = GetName ("SkillMesh");
			skillMeshes.Add (SkillMesh);

			SetCallback ("ResetButton", OnReset);
			SetCallback ("UpButton", OnUp);
			SetCallback ("ShortcutButton", OnShort);
		}

		void OnReset(GameObject g) {
			var sk = allSkill [curSkill];
			GameInterface_Skill.skillInterface.ResetSkill (sk.skillId);

		}

		void OnUp(GameObject g) {
			var sk = allSkill [curSkill];
			GameInterface_Skill.skillInterface.SkillLevelUp (sk.skillId);
		}

		void OnShort(GameObject g) {
			WindowMng.windowMng.PushView ("UI/ShortCutPanel");
			var evt = new MyEvent (MyEvent.EventType.OpenShortCut);
			var sk = allSkill [curSkill];
			evt.intArg = sk.skillId;
			evt.intArg1 = sk.level;
			MyEventSystem.myEventSystem.PushEvent (evt);
		}


		void OnActive(bool b) {
			if (b) {
				selSkill = SkillSort.ACTIVE_SKILL;
				UpdateFrame ();
			}
		}
		void OnPass(bool b) {
			if (b) {
				selSkill = SkillSort.PASV_SKILL;
				UpdateFrame ();
			}
		}

		void OnElse(bool b) {
			if (b) {
				selSkill = SkillSort.EMBLEM_SKILL;
				UpdateFrame ();
			}
		}

		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}

		void OnSkill(GameObject g) {
			Log.GUI ("On Skill Button "+g.name);
			curSkill = System.Convert.ToInt32(g.name);
			UpdateSkillInfo ();
		}

		//更新右侧技能面板信息
		void UpdateSkillInfo() {
			if(allSkill.Count > curSkill) {
				var sk = allSkill [curSkill];
				GetLabel("Lv_Int_1").text = sk.level.ToString(); 
				GetLabel("Text1").text = GameInterface_Skill.skillInterface.GetSkillDesc(sk.skillData);
				GetLabel("time1").text = (sk.skillData.CooldownMS/1000.0f).ToString();
				GetLabel("CostMp1").text = sk.skillData.ManaCost.ToString();
				GetLabel("CostSp1").text = sk.skillData.SpCost.ToString();


				if(sk.level < sk.skillData.MaxLevel) {
					GetName("Line2").SetActive(true);
					GetLabel("Lv_Int_2").text = (sk.level+1).ToString();
					var sk2 = Util.GetSkillData(sk.skillId, sk.level+1);
					GetLabel("Text2").text = GameInterface_Skill.skillInterface.GetSkillDesc(sk2);
					GetLabel("time2").text = (sk2.CooldownMS/1000.0f).ToString();
					GetLabel("CostMp1").text = sk2.ManaCost.ToString();
					GetLabel("CostSp1").text = sk2.SpCost.ToString();
				}else {
					GetName("Line2").SetActive(false);
				}
			}

			GetLabel ("LeftCount").text = GameInterface_Skill.skillInterface.GetLeftSp ().ToString();
			GetLabel ("UsedSp").text = GameInterface_Skill.skillInterface.DistriSp ().ToString ();

		}


		void UpdateFrame() {
			allSkill = null;
			if (selSkill == SkillSort.ACTIVE_SKILL) {	
				allSkill = GameInterface_Skill.skillInterface.GetActiveSkill();
			}else if (selSkill == SkillSort.PASV_SKILL) {
				allSkill = GameInterface_Skill.skillInterface.GetPassitiveSkill();
			}else if(selSkill == SkillSort.EMBLEM_SKILL) {
			}
			while (skillMeshes.Count < allSkill.Count) {
				skillMeshes.Add(NGUITools.AddChild(SkillMesh.transform.parent.gameObject, SkillMesh));
			}

			int c = 0;
			foreach(GameObject g in skillMeshes) {
				g.name = c.ToString();
				g.SetActive(false);
				//g.GetComponent<UIButton>().onClick.Clear();
				c++;
			}

			c = 0;
			foreach (SkillFullInfo sk in allSkill) {
				skillMeshes[c].SetActive(true);
				var icon = Util.FindChildRecursive(skillMeshes[c].transform, "Icon");
				var label = Util.FindChildRecursive(skillMeshes[c].transform, "SkillName");
				label.GetComponent<UILabel>().text = sk.skillData.SkillName;
				Util.SetIcon(icon.GetComponent<UISprite>(), sk.skillData.sheet, sk.skillData.icon);
				//EventDelegate.Add(skillMeshes[c].GetComponent<UIButton>().onClick, OnSkill);
				UIEventListener.Get(skillMeshes[c]).onClick = OnSkill;
				c++;
			}

			UpdateSkillInfo ();
		}


	}
}
