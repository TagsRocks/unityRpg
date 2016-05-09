
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using Google.ProtocolBuffers;

namespace KBEngine
{
    using UnityEngine;
    using System;
    using System.Net.Sockets;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;


    using MessageModuleID = System.SByte;
    using MessageID = System.UInt16;
    using MessageLength = System.UInt32;

    public delegate void MessageHandler(Packet msg);

    public class NetworkInterface
    {
        //private Socket socket_ = null;
        private MessageReader msgReader = new MessageReader();
        private static byte[] _datas = new byte[MemoryStream.BUFFER_MAX];

        public Dictionary<uint, MessageHandler> flowHandlers = new Dictionary<uint, MessageHandler>();

        public NetworkInterface(KBEngineApp app)
        {
            msgReader.mainLoop = KBEngine.KBEngineApp.app;
        }

        public void reset()
        {
			
            //socket_ = null;
            msgReader = new MessageReader();
            msgReader.mainLoop = KBEngineApp.app;
        }
		
        public bool valid()
        {
            //return ((socket_ != null) && (socket_.Connected == true));
            return true;
        }

        void DumpHandler(Packet p)
        {
        }

        public void send(byte[] datas, MessageHandler handler, uint flowId)
        {
            if (datas == null || datas.Length == 0)
            {
                throw new ArgumentException("invalid datas!");
            }
            if (handler == null)
            {
                flowHandlers [flowId] = DumpHandler;
            } else
            {
                flowHandlers [flowId] = handler;
            }
            MyLib.DemoServer.demoServer.GetThread().ReceivePacket(datas);
        }
        private  Queue<byte[]> packets = new Queue<byte[]>();
        public void ReceivePacket(byte[] d) {
            packets.Enqueue(d);
        }

        private void recv() {
            while(packets.Count > 0) {
                var data = packets.Dequeue();
                msgReader.process(data, (MessageLength)data.Length, flowHandlers);
            }
        }


        public void process()
        {
            recv();
        }
    }
}
