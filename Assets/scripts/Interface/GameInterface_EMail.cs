
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
	 * 邮件数据UI相关接口
	 * 
	 */
    public class GameInterface_EMail
	{
        private static GameInterface_EMail instance;
        private int EMailNum = 6;
		private List<Mail> maillistinfo;
        public static GameInterface_EMail GetInstance()
        {
            if (instance == null)
            {
                instance = new GameInterface_EMail();
            }
            return instance;
        }

		public GameInterface_EMail()
		{
			//maillistinfo = MailController.mailController.GetMailsList ();
		}
		
		public bool IsAttach(int EmailId)
		{
			foreach(Mail m in maillistinfo)
			{
				if(m.MailId == EmailId)
				{
					return m.IsAttach;
				}
			}
			return false;
		}
		
		public void DelMail(List<int> _mailIds) {
			for (int i = 0; i < _mailIds.Count; i++)
			{
				for (int j = 0; j < maillistinfo.Count; j++)
				{
					if (_mailIds[i] == maillistinfo[j].MailId)
					{
						maillistinfo.RemoveAt(j);
					}
				}
			}
		}

		public List<Mail> GetMailListInfo()
		{
			maillistinfo = MailController.mailController.GetMailsList ();
			return maillistinfo;
		}


		public GCReadMail GetreamMailInfo()
		{
			return MailController.mailController.GetreadMails();
		}

		public EMailData GetSingleEmaildata()
		{
			EMailData data = new EMailData ();
			data.userMailId =  Convert.ToInt32(GetreamMailInfo ().UserMailId);
			foreach(Mail m in maillistinfo)
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
		}

		public void LoadEmail()
		{
			Debug.Log ("MailController.mailController::"+MailController.mailController);
			MailController.mailController.StartCoroutine(MailController.mailController.LoadMail());
		}

		/*
		 * 获取邮件数量
		 */ 
		public int GetEMailNum() {
			EMailNum = MailController.mailController.EMailNum;
            return EMailNum;
		}

		/*
		 * 获取玩家的好友列表
		 */ 
		public List<string> GetName() {
            List<string> namelist = new List<string>();
            namelist.Add("Joe");
            namelist.Add("Lily");
            namelist.Add("Jime");
			return namelist;
		}


        /*
         * 领取附件请求   一键提取
         */
        public void GetAllAnnexRequest()
        {
			MailController.mailController.StartCoroutine(MailController.mailController.ReceiveMailsAllReward());
        }

		/*
         * 领取附件请求   单个提取
         */
		public void GetSingleAnnexRequest(int EMailId)
		{
			Debug.Log("Get Annex Email id:" + EMailId);
			
			MailController.mailController.StartCoroutine(MailController.mailController.ReceiveSingleMail(EMailId));
		}

        /*
         * 读取邮件请求
         */
        public void ReadEMailRequest(int EMailId)
        {
			Debug.Log("Had Readed Email id:"+EMailId);
			MailController.mailController.StartCoroutine(MailController.mailController.ReadMail(EMailId));
        }

        /*
         * 删除单个邮件请求
         */
        public void DelEMailRequest(int EMailId)
        {
            Debug.Log("Del Email id:" + EMailId);
			//
			List<int> delEmail = new List<int> ();
			delEmail.Add (EMailId);
			MailController.mailController.StartCoroutine(MailController.mailController.DelMails(delEmail));
        }

		/*
         * 删除所有邮件请求
         */
		public void DelAllEMailRequest(List<int> EMailListId)
		{
			Debug.Log("Del all Email id:" + EMailListId);
			MailController.mailController.StartCoroutine(MailController.mailController.DelMails(EMailListId));
		}

        /*
         * 发送邮件请求
         */
		public void SendEMailRequest(string receiver, string title, string content, int goldCoin, int goldTicket, int silverCoin, int silverTicket, List<int> itemIds)
		{
			MailController.mailController.StartCoroutine(MailController.mailController.SendMail(receiver,title,content,goldCoin,goldTicket,silverCoin,silverTicket,itemIds));
        }
        

	}
}