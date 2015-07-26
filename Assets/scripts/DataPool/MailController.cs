
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChuMeng
{
	public class MailController : MonoBehaviour
	{
		public static MailController mailController;
		public GCLoadMails mails;
		public GCReadMail readmail;


		//邮件列表信息
		public List<Mail> GetMailsList()
		{
			Debug.Log ("GetMailsList??");
			List<Mail> list = new List<Mail> ();
			for (int i = 0; i<mails.MailList.Count; i++) 
			{
				list.Add(mails.MailList[i]);
			}
			return list;
		}

		//打开邮件信息
		public GCReadMail GetreadMails()
		{
			return readmail;
		}
		//获取邮件数量
		public int EMailNum {
			get {
				Debug.Log("Email num:"+mails);
				return mails.MailCount;
			}
		}

		void Awake() {
			mailController = this;
			Debug.Log ("is true???"+mailController);
			DontDestroyOnLoad (this);
		}

		/*
		 * 加载邮件消息
		 */ 
		public IEnumerator LoadMail() {
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadMails.CreateBuilder ();
			yield return StartCoroutine(KBEngine.Bundle.sendSimple(this, load, packet));
			mails = (packet.packet.protoBody as GCLoadMails);
		}

		/*
		 * 读取一封邮件
		 */ 
		public IEnumerator ReadMail(int EMailId) {
			var packet = new KBEngine.PacketHolder ();
			var read = CGReadMail.CreateBuilder ();
			read.MailId = EMailId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, read, packet));
			readmail = (packet.packet.protoBody as GCReadMail);
		}

		/*
		 * 删除一封邮件
		 */ 
		public IEnumerator DelMails(List<int> mailIds) {
			var ids = string.Join (",", mailIds.Select(x=>x.ToString()).ToArray());
			var packet = new KBEngine.PacketHolder ();
			var del = CGDelMails.CreateBuilder ();
			del.MailIds = ids; 
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, del, packet));
			//mailList.DelMail (mailIds);
		}

		/*
		 * 发送邮件
		 */ 
		public IEnumerator SendMail(string receiver, string title, string content, int goldCoin, int goldTicket, int silverCoin, int silverTicket, List<int> itemIds) {
			var packet = new KBEngine.PacketHolder ();
			var send = CGSendMail.CreateBuilder ();
			send.Receiver = receiver;
			send.Title = title;
			send.Content = content;
			//send.GoldCoin = 0;
			//send.GoldTicket = 0;
			send.SilverCoin = silverCoin;
			if (itemIds != null)
			{
				foreach (int i in itemIds) {
					var att = SendSingleAttachment.CreateBuilder ();
					att.UserPropId = i;
					att.UserPropType = 0;
					send.AddSendSingleAttachment (att);
				}
			}

			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, send, packet));

			//扣除银币和 道具
			//BackPack.backpack.SendMail (silverCoin, itemIds);
		}


		/*
		 * 收取所有邮件  里面的附件
		 */ 
		public IEnumerator ReceiveMailsAllReward() {
			var packet = new KBEngine.PacketHolder ();
			var rec = CGReceiveMailsAllReward.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, rec, packet));

			//Clear Mail State
			//BackPack.backpack.ReceiveAll (packet.packet.protoBody as GCReceiveMailsAllReward);
		}

		/*
		 * 收取单个邮件 附件
		 */ 
		public IEnumerator ReceiveSingleMail(int mailId) {
			var packet = new KBEngine.PacketHolder ();
			var rec = CGReceiveSingleMailAllReward.CreateBuilder ();
			rec.MailId = mailId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, rec, packet));

			//mailList.ReceiveSingle (mailId);
		}

		/*
		 * 收到服务器推送的邮件
		 */ 
		public void ReceiveMail(GCPushMail mail) {
			//mailList.ReceiveMail (mail);
		}
	}

}
