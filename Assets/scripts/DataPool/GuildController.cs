
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChuMeng
{
	public class GuildController : MonoBehaviour
	{
		
		public static GuildController guildController;
		public List<GCLoadGuildList> guildlist;
		GCLoadGuildList loadguildList;
		public List<GCLoadMembersList> memberlist;
		GCLoadMembersList loadmemberList;//成员列表
		GCLoadWageList loadwagelist;//工资列表
		public List<GCLoadVerifyList> verifylist;
		GCLoadVerifyList loadverifyList;//验证列表
		public GCLoadGuildInfo guildInfo;  
		public GCLearnGuildSkill learnguildskill;//学习公会技能
		void Awake() {
			guildController = this;
			DontDestroyOnLoad (this);
		}

		//公会列表
		public List<GuildItem> GetGuildList()
		{
			Debug.Log ("GetGuildList??");
			List<GuildItem> list = new List<GuildItem> ();
			for (int i = 0; i<loadguildList.GuildItemList.Count; i++) 
			{
				list.Add(loadguildList.GuildItemList[i]);
			}
			return list;
		}
		
		public List<WageItem> GetWageList()
		{
			Debug.Log ("GetWageList??");
			List<WageItem> list = new List<WageItem> ();
			for (int i = 0; i<loadwagelist.WageItemList.Count; i++) 
			{
				list.Add(loadwagelist.WageItemList[i]);
			}
			return list;
		}

		//成员列表
		public List<GuildMemberItem> GetMembersList()
		{
			Debug.Log ("GetMembersList??");
			List<GuildMemberItem> list = new List<GuildMemberItem> ();
			for (int i = 0; i<loadmemberList.GuildMemberItemList.Count; i++) 
			{
				list.Add(loadmemberList.GuildMemberItemList[i]);
			}
			return list;
		}

		//验证列表
		public List<VerifyItem> GetVerifyList()
		{
			Debug.Log ("GetVerifyList??");
			List<VerifyItem> list = new List<VerifyItem> ();
			for (int i = 0; i<loadverifyList.VerifyItemList.Count; i++) 
			{
				list.Add(loadverifyList.VerifyItemList[i]);
			}
			return list;
		}

		/*
		 *  创建公会
		 */ 
		public IEnumerator CreatGuildInfo (string guildName,string guildManifesto)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGCreateGuild.CreateBuilder ();
			load.GuildName = guildName;
			load.GuildManifesto = guildManifesto;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 * 加载公会列表信息
		 */ 
		public IEnumerator LoadGuildListInfo ()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadGuildList.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			loadguildList = (packet.packet.protoBody as GCLoadGuildList);
			
		}

		/*
		 *  创建公会
		 */ 
		public IEnumerator ApplyJoinGuild (int guildId)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGApplyJoinGuild.CreateBuilder ();
			load.GuildId = guildId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 * 获取公会信息
		 */ 
		public IEnumerator LoadGuildInfo ()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadGuildInfo.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			guildInfo = (packet.packet.protoBody as GCLoadGuildInfo);
			
		}

		/*
		 *  修改宣言信息
		 */ 
		public IEnumerator ModifyManifesto (string str)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGModifyManifesto.CreateBuilder ();
			load.GuildManifesto = str;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  注入经验
		 */ 
		public IEnumerator InjectionExperience ()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGInjectionExperience.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  获取工资列表信息
		 */ 
		public IEnumerator LoadWageList()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadWageList.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			loadwagelist = (packet.packet.protoBody as GCLoadWageList);
		}

		/*
		 *  修改发放工资
		 */ 
		public IEnumerator ModifyPayOffWage(List<ModifyWage> wagelist)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGModifyPayOffWage.CreateBuilder ();
			foreach (ModifyWage m in wagelist)
			{
				load.AddModifyWage(m);
			}
			//load.TargetPlayerId = playerId;
			//load.Wage = wage;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  获取成员列表信息
		 */ 
		public IEnumerator LoadMembersList()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadMembersList.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			loadmemberList = (packet.packet.protoBody as GCLoadMembersList);
		}

		/*
		 *  踢出公会
		 */ 
		public IEnumerator DismissMember(int targetId)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGDismissMember.CreateBuilder ();
			load.TargetId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  任命成员
		 */ 
		public IEnumerator AppointMember(int targetId,int duty)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGAppointMember.CreateBuilder ();
			load.TargetPlayerId = targetId;
			load.Duty = duty;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  转让会长
		 */ 
		public IEnumerator DevolveMaster(int targetId)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGDevolveMaster.CreateBuilder ();
			load.TargetPlayerId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  进入公会领地
		 */ 
		public IEnumerator EnterGuildScene()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGEnterGuildScene.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  获取验证列表信息
		 */ 
		public IEnumerator LoadVerifyList()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadVerifyList.CreateBuilder ();
			load.VerifyType = 0;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			loadverifyList = (packet.packet.protoBody as GCLoadVerifyList);
		}

		/*
		 * 审批申请加入公会(批量处理)
		 */ 
		public IEnumerator ApprovalApplyJoinGuild(List<int> mailIds,bool reply)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGApprovalApplyJoinGuild.CreateBuilder ();
			var ids = string.Join (",", mailIds.Select(x=>x.ToString()).ToArray());
			load.Verify = ids;
			load.Reply = reply;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			
            //GCApprovalApplyJoinGuild approvalapplyjoinguild = (packet.packet.protoBody as GCApprovalApplyJoinGuild);//处理单条信息  返回的数据   如果是批量操作 如全部删除 全部同意 全部拒绝  返回结果数据就没有意义
			//approvalapplyjoinguild.VerifyApprovalList
		}

		/*
		 *  退出公会
		 */ 
		public IEnumerator ExitGuild()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGExitGuild.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  解散公会
		 */ 
		public IEnumerator DisbandGuild()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGDisbandGuild.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 *  加载公会技能面板
		 */ 
		public IEnumerator LoadGuildSkills()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadGuildSkills.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			
            //GCLoadGuildSkills guildskills = (packet.packet.protoBody as GCLoadGuildSkills);
			//guildskills.GuildSkillList
		}

		/*
		 * 学习公会技能
		 */ 
		public IEnumerator LearnGuildSkill(int skillId)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLearnGuildSkill.CreateBuilder ();
			load.SkillId = skillId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			learnguildskill = (packet.packet.protoBody as GCLearnGuildSkill);
		}

		/*
		 * 购买公会商店物品
		 */ 
		public IEnumerator BuyGuildShopGoods(int shopId,int count)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGBuyGuildShopGoods.CreateBuilder ();
			load.ShopId = shopId;
			load.Count = count;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			//GCBuyGuildShopGoods buy = (packet.packet.protoBody as GCBuyGuildShopGoods);
			//buy.Donate
		}

		/*
		 * 推送踢出公会
		 */ 
		public void PushDismissMember(GCPushDismissMember dismissmember) {
			
		}
		/*
		 * 推送解散公会
		 */ 
		public void PushDisbandGuild(GCPushDisbandGuild disbandguild) {
			
		}

		/*
		 * 推送公会成员,谁加入了公会
		 */ 
		public void PushPlayerJoinGuild(GCPushPlayerJoinGuild playerjoinguild) {
			
		}
		/*
		 * 推送玩家加入公会成功
		 */ 
		public void PushPlayerJoinGuildSuccess(GCPushPlayerJoinGuildSuccess success) {
			
		}
		/*
		 * 推送通知成为会长
		 */ 
		public void PushDevolveMaster(GCPushDevolveMaster master) {
			
		}
		/*
		 * 推送玩家退出公会信息
		 */ 
		public void PushExitGuild(GCPushExitGuild exitguild) {
			
		}
		/*
		 * 推送公会资源和贡献更新
		 */ 
		public void PushGuildDonateUpdate(GCPushGuildDonateUpdate donateupdate) {
			
		}
	}
}
