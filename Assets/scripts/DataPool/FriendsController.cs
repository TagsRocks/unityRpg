﻿
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class FriendsController : MonoBehaviour
	{
		public static FriendsController friendsController;
		public GCLoadListInfo friendlists;

		public GCLoadVerifyPlayer applyLists;

		public GCProcessInviteFriend inviteLists;

		public GCRecommendOnlinePlayer recommendLists;
		
		public FriendInfo searchInfo;	//通过名字搜索的好友
		public GCAddBlack blackInfo;		//拉黑好友
		public FriendInfo friendinfo;
		void Awake() {
			friendsController = this;
			DontDestroyOnLoad (gameObject);
		}

		//好友列表信息
		public List<FriendInfo> GetFriendsList()
		{
			Debug.Log ("GetFriendsList??");
			List<FriendInfo> list = new List<FriendInfo> ();
			for (int i = 0; i<friendlists.FriendInfoList.Count; i++) 
			{
				list.Add(friendlists.FriendInfoList[i]);
			}
			return list;
		}

		//申请列表信息
		public List<VerifyPlayer> GetApplyList()
		{
			List<VerifyPlayer> list = new List<VerifyPlayer> ();
			for (int i = 0; i<applyLists.VerifyPlayerList.Count; i++) 
			{
				list.Add(applyLists.VerifyPlayerList[i]);
			}
			return list;
		}

		//推荐好友列表
		public List<FriendInfo> GetRecommendList()
		{
			List<FriendInfo> list = new List<FriendInfo> ();
			for (int i = 0; i<recommendLists.FriendInfoList.Count; i++) 
			{
				list.Add(recommendLists.FriendInfoList[i]);
			}
			return list;
		}

		/*
		 * 加载 列表信息    分为4种类型的请求 
		 * 					NONE_RELATION  0  未确定关系（即申请列表） 
		 * 					FRIENDLY_RELATION 1 好友列表 
		 * 					ENEMY_RELATION  2 仇人列表 
		 * 					BLACK_RELATION  3 黑名单
		 */ 
		public IEnumerator LoadList(FriendType type) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGLoadListInfo.CreateBuilder ();
			list.FriendType = type;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));
			friendlists = (packet.packet.protoBody as GCLoadListInfo);
		}

		/*
		 * 加对方为好友 邀请好友   ??
		 */ 
		public IEnumerator InviteFriend(int targetId) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGInviteFriend.CreateBuilder ();
			list.TargetId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));
		}

		/*
		 * 处理别人的好友请求
		 */ 
		public IEnumerator ProcessInvite(bool accept, List<int> inviteId) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGProcessInviteFriend.CreateBuilder ();
			list.Accept = accept;
			foreach (int i in inviteId) {
				list.AddInviteId(i);
			}
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));
			inviteLists = (packet.packet.protoBody as GCProcessInviteFriend);
			//inviteLists.FriendInfoList
		}
		/*
		 * 删除好友
		 */ 
		public IEnumerator DelFriend(int targetId) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGDelFriend.CreateBuilder ();
			list.TargetId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));

		}

		/*
		 * 拉黑好友
		 */ 
		public IEnumerator AddBlack(int targetId) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGAddBlack.CreateBuilder ();
			list.TargetId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));
			//有返回   GCAddBlack
			GCAddBlack black = (packet.packet.protoBody as GCAddBlack);
			//black.f
		}

		/*
		 * 删除黑名单
		 */ 
		public IEnumerator DelBlack(int targetId) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGDelBlack.CreateBuilder ();
			list.TargetId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));	
		}

		/*
		 * 移除仇人
		 */ 
		public IEnumerator DelEnemy(int targetId) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGDelEnemy.CreateBuilder ();
			list.TargetId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));	
		}

		/*
		 * 通过名字查找英雄
		 */ 
		public IEnumerator SearchPlayer(string searchName) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGSearchPlayer.CreateBuilder ();
			list.SearchName = searchName;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));	
			searchInfo = (packet.packet.protoBody as GCSearchPlayer).FriendInfo;
		}

		/*
		 * 送体力
		 */ 
		public IEnumerator GivePhysical(int targetId) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGPresentPower.CreateBuilder (); 
			list.FriendsId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));	
		}

		/*
		 * 获取推荐在线好友
		 */ 
		public IEnumerator RecommendOnlinePlayer() {
			var packet = new KBEngine.PacketHolder ();
			var list = CGRecommendOnlinePlayer.CreateBuilder (); 
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));	
			recommendLists = (packet.packet.protoBody as GCRecommendOnlinePlayer);
			//recommendLists.FriendInfoList
		}

		/*
		 * 获取验证信息
		 */ 
		public IEnumerator LoadVerifyPlayer() {
			var packet = new KBEngine.PacketHolder ();
			var list = CGLoadVerifyPlayer.CreateBuilder (); 
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));	
			applyLists = (packet.packet.protoBody as GCLoadVerifyPlayer);
			//applyLists.VerifyPlayerList 
		}

		/*
		 * 删除验证信息
		 */ 
		public IEnumerator DelVerify(int targetId) {
			var packet = new KBEngine.PacketHolder ();
			var list = CGDelVerify.CreateBuilder (); 
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));	
		}

		/*
		 * 推送添加好友
		 */ 
		public void PushAddFriend(GCPushAddFriend addFriend) {

		}

		/*
		 * 推送好友 邀请
		 */ 
		public void PushFriendInvited(GCPushFriendInvited invite) {
			
		}


		/*
		 * 别人 接受或者拒绝好友邀请
		 */ 
		public void PushFriendInvitedResult(GCPushFriendInvitedResult result) {
			
		}

		/*
		 * 好友在线状态
		 */ 
		public void OnlineState(GCPushFriendOnlineState state) {
		}
		public void FriendDelMe(GCPushFriendDeleted del) {
		}
		public void LevelChange(GCPushFriendLevelChange level) {
		}
		public void FriendDelete(GCPushFriendDeleted del) {
		}
		public void AddHated(GCPushFriendAddHatred hate) {
		}
	}
}
