
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	/*
	 * 好友数据UI相关接口
	 * 
	 */
	public class GameInterface_Friend
	{
		private static GameInterface_Friend instance;
        private int FriendNum = 6;
		private List<FriendInfo> friendlistsinfo;
		public Dictionary<int,string> friendListNameId = new Dictionary<int, string>();
		public List<string> friendListName = new List<string>();
		public static GameInterface_Friend GetInstance()
        {
            if (instance == null)
            {
				instance = new GameInterface_Friend();
            }
            return instance;
        }

		public GameInterface_Friend()
		{



		}


		public void PackFriendList()
		{
			friendlistsinfo = FriendsController.friendsController.GetFriendsList ();
			foreach (FriendInfo f in friendlistsinfo) 
			{
				friendListNameId.Add(Convert.ToInt32(f.PlayerId),f.PlayerName);
				friendListName.Add(f.PlayerName);
			}
		}
		
		/*public void DelMail(List<int> _mailIds) {
			for (int i = 0; i < _mailIds.Count; i++)
			{
				for (int j = 0; j < friendlistsinfo.Count; j++)
				{
					if (_mailIds[i] == friendlistsinfo[j].MailId)
					{
						friendlistsinfo.RemoveAt(j);
					}
				}
			}
		}*/

		public List<FriendInfo> GetMailListInfo()
		{
			friendlistsinfo = FriendsController.friendsController.GetFriendsList ();
			return friendlistsinfo;
		}


		public GCReadMail GetreamMailInfo()
		{
			return MailController.mailController.GetreadMails();
		}

		/*public EMailData GetSingleEmaildata()
		{
			EMailData data = new EMailData ();
			data.userMailId =  Convert.ToInt32(GetreamMailInfo ().UserMailId);
			foreach(Mail m in friendlistsinfo)
			{
				if(m.MailId == data.userMailId)
				{
					data.title = m.Title;
					data.sender = m.Sender;
				}
			}
			data.content = GetreamMailInfo ().Content;
			data.goldCoin = GetreamMailInfo ().GoldCoin;
			data.goldTicket = GetreamMailInfo ().GoldTicket;
			data.silverCoin = GetreamMailInfo ().SilverCoin;
			data.silverTicket = GetreamMailInfo ().SilverTicket;
			//data.leaveTimer = GetreamMailInfo ();
			data.Attachment = GetreamMailInfo ().AttachmentList;
			return data;
		}*/


		/*
		 * 获取好友数量
		 */ 
		public int GetEMailNum() {
			FriendNum = FriendsController.friendsController.GetFriendsList ().Count;
			return FriendNum;
		}

	

		public void GetSingleAnnexRequest(int EMailId)
		{
			Debug.Log("Get Annex Email id:" + EMailId);
			
			//MailController.mailController.StartCoroutine(MailController.mailController.ReceiveSingleMail(EMailId));
		}

       

        /*
         * 删除单个邮件请求
         */
        public void DelEMailRequest(int EMailId)
        {
            
        }

		/*
         * 删除所有邮件请求
         */
		public void DelAllEMailRequest(List<int> EMailListId)
		{
			Debug.Log("Del all Email id:" + EMailListId);
		}

        /*
         * 发送邮件请求
         */
		public void SendEMailRequest(string receiver, string title, string content, int goldCoin, int goldTicket, int silverCoin, int silverTicket, List<int> itemIds)
		{

        }
        

	}
}