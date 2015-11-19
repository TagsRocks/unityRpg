using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;
using System.Collections.Generic;

namespace ChuMeng
{
    public class MsgBuffer
    {
        public int position = 0;
        public byte[] buffer;

        public int Size
        {
            get
            {
                return buffer.Length - position;
            }
        }
    }
    public class RemoteClient
    {
        uint myId = 0;
        byte[] mTemp = new byte[8192];
        KBEngine.MessageReader msgReader = new KBEngine.MessageReader();

        Socket mSocket;
        IPEndPoint endPoint;
        public bool IsClose = false;
        List<MsgBuffer> msgBuffer = new List<MsgBuffer>();
        public KBEngine.MessageHandler msgHandler;

        public RemoteClient(IMainLoop loop) {
            msgReader.msgHandle = HandleMsg;
            msgReader.mainLoop = loop; 
        }

        void HandleMsg(KBEngine.Packet packet) {
            Debug.LogError("HandlerMsg "+packet.protoBody);
            if(msgHandler != null) {
                msgHandler(packet);
            }
            if(packet.protoBody.GetType() == typeof(GCPlayerCmd)) {
                var proto = packet.protoBody as GCPlayerCmd;
                var cmds = proto.Result.Split(' ');
                if(cmds[0] == "Login") {
                    myId = Convert.ToUInt32(cmds[1]);
                }
            }
        }

        public void Connect(string ip1, int port1)
        {
            endPoint = new IPEndPoint(IPAddress.Parse(ip1), port1);
            try
            {
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var result = mSocket.BeginConnect(endPoint, new AsyncCallback(OnConnectResult), null);
                var th = new Thread(CancelConnect);
                th.Start(result);
            } catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }
        public void StartReceive() {
            Debug.LogError("StartReceive");
            try {
                mSocket.BeginReceive(mTemp, 0, mTemp.Length, SocketFlags.None, OnReceive, null);
            }catch(Exception exception) {
                Debug.LogError(exception.ToString());
                Close();
            }
        }

        void OnReceive(IAsyncResult result) {
            int bytes = 0;
            try {
                bytes = mSocket.EndReceive(result);
            }catch(Exception exception){
                Debug.LogError(exception.ToString());
                Close();
            }
            Debug.LogError("OnReceive "+bytes);
            if(bytes <= 0){
                Close();
            }else {
                uint num = (uint)bytes;
                msgReader.process(mTemp, num, null);
                try {
                    mSocket.BeginReceive(mTemp, 0, mTemp.Length, SocketFlags.None, OnReceive, null);
                }catch(Exception exception){
                    Debug.LogError(exception.ToString());
                    Close();
                }
            }
        }

        void OnConnectResult(IAsyncResult result)
        {
            if (mSocket == null)
            {
                return;
            }
            bool success = false;
            try
            {
                mSocket.EndConnect(result);
                success = true;

            } catch (Exception exception)
            {
                mSocket.Close();
                mSocket = null;
                Debug.LogError(exception.Message);
                success = false;
            }
            if (success)
            {
                Debug.LogError("Connect Success");

            } else
            {
                Close();
            }
        }
        public void Disconnect() {
            Close();
        }
        void Close()
        {
            if (IsClose)
            {
                return;
            }
            if (mSocket != null && mSocket.Connected)
            {
                try
                {
                    mSocket.Shutdown(SocketShutdown.Both);
                    mSocket.Close();
                } catch (Exception exception)
                {
                    Debug.LogError(exception.ToString());
                }
            }
            if (mSocket != null)
            {
                try
                {
                    mSocket.Close();
                } catch (Exception exception)
                {
                    Debug.LogError(exception.ToString());
                }
            }
            mSocket = null;
            IsClose = true;
        }

        void CancelConnect(object obj)
        {
            var res = (IAsyncResult)obj;
            if (res != null && !res.AsyncWaitHandle.WaitOne(3000, true))
            {
                Debug.LogError("ConnectError");
                try
                {
                    if (mSocket != null)
                    {
                        mSocket.Close();
                        mSocket = null;
                    }
                } catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                }
                Close();
            }else {
                StartReceive();
            }
        }

        public void Send(byte[] data)
        {
            lock (msgBuffer)
            {
                var mb = new MsgBuffer(){position=0, buffer=data};
                msgBuffer.Add(mb);
                if (msgBuffer.Count == 1)
                {
                    try
                    {
                        mSocket.BeginSend(mb.buffer, mb.position, mb.Size, SocketFlags.None, OnSend, null);
                    } catch (Exception exception)
                    {
                        Debug.LogError(exception.ToString());
                        Close();
                    }
                }
            }
        }

        void OnSend(IAsyncResult result)
        {
            int num = 0;
            try
            {
                num = mSocket.EndSend(result);
            } catch (Exception exception)
            {
                num = 0;
                Close();
                Debug.LogError(exception.ToString());
                return;
            }
            lock (msgBuffer)
            {
                if (mSocket != null && mSocket.Connected)
                {
                    var mb = msgBuffer [0];
                    MsgBuffer nextBuffer = null;
                    if (mb.Size <= num)
                    {
                        msgBuffer.RemoveAt(0);
                        if (msgBuffer.Count > 0)
                        {
                            nextBuffer = msgBuffer [0];
                        }
                    } else if (mb.Size > num)
                    {
                        mb.position += num;
                        nextBuffer = mb;
                    }
                    if (nextBuffer != null)
                    {
                        try
                        {
                            mSocket.BeginSend(nextBuffer.buffer, nextBuffer.position, nextBuffer.Size, SocketFlags.None, OnSend, null);
                        } catch (Exception exception)
                        {
                            Debug.LogError(exception.ToString());
                            Close();
                        }
                    }
                }
            }
        }



    }

}