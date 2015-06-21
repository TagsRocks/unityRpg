using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Google.ProtocolBuffers;
using SimpleJSON;
using System;

namespace ChuMeng
{
	public class MyCon
	{
		public Socket connect;
		public bool isClose = false;

		public MyCon (Socket s)
		{
			connect = s;
		}
	}

	public class ServerThread
	{
		int playerId = 100;
		//index 1 - 25
		string packInf = @"
[
	{""id"": 4, ""baseId"" : 11, ""index"" : 3, ""cdTime"" : 0, ""goodsType"" : 0, ""count"" : 1},

    {""id"": 5, ""baseId"" : 12, ""index"" : 4, ""cdTime"" : 0, ""goodsType"" : 0, ""count"" : 1},
    

    {""id"": 6, ""baseId"" : 13, ""index"" : 5, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},    
    {""id"": 7, ""baseId"" : 14, ""index"" : 6, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
    {""id"": 8, ""baseId"" : 15, ""index"" : 7, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
    {""id"": 9, ""baseId"" : 16, ""index"" : 8, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},    
    {""id"": 10, ""baseId"" : 17, ""index"" : 9, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},


    {""id"": 11, ""baseId"" : 18, ""index"" : 10, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
    {""id"": 12, ""baseId"" : 19, ""index"" : 11, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
    {""id"": 13, ""baseId"" : 20, ""index"" : 12, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
    {""id"": 14, ""baseId"" : 21, ""index"" : 13, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
    {""id"": 15, ""baseId"" : 22, ""index"" : 14, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},


    {""id"": 16, ""baseId"" : 24, ""index"" : 15, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},

    {""id"": 19, ""baseId"" : 26, ""index"" : 18, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},

    {""id"": 20, ""baseId"" : 23, ""index"" : 19, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	
	
	
	{""id"": 21, ""baseId"" : 78, ""index"" : 20, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 22, ""baseId"" : 79, ""index"" : 21, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 23, ""baseId"" : 80, ""index"" : 22, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 24, ""baseId"" : 81, ""index"" : 23, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	
	
	{""id"": 25, ""baseId"" : 85, ""index"" : 24, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
]

";

		/*
{""id"": 21, ""baseId"" : 28, ""index"" : 20, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 22, ""baseId"" : 29, ""index"" : 21, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 23, ""baseId"" : 30, ""index"" : 22, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 24, ""baseId"" : 31, ""index"" : 23, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 25, ""baseId"" : 32, ""index"" : 24, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},

{""id"": 21, ""baseId"" : 33, ""index"" : 20, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 22, ""baseId"" : 34, ""index"" : 21, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 23, ""baseId"" : 35, ""index"" : 22, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 24, ""baseId"" : 36, ""index"" : 23, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 25, ""baseId"" : 37, ""index"" : 24, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},

{""id"": 21, ""baseId"" : 38, ""index"" : 20, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 22, ""baseId"" : 39, ""index"" : 21, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 23, ""baseId"" : 40, ""index"" : 22, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 24, ""baseId"" : 41, ""index"" : 23, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 25, ""baseId"" : 42, ""index"" : 24, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},

{""id"": 21, ""baseId"" : 43, ""index"" : 20, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 22, ""baseId"" : 44, ""index"" : 21, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 23, ""baseId"" : 45, ""index"" : 22, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 24, ""baseId"" : 46, ""index"" : 23, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 25, ""baseId"" : 47, ""index"" : 24, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},


{""id"": 21, ""baseId"" : 48, ""index"" : 20, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 22, ""baseId"" : 49, ""index"" : 21, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 23, ""baseId"" : 50, ""index"" : 22, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 24, ""baseId"" : 51, ""index"" : 23, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 25, ""baseId"" : 52, ""index"" : 24, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},

{""id"": 21, ""baseId"" : 63, ""index"" : 20, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 22, ""baseId"" : 64, ""index"" : 21, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 23, ""baseId"" : 65, ""index"" : 22, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 24, ""baseId"" : 66, ""index"" : 23, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
	{""id"": 25, ""baseId"" : 67, ""index"" : 24, ""cdTime"" : 0, ""goodsType"" : 1, ""count"" : 1},
		 */ 
		int selectPlayerJob = 4;
		MyCon con;
		Socket socket;
		ChuMeng.ServerMsgReader msgReader = new ServerMsgReader ();
		List<byte[]> msgBuffer = new List<byte[]> ();

		public ServerThread ()
		{
			socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//socket.SetSocketOption (System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, );
			socket.SetSocketOption (System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			IPEndPoint ip = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 20000);

			socket.Bind (ip);
			socket.Listen (1);

			msgReader.msgHandle = handleMsg;
		}


        public void SendPacket (IBuilderLite retpb, uint flowId, int errorCode)
        {
            var bytes = ServerBundle.sendImmediateError (retpb, flowId, errorCode);
            Debug.Log ("DemoServer: Send Packet " + flowId);
            lock (msgBuffer) {
                msgBuffer.Add (bytes);
            }
        }

		public void SendPacket (IBuilderLite retpb, uint flowId)
		{
			var bytes = ServerBundle.MakePacket(retpb, flowId);
			Debug.Log ("DemoServer: Send Packet " + flowId);
			lock (msgBuffer) {
				msgBuffer.Add (bytes);
			}
		}

		void handleMsg (KBEngine.Packet packet)
		{
			var receivePkg = packet.protoBody.GetType ().FullName;
			Debug.Log ("Server Receive " + receivePkg);
			var className = receivePkg.Split (char.Parse (".")) [1];
			IBuilderLite retPb = null;
			uint flowId = packet.flowId;
            bool findHandler = false;

			if (className == "CGAutoRegisterAccount") {
				var au = GCAutoRegisterAccount.CreateBuilder ();
				au.Username = "liyong";
				retPb = au;
			} else if (className == "CGRegisterAccount") {
				var au = GCRegisterAccount.CreateBuilder ();
				retPb = au;
			} else if (className == "CGLoginAccount") {
				var au = GCLoginAccount.CreateBuilder ();
                /*
				var role = RolesInfo.CreateBuilder ();
				role.Name = "刺客";
				role.PlayerId = 101;
				role.Level = 1;
				role.Job = (Job)4;
				au.AddRolesInfos (role);

				role = RolesInfo.CreateBuilder ();
				role.Name = "枪手";
				role.PlayerId = 102;
				role.Level = 1;
				role.Job = (Job)2;
				au.AddRolesInfos (role);

				role = RolesInfo.CreateBuilder ();
				role.Name = "战士";
				role.PlayerId = 103;
				role.Level = 1;
				role.Job = (Job)1;
				au.AddRolesInfos (role);
                */
                var playerInfo = ServerData.Instance.playerInfo;
                if(playerInfo.HasRoles){
                    var role = RolesInfo.CreateBuilder().MergeFrom(playerInfo.Roles);
                    au.AddRolesInfos(role);
                }

				retPb = au;
			} else if (className == "CGSelectCharacter") {
				var inpb = packet.protoBody as CGSelectCharacter;
				if (inpb.PlayerId == 101) {
					selectPlayerJob = 4;
				} else if (inpb.PlayerId == 102) {
					selectPlayerJob = 2;
				} else {
					selectPlayerJob = 1;
				}
				var au = GCSelectCharacter.CreateBuilder ();
				au.TokenId = "12345";
				retPb = au;
			} else if (className == "CGBindingSession") {
				var au = GCBindingSession.CreateBuilder ();
				au.X = 22;
				au.Y = 1;
				au.Z = 17;
				au.Direction = 10;
				au.MapId = 0;
				au.DungeonBaseId = 0;
				au.DungeonId = 0;
				retPb = au;
			} else if (className == "CGEnterScene") {
				var inpb = packet.protoBody as CGEnterScene;
				var au = GCEnterScene.CreateBuilder ();
				au.Id = inpb.Id;
				retPb = au;
			} else if (className == "CGLoadPackInfo") {
				var inpb = packet.protoBody as CGLoadPackInfo;
				var au = GCLoadPackInfo.CreateBuilder ();

				if (inpb.PackType == PackType.DEFAULT_PACK) {
                    var pkInfo = ServerData.Instance.playerInfo.PackInfoList;
                    au.AddRangePackInfo(pkInfo);
                    /*
					int c = 0;
					var pk = new JSONArray();

					foreach (EquipConfigData ed in GameData.EquipConfig) {
						if (ed.job == selectPlayerJob) {
							var pinfo = PackInfo.CreateBuilder ();
							var pkentry = PackEntry.CreateBuilder ();
							pkentry.Id = ed.id;
							pkentry.BaseId = ed.id;
							pkentry.Index = c;
							pinfo.CdTime = 0;
							pkentry.GoodsType = 1;
							pinfo.PackEntry = pkentry.BuildPartial ();
							au.AddPackInfo (pinfo);
							var dict = new JSONClass();
							dict["id"].AsInt = ed.id;
							dict["baseId"].AsInt = ed.id;
							dict["index"].AsInt = c;
							dict["goodsType"].AsInt = 1;
							pk.Add(dict);
							c++;
						}

					}
					packInf = pk.ToString();
                    */
				} else {
					au.PackType = PackType.DRESSED_PACK;
				}

				retPb = au;
			} else if (className == "CGGetCharacterInfo") {
				var inpb = packet.protoBody as CGGetCharacterInfo;
				var au = GCGetCharacterInfo.CreateBuilder ();
				foreach (int l in inpb.ParamKeyList) {
					var att = RolesAttributes.CreateBuilder ();
					var bd = BasicData.CreateBuilder ();
					att.AttrKey = l;
					bd.Indicate = 1;
                    if(l == (int)CharAttribute.CharAttributeEnum.GOLD_COIN) {
                        bd.TheInt32 = ServerData.Instance.playerInfo.Gold;
                    }else {
					    bd.TheInt32 = 120;
                    }
					att.BasicData = bd.BuildPartial ();
					au.AddAttributes (att);
				}
				retPb = au;
			} 
            /*
            else if (className == "CGLoadShortcutsInfo") {
				var au = GCLoadShortcutsInfo.CreateBuilder ();


				if (selectPlayerJob == 2) {
					var sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 0;
					sh.IndexId = 0;
					sh.BaseId = 8;
					sh.Type = 0;
					au.AddShortCutInfo (sh);

					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 1;
					sh.IndexId = 0;
					sh.BaseId = 9;
					sh.Type = 0;
					au.AddShortCutInfo (sh);

					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 2;
					sh.IndexId = 0;
					sh.BaseId = 10;
					sh.Type = 0;
					au.AddShortCutInfo (sh);

					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 3;
					sh.IndexId = 0;
					sh.BaseId = 11;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
				} else if (selectPlayerJob == 4) {
					var sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 0;
					sh.IndexId = 0;
					sh.BaseId = 14;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
					
					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 1;
					sh.IndexId = 0;
					sh.BaseId = 15;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
					
					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 2;
					sh.IndexId = 0;
					sh.BaseId = 16;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
					
					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 3;
					sh.IndexId = 0;
					sh.BaseId = 17;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
				} else {
					var sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 0;
					sh.IndexId = 0;
					sh.BaseId = 3;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
					
					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 1;
					sh.IndexId = 0;
					sh.BaseId = 4;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
					
					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 2;
					sh.IndexId = 0;
					sh.BaseId = 5;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
					
					sh = ShortCutInfo.CreateBuilder ();
					sh.Index = 3;
					sh.IndexId = 0;
					sh.BaseId = 6;
					sh.Type = 0;
					au.AddShortCutInfo (sh);
				}
				retPb = au;
			} 
            */
            else if (className == "CGListBranchinges") {
				var au = GCListBranchinges.CreateBuilder ();
				var bran = Branching.CreateBuilder ();
				bran.Line = 1;
				bran.PlayerCount = 2;
				au.AddBranching (bran);
				retPb = au;
			} else if (className == "CGHeartBeat") {
			
			} 
            else if (className == "CGLoadSaleItems") {
				var au = GCLoadSaleItems.CreateBuilder ();
				retPb = au;
			} else if (className == "CGListAllTeams") {
				var au = GCListAllTeams.CreateBuilder ();
				retPb = au;
			} else if (className == "CGCopyInfo") {
                var pinfo = ServerData.Instance.playerInfo;
                if(pinfo.HasCopyInfos){
                    retPb = GCCopyInfo.CreateBuilder().MergeFrom(pinfo.CopyInfos);
                }else {
                    //First Fetch Login Info
                    var au = GCCopyInfo.CreateBuilder ();
                    var cin = CopyInfo.CreateBuilder ();
                    cin.Id = 3;
                    cin.IsPass = false;
                    au.AddCopyInfo (cin);
                    var msg = au.Build();
                    pinfo.CopyInfos = msg;
                    retPb = GCCopyInfo.CreateBuilder().MergeFrom(msg);
                }

				
			}else if(className == "CGLoadVipLevelGiftReceiveInfo"){
				var au = GCLoadVipLevelGiftReceiveInfo.CreateBuilder();
				var vip = ReceviedReward.CreateBuilder();
				vip.RewardId = 1;
				au.AddReceviedLevelRewards(vip);

				vip = ReceviedReward.CreateBuilder();
				vip.RewardId = 2;
				au.AddReceviedLevelRewards(vip);

				vip = ReceviedReward.CreateBuilder();
				vip.RewardId = 3;
				au.AddReceviedLevelRewards(vip);

				retPb = au;
			}else if( className == "CGLoadVipFreeGiftReceiveInfo"){
				/*
				var au = GCLoadVipFreeGiftReceiveInfo.CreateBuilder();
				var vip = ReceviedReward.CreateBuilder();
				vip.RewardId = 1;
				au.AddReceviedFreeRewards(vip);
				retPb = au;
				*/
			}else if( className == "CGLoadVipInfo"){
				var au = GCLoadVipInfo.CreateBuilder();
				au.VipType = VipType.NONE_VIP;
				au.VipRemainTime = 0;
				au.VipLevel = 4;
				au.VipExp = 0;
				retPb = au;
			}else if( className == "CGLoadTaskList"){
				var au = GCLoadTaskList.CreateBuilder();
				var task = PlayerTask.CreateBuilder();
				task.TaskId = 1;
				task.PlayerTaskId = 1;
				task.PlayerId = 2;
				task.TaskState = 5;
				task.Chain = 6;
				au.AddPlayerTask(task);

				task = PlayerTask.CreateBuilder();
				task.TaskId = 2;
				task.PlayerTaskId = 1;
				task.PlayerId = 2;
				task.TaskState = 5;
				task.Chain = 6;
				au.AddPlayerTask(task);

				task = PlayerTask.CreateBuilder();
				task.TaskId = 3;
				task.PlayerTaskId = 1;
				task.PlayerId = 2;
				task.TaskState = 5;
				task.Chain = 6;
				au.AddPlayerTask(task);

				retPb = au;
			}else if(className == "CGLoadAchievements"){
				var au = GCLoadAchievements.CreateBuilder ();
				var ac = Achievement.CreateBuilder();
				ac.AchievementId = 1000;
				au.AddAchievements(ac);

				ac = Achievement.CreateBuilder();
				ac.AchievementId = 1001;
				au.AddAchievements(ac);

				ac = Achievement.CreateBuilder();
				ac.AchievementId = 1002;
				au.AddAchievements(ac);

				ac = Achievement.CreateBuilder();
				ac.AchievementId = 1003;
				au.AddAchievements(ac);

				ac = Achievement.CreateBuilder();
				ac.AchievementId = 1004;
				au.AddAchievements(ac);
				retPb = au;
			}else if (className == "CGAuctionInfo") {
				var au = GCAuctionInfo.CreateBuilder ();
				au.MaxSize = 1;
				au.PageSize = 6;
				var item = AuctionItem.CreateBuilder ();
				item.Id = 11;
				item.BaseId = 14;
				item.Type = 1;
				item.SellCount = 2;
				item.RemainTime = 60;
				item.TotalCost = 88;
				au.AddAuctionItems (item);
				
				item = AuctionItem.CreateBuilder ();
				item.Id = 119;
				item.BaseId = 15;
				item.Type = 1;
				item.SellCount = 2;
				item.RemainTime = 609;
				item.TotalCost = 889;
				au.AddAuctionItems (item);
				
				item = AuctionItem.CreateBuilder ();
				item.Id = 118;
				item.BaseId = 16;
				item.Type = 1;
				item.SellCount = 2;
				item.RemainTime = 608;
				item.TotalCost = 888;
				au.AddAuctionItems (item);

				item = AuctionItem.CreateBuilder ();
				item.Id = 117;
				item.BaseId = 23;
				item.Type = 1;
				item.SellCount = 2;
				item.RemainTime = 607;
				item.TotalCost = 887;
				au.AddAuctionItems (item);

				item = AuctionItem.CreateBuilder ();
				item.Id = 116;
				item.BaseId = 24;
				item.Type = 1;
				item.SellCount = 2;
				item.RemainTime = 605;
				item.TotalCost = 886;
				au.AddAuctionItems (item);

				item = AuctionItem.CreateBuilder ();
				item.Id = 115;
				item.BaseId = 25;
				item.Type = 1;
				item.SellCount = 2;
				item.RemainTime = 605;
				item.TotalCost = 88;
				au.AddAuctionItems (item);
				retPb = au;
			} else if (className == "CGUserDressEquip") {
				var inpb = packet.protoBody as CGUserDressEquip;
				var au = GCUserDressEquip.CreateBuilder ();
				var pk = SimpleJSON.JSONNode.Parse (packInf).AsArray;
				foreach (JSONNode n in pk) {
					if (n ["id"].AsInt == inpb.SrcEquipId) {
						var dress = PackEntry.CreateBuilder ();
						dress.Id = n ["id"].AsInt;
						dress.BaseId = n ["baseId"].AsInt;
						dress.GoodsType = n ["goodsType"].AsInt;
						dress.Count = 1;
						dress.Index = n ["index"].AsInt;
						au.DressEquip = dress.BuildPartial ();
						break;
					}
				}
				retPb = au;
			} else if (className == "CGAutoRegisterAccount") {
				var au = GCAutoRegisterAccount.CreateBuilder ();
				au.Username = "liyong_"+UnityEngine.Random.Range(0, 1000);
				retPb = au;
			} else if (className == "CGRegisterAccount") {
                var inpb = packet.protoBody as CGRegisterAccount;
                ServerData.Instance.playerInfo.Username = inpb.Username;

				var au = GCRegisterAccount.CreateBuilder ();
				retPb = au;
			} else if (className == "CGCreateCharacter") {

				var inpb = packet.protoBody as CGCreateCharacter;

				var au = GCCreateCharacter.CreateBuilder ();
				var role = RolesInfo.CreateBuilder ();
				role.Name = inpb.Username;
				role.PlayerId = playerId;
				playerId++;
				role.Level = 1;
				role.Job = inpb.Job;
                var msg = role.Build();
                ServerData.Instance.playerInfo.Roles = msg;

                au.AddRolesInfos (msg);
            
				retPb = au;
			} 
            else if (className == "CGPlayerMove") {
				var au = GCPlayerMove.CreateBuilder ();
				retPb = au;
            }else {
                var fullName = packet.protoBody.GetType().FullName;
                var handlerName = fullName.Replace("ChuMeng", "ServerPacketHandler");

                var tp = Type.GetType(handlerName);
                if(tp == null) {
                    Debug.LogError("PushMessage noHandler "+handlerName);
                }else {
                    findHandler = true;
                    var ph = (ServerPacketHandler.IPacketHandler)Activator.CreateInstance(tp);
                    KBEngine.KBEngineApp.app.queueInLoop(
                        delegate() {
                            ph.HandlePacket(packet);
                       });
                }
            }
		

			if (retPb != null) {
				SendPacket (retPb, flowId);
			} else {
                if(className != "CGHeartBeat" && !findHandler) {
				   Debug.LogError ("DemoServer::not Handle Message " + className);
                }
			}
		}

		void SendThread ()
		{
			bool conOK = true;
			while (conOK && !DemoServer.demoServer.stop) {
				lock (msgBuffer) {
					while (msgBuffer.Count > 0) {
						con.connect.Send (msgBuffer [0]);
						msgBuffer.RemoveAt (0);
					}
				}
				Thread.Sleep (100);
			}
		}

		public void run ()
		{
			Debug.Log ("Start Demo Server");
			byte[] buffer = new byte[1024];
			while (!DemoServer.demoServer.stop) {
				var connect = socket.Accept ();
				con = new MyCon (connect);
				var sendThread = new Thread (new ThreadStart (SendThread));
				sendThread.Start ();

				lock (msgBuffer) {
					msgBuffer.Clear ();
				}

				while (!DemoServer.demoServer.stop) {
					int num = connect.Receive (buffer);
					if (num > 0) {
						msgReader.process (buffer, (uint)num);
					} else {

					}
				}
			}

			Debug.Log ("DemoServer Stop");
		}
	}

	public class DemoServer
	{
		public static DemoServer demoServer;
		public bool stop = false;
        ServerData serverData;
        ServerThread sth;
        public ServerThread GetThread(){
            return sth;
        }
        public ServerData GetServerData(){
            return serverData;
        }

		public DemoServer ()
		{
			demoServer = this;
            serverData = new ServerData();
            serverData.LoadData();

			Debug.Log ("Init DemoServer");
			sth = new ServerThread ();
			var t = new Thread (new ThreadStart (sth.run));
			t.Start ();

		}

		public void ShutDown ()
		{
            serverData.SaveUserData();
			stop = true;
		}
	}
}
