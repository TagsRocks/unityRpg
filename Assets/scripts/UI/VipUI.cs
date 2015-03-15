/*
Author :  Wangjunbo
Email  :  1305201219@qq.com
*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class VipUI : IUserInterface
	{
		public VipInfo vipinfo;                             //角色vip信息

		public List<GameObject> item = null;
		public List<VipLevelItem> viplevel  = null;
		public List<VipFreeItem> vipfree = null;
		public GameObject vipMesh;

		bool type = true;      //礼包类型   true为等级礼包    false为特权礼包

		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> (){
				MyEvent.EventType.UpdateVip,
			};
			RegEvent ();

			//奖励模板
			item = new List<GameObject>();
			vipMesh = GetName ("VipMesh");
			vipMesh.name = "0";
			item.Add(vipMesh);


			SetCallback ("close", Hide);               
			SetCheckBox ("levelToggle", OnLevel);
			SetCheckBox ("privilegeToggle", OnPrivilege);
		}

		void OnLevel(bool b) {
			if (b) {
				type = true;
				UpdateFrame();
			}
		} 

		void OnPrivilege(bool b){
			if (b) {
				type = false;
				UpdateFrame();
			}
		}

		/*
		 * 领取礼包按钮响应事件还未完成
		*/
		public void GetGiftBag(){
					
		}


		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}

		void UpdateFrame() {
			//获取vip信息
			vipinfo = GameInterface_Vip.vipInterface.GetVipInfo ();
			if (vipinfo != null) {
				//修改vip等级图片
				var level = Util.FindChildRecursive (transform, "Icon").GetComponent<UISprite> ();
				level.spriteName = "vip" + vipinfo.Level.ToString ();
				
				//将vip经验值转换为value值
				var exp = Util.FindChildRecursive (transform, "Progress Bar").GetComponent<UISlider> ();
				if (vipinfo.Level < 15) {
					var nowexp = vipinfo.Exp;
					var needexp = GameInterface_Vip.vipInterface.GetVipNeedExp (vipinfo.Level+1);
					exp.value = nowexp * 1.0f / needexp;	
				}else
				{
					exp.value = 1;
				}
				
				//判断是否有开通vip月卡
				var notvip = Util.FindChildRecursive (transform, "notVip").GetComponent<UILabel> ();
				if (vipinfo.Time == 0) {
					notvip.text = "您还没有开通Vip月卡";			
				}else{
					var time = vipinfo.Time / 86400000;
					notvip.text = "vip剩余天数： " + time.ToString() + "天";
				}		
			}


			//获取玩家特权
			var explain = Util.FindChildRecursive (transform, "explain").GetComponent<UILabel> ();
			explain.text = GameInterface_Vip.vipInterface.GetVipDescribe (vipinfo.Level);

			//获取玩家的金币
			var gold = Util.FindChildRecursive (transform, "number").GetComponent<UILabel> ();
			var game = ObjectManager.objectManager.GetMyPlayer ().GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.GOLD_COIN);
			gold.text = game.ToString ();

			//加载礼包
			if (type) {
				//获取vip等级奖励礼包
				viplevel = GameInterface_Vip.vipInterface.GetVipLevelAward();
				Log.GUI("Get VipLevel of List" + viplevel.Count);
			
				//加载礼包奖励gameobject
				int c = item.Count;
				while( item.Count < viplevel.Count){
					Log.Net("item  data " + item.Count);
					var items = NGUITools.AddChild(vipMesh.transform.parent.gameObject, vipMesh);
					items.name = c.ToString();
					item.Add(items);
					c++;
				}

				for(int i = 0; i < viplevel.Count; i++){
					item[i].SetActive(true);

					//礼包描述
					var describe = Util.FindChildRecursive(item[i].transform, "describe").GetComponent<UILabel>();
					Debug.Log(describe);
					describe.text = viplevel[i].Describe;


					//礼包按钮状态
					var getbutton = Util.FindChildRecursive(item[i].transform, "GetButton").GetComponent<UIButton>();
					Debug.Log(getbutton);

					if (!viplevel[i].Enable){
						getbutton.isEnabled = false;
					}else{
						getbutton.isEnabled = true;
					}

				}

			}else{
				//获取vip特权奖励礼包
				vipfree = GameInterface_Vip.vipInterface.GetVipFreeAward();
				Log.GUI("Get VipFree of List" + vipfree.Count);


				for(int i = 0; i < vipfree.Count; i++){
					//礼包描述
					var describe = Util.FindChildRecursive(item[i].transform, "describe").GetComponent<UILabel>();
					Debug.Log(describe);
					describe.text = vipfree[i].Describe;


					//礼包按钮状态
					var getbutton = Util.FindChildRecursive(item[i].transform, "GetButton").GetComponent<UIButton>();
					Debug.Log(getbutton);


					if(!vipfree[i].Enable){
						getbutton.isEnabled = false;
					}else
					{
						getbutton.isEnabled = true;
					}

				}


				for(int i = vipfree.Count; i < item.Count; i++){
					item[i].SetActive(false);
				}
				
			}

			//重置grid排列
			vipMesh.transform.parent.gameObject.GetComponent<UIGrid> ().Reposition ();
		}

	}
}