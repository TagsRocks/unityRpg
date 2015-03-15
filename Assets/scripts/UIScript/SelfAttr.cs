using UnityEngine;
using System.Collections;


//人物属性面板
namespace ChuMeng
{
	public class SelfAttr : IUserInterface
	{
		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
				MyEvent.EventType.OpenAttr,
			};
			RegEvent ();

		}

		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}

		void UpdateFrame() {
			var player = ObjectManager.objectManager.GetMyPlayer ().GetComponent<ChuMeng.CharacterInfo>();
			GetLabel ("FightValue").text = player.GetProp (CharAttribute.CharAttributeEnum.FIGHT_TOTAL_CAPACITY).ToString();
			GetLabel ("HPLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.HP).ToString () + "/" + player.GetProp (CharAttribute.CharAttributeEnum.HP_MAX).ToString ();
			GetLabel ("MPLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.MP).ToString () + "/" + player.GetProp (CharAttribute.CharAttributeEnum.MP_MAX).ToString ();
			GetLabel ("BlockLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.BLOCK).ToString () + "/" + player.GetProp (CharAttribute.CharAttributeEnum.BLOCK_MAX).ToString ();
			GetSlider ("HPProgressBar").value = player.GetProp (CharAttribute.CharAttributeEnum.HP) * 1.0f / player.GetProp (CharAttribute.CharAttributeEnum.HP_MAX);
			GetSlider ("MPProgressBar").value = player.GetProp (CharAttribute.CharAttributeEnum.MP) * 1.0f / player.GetProp (CharAttribute.CharAttributeEnum.MP_MAX);
			GetSlider ("BlockProgressBar").value = player.GetProp (CharAttribute.CharAttributeEnum.BLOCK) * 1.0f / player.GetProp (CharAttribute.CharAttributeEnum.BLOCK_MAX);
			
			GetLabel ("PowerLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.POWER).ToString ();
			GetLabel ("VitalityLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.ENERGY).ToString ();
			GetLabel ("ReputationLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.REPUTATION).ToString ();
			GetLabel ("FightingWillLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.FIGHTING_WILL).ToString ();
			GetLabel ("TibiaLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.PHYSIQUE).ToString ();
			GetLabel ("AccurateLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.PRECISE).ToString ();
			GetLabel ("ConstitutionLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.CONSTITUTION).ToString ();
			GetLabel ("GailLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.GAIL).ToString ();
			GetLabel ("SpeedLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.MOVE_SPEED).ToString ();
			
			GetLabel ("ATKLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.ATTACK).ToString ();
			GetLabel ("DEFLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.DEFENSE).ToString ();
			GetLabel ("HitLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.HIT).ToString ();
			GetLabel ("DodgeLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.DODGE).ToString ();
			GetLabel ("CRILabel").text = player.GetProp (CharAttribute.CharAttributeEnum.CRITICAL).ToString ();
			GetLabel ("CRIDEFLabel").text = player.GetProp (CharAttribute.CharAttributeEnum.CRITICAL_DEFENSE).ToString ();
			
			GetLabel ("WaterATK").text = player.GetProp (CharAttribute.CharAttributeEnum.WATER_ATTACK).ToString ();
			GetLabel ("FireATK").text = player.GetProp (CharAttribute.CharAttributeEnum.FIRE_ATTACK).ToString ();
			GetLabel ("WindATK").text = player.GetProp (CharAttribute.CharAttributeEnum.WIND_ATTACK).ToString ();
			GetLabel ("SoilATK").text = player.GetProp (CharAttribute.CharAttributeEnum.SOIL_ATTACK).ToString ();

			
			GetLabel ("WaterDEF").text = player.GetProp (CharAttribute.CharAttributeEnum.ANTI_WATER).ToString ();
			GetLabel ("FireDEF").text = player.GetProp (CharAttribute.CharAttributeEnum.ANTI_FIRE).ToString ();
			GetLabel ("WindDEF").text = player.GetProp (CharAttribute.CharAttributeEnum.ANTI_WIND).ToString ();
			GetLabel ("SoilDEF").text = player.GetProp (CharAttribute.CharAttributeEnum.ANTI_SOIL).ToString ();
			
		}
	}

}
