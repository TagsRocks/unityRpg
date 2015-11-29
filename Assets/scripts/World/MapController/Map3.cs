using UnityEngine;
using System.Collections;
using System;

namespace ChuMeng
{
    public enum WorldState {
        Idle,
        Connecting,
        Connected,
        Closed,
    }
    /// <summary>
    /// Idle
    /// Connecting
    /// Connected
    /// Closed
    /// </summary>
    public class Map3 : CScene
    {
        private int  myId = 0;
        MainThreadLoop ml;
        RemoteClient rc;
        private WorldState _s = WorldState.Idle;
        public WorldState state{
            get {
                return _s;
            }
            set {
                if(_s != WorldState.Closed) {
                    _s = value;
                }else {
                    Debug.LogError("WorldHasQuit Not: "+value);
                }
            }
        }

        public override bool IsNet
        {
            get
            { 
                return true;
            }
        }

        protected override void Awake() {
            base.Awake();
            ml = gameObject.AddComponent<MainThreadLoop>();
            rc = new RemoteClient(ml);
            rc.evtHandler = EvtHandler;
            rc.msgHandler = MsgHandler;
            state = WorldState.Connecting;
            StartCoroutine(InitConnect());
        }

        /// <summary>
        /// 协程改造的Connect和Send函数 
        /// </summary>
        /// <returns>The connect.</returns>
        IEnumerator InitConnect() {
            rc.Connect("127.0.0.1", 10001);
            while(lastEvt == RemoteClientEvent.None && state == WorldState.Connecting) {
                yield return new WaitForSeconds(1);
            }
            if(lastEvt == RemoteClientEvent.Connected) {
                state = WorldState.Connected;
                yield return StartCoroutine(InitData());       
                yield return StartCoroutine(SendCommandToServer());
            }
        }


        void SyncMyPos() {
            var me = ObjectManager.objectManager.GetMyPlayer();
            if(me == null) {
                return;
            }
            var pos = me.transform.position;

            var cg = CGPlayerCmd.CreateBuilder();
            cg.Cmd = "UpdateData";
            var ainfo = AvatarInfo.CreateBuilder();
            ainfo.X = (int)(pos.x*100);
            ainfo.Z = (int)(pos.z*100);
            ainfo.Y = (int)(pos.y*100);
            cg.AvatarInfo = ainfo.Build();

            BroadcastMsg(cg);
        }

        IEnumerator SendCommandToServer() {
            while(state == WorldState.Connected) {
                SyncMyPos();
                yield return new WaitForSeconds(0.5f);
            }
        }


        void SendUserData() {
            if(state != WorldState.Connected) {
                return;
            }
            var me = ObjectManager.objectManager.GetMyPlayer();
            var pos = me.transform.position;

            var cg = CGPlayerCmd.CreateBuilder();
            cg.Cmd = "InitData";
            var ainfo = AvatarInfo.CreateBuilder();
            ainfo.X = (int)(pos.x*100);
            ainfo.Z = (int)(pos.z*100);
            ainfo.Y = (int)(pos.y*100);
            var pinfo = ServerData.Instance.playerInfo;
            foreach(var d in pinfo.DressInfoList) {
                ainfo.DressInfoList.Add(d);
            }
            ainfo.Level = ObjectManager.objectManager.GetMyProp(CharAttribute.CharAttributeEnum.LEVEL);

            cg.AvatarInfo = ainfo.Build();
            var data = KBEngine.Bundle.GetPacket(cg);
            rc.Send(data);
        }

        IEnumerator InitData() {
            var cg = CGPlayerCmd.CreateBuilder();
            cg.Cmd = "Login";
            var data = KBEngine.Bundle.GetPacket(cg);
            rc.Send(data);

            while(myId == 0 && state == WorldState.Connected) {
                yield return new WaitForSeconds(1);
            }
            SendUserData();
        }

        private RemoteClientEvent lastEvt = RemoteClientEvent.None;
        void EvtHandler (RemoteClientEvent evt) {
            Debug.LogError("RemoteClientEvent: "+evt);
            lastEvt = evt;
            if(lastEvt == RemoteClientEvent.Close) {
                if(state != WorldState.Closed) {
                    Debug.LogError("ConnectionClosed But WorldNotClosed");
                    state = WorldState.Idle;
                    StartCoroutine(RetryConnect());
                }
            }
        }

        /// <summary>
        /// 断开连接之后重新连接 
        /// </summary>
        /// <returns>The connect.</returns>
        IEnumerator RetryConnect() {
            yield return new WaitForSeconds(2);
            //重试是否连接重置用户ID
            lastEvt = RemoteClientEvent.None;
            myId = 0;
            state = WorldState.Connecting;
            if(state == WorldState.Connecting) {
                StartCoroutine(InitConnect());
            }
        }


        void MsgHandler(KBEngine.Packet packet) {
            var proto = packet.protoBody as GCPlayerCmd;
            Log.Net("Map3Receive: "+proto);
            var cmds = proto.Result.Split(' ');
            if(cmds[0] == "Login") {
                myId = Convert.ToInt32(cmds[1]);
                ObjectManager.objectManager.RefreshMyServerId(myId);
            }else if(cmds[0] == "Add") {
                ObjectManager.objectManager.CreateOtherPlayer(proto.AvatarInfo);
                PlayerDataInterface.DressEquip(proto.AvatarInfo);

            }else if(cmds[0] == "Remove") {
                ObjectManager.objectManager.DestroyPlayer(proto.AvatarInfo.Id); 
            }else if(cmds[0] == "Update") {
                var player = ObjectManager.objectManager.GetPlayer(proto.AvatarInfo.Id);
                if(player != null) {
                    var sync = player.GetComponent<PlayerSync>();
                    sync.NetworkMove(proto.AvatarInfo);
                }
            }else if(cmds[0] == "Damage") {
                SkillDamageCaculate.DoNetworkDamage(proto);
            }
        }

        void QuitWorld() {
            state = WorldState.Closed;
            rc.evtHandler = null;
            rc.msgHandler = null;
            rc.Disconnect();
        }

        protected override void  OnDestroy() {
            base.OnDestroy();
            QuitWorld();
        }

        public override void BroadcastMsg(CGPlayerCmd.Builder cg)
        {
            var data = KBEngine.Bundle.GetPacket(cg);
            rc.Send(data);
        }
    }

}