using UnityEngine;
using System.Collections;
using SimpleJSON;

/*
 * Initial CharacterData From Network
 * Usage:
 * 		Show Information On CharacterBoard
 */
using System.Collections.Generic;

 
namespace ChuMeng
{
	/// <summary>
	/// 角色或者怪物的属性
	/// 角色的属性通过网络来初始化
	///	怪物的属性 通过本地数据库加载来初始化
	/// </summary>
	//TODO::怪物的属性 通过本地数据库加载来初始化
	public class CharacterInfo : KBEngine.MonoBehaviour
	{
		private Dictionary<CharAttribute.CharAttributeEnum, int> propertyValue = new Dictionary<CharAttribute.CharAttributeEnum, int> ();
		Dictionary<CharAttribute.CharAttributeEnum, bool> propertyDirty = new Dictionary<CharAttribute.CharAttributeEnum, bool>();
		bool initYet = false;
		NpcAttribute attribute;
	
		/*
		 * Load From Json File PropertyKey
		 * 
		 * No State Code
		 * 根据从服务器加载的角色数据来初始化NpcAttribute 中的数值（后续可能需要调整这种方式）
		 */ 

		IEnumerator InitProperty() {
			Log.Net ("characterinfo   init");
			NetDebug.netDebug.AddConsole("CharacterInfo:: Init Property "+gameObject.name);
			CGGetCharacterInfo.Builder getChar = CGGetCharacterInfo.CreateBuilder ();
			getChar.PlayerId = photonView.GetServerID();

			foreach (CharAttribute.CharAttributeEnum e in (CharAttribute.CharAttributeEnum[])System.Enum.GetValues(typeof(CharAttribute.CharAttributeEnum))) {
				var key = (int)e;
				//TODO: Hp mp load From LocalData
				if(e != CharAttribute.CharAttributeEnum.LEVEL) {
					getChar.AddParamKey (key);
				}
			}

			NetDebug.netDebug.AddConsole ("GetChar is "+getChar.ParamKeyCount);

			KBEngine.PacketHolder packet = new KBEngine.PacketHolder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, getChar, packet));

			if (packet.packet.responseFlag == 0) {
				var info = packet.packet.protoBody as GCGetCharacterInfo;
				Debug.Log("CharacterInfo::InitProperty "+info);
				foreach(RolesAttributes att in info.AttributesList) {

					if(att.AttrKey == (int)CharAttribute.CharAttributeEnum.GOLD_COIN) {
						Log.Net("attr int "+att.BasicData.Indicate);
						Log.Net("attr key "+att.BasicData.TheInt32);
						Log.Net("attr "+att.ToString());
					}
					if(att.BasicData.Indicate == 2) {
						propertyValue[(CharAttribute.CharAttributeEnum)att.AttrKey] =(int) att.BasicData.TheInt64;
					}else {
						propertyValue[(CharAttribute.CharAttributeEnum)att.AttrKey] = att.BasicData.TheInt32;
					}
				}
			} else {
				Debug.LogError("CharacterInfo::InitProperty Error");
			}

			var m = GetProp (CharAttribute.CharAttributeEnum.GOLD_COIN);
			Log.Net (m.ToString());

			Log.Important ("PropertyValue is "+propertyValue);
			//Hp MP Init From Config table
			SetProp (CharAttribute.CharAttributeEnum.HP, attribute.ObjUnitData.HP);
			SetProp (CharAttribute.CharAttributeEnum.HP_MAX, attribute.ObjUnitData.HP);
			SetProp (CharAttribute.CharAttributeEnum.MP, attribute.ObjUnitData.MP);
			SetProp (CharAttribute.CharAttributeEnum.MP_MAX, attribute.ObjUnitData.MP);
			SetProp (CharAttribute.CharAttributeEnum.LEVEL, attribute.ObjUnitData.Level);


			attribute.ChangeHP (0);
			attribute.ChangeMP (0);
			attribute.ChangeExp (0);
			Log.Important ("Init HP "+attribute.HP);
			Log.Important ("Init Property Over");
			initYet = true;
			NetDebug.netDebug.AddConsole ("Init CharacterInfo Over");
			Log.Sys ("UpdatePlayerData "+attribute.ObjUnitData.Level);
			var evt = new MyEvent (MyEvent.EventType.UpdatePlayerData);
			evt.localID = attribute.GetLocalId ();
			MyEventSystem.myEventSystem.PushEvent (evt);
		}

		public int GetProp(CharAttribute.CharAttributeEnum key) {
			int v = 1;
			if (!propertyValue.TryGetValue (key, out v)) {

				//Debug.LogError("Object No Attribuet "+gameObject.name+" "+key);
			}
			return v;
		}

		//TODO:Level Up
		public void SetProp(CharAttribute.CharAttributeEnum key, int val) {
			propertyValue [key] = val;
			/*
			if (key == CharAttribute.CharAttributeEnum.LEVEL) {
				RolesInfo.Builder rb = RolesInfo.CreateBuilder(SaveGame.saveGame.selectChar);
				rb.Level = val;
				SaveGame.saveGame.selectChar = rb.BuildPartial();
			}
			*/
		}
	
		public void SetDirty(CharAttribute.CharAttributeEnum key) {
			propertyDirty [key] = true;
		}
		public void ClearDirty(CharAttribute.CharAttributeEnum key) {
			propertyDirty [key] = false;
		}
		public bool GetDirty(CharAttribute.CharAttributeEnum key) {
			bool ret = false;
			propertyDirty.TryGetValue (key, out ret);
			return ret;
		}

		// Use this for initialization
		IEnumerator Start ()
		{
			while (KBEngine.KBEngineApp.app == null) {
				yield return null;
			}
			attribute = GetComponent<NpcAttribute> ();
			if(photonView.IsMine) {
				StartCoroutine(InitProperty());
			}
		}
	
		//更新玩家角色的数据缓冲池
		//每次进入新场景 重新加载全部数据
		public void UpdateData(PlayerRolesAttributes msg) {
			Log.Important ("Update Character Data Msg "+msg.ToString());
			foreach (RolesAttributes att in msg.AttributesList) {
				CharAttribute.CharAttributeEnum key = (CharAttribute.CharAttributeEnum)att.AttrKey;
				int value = att.BasicData.TheInt32;
				propertyValue[key] = value;
				if(key == CharAttribute.CharAttributeEnum.SILVER_TICKET) {
					MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UnitMoney);
				}
			}
		}

	}

}