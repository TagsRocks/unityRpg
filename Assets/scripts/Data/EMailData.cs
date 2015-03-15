
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ChuMeng
{
	public class EMailData  {
		public int userMailId;
		public string sender;
		public string title;
		public string content;
		public int goldCoin;
		public int goldTicket;
		public int silverCoin;
		public int silverTicket;
		public int leaveTimer;
		public IList<Attachment> Attachment;
		public EMailData()
		{

		}

	}

}