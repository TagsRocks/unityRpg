using UnityEngine;
using System.Collections;
using MyLib;

/// <summary>
/// 跨场景的远程网络管理器
/// 1：全局RC 管理
/// 2：重连管理
/// 3：Mapxx 注册有每个场景的一些回调处理函数
/// 4：回调处理多级处理 全局级别 和 场景级别
/// 
/// RemoteManager向下GameObject传递回调处理
/// </summary>
public class  RemoteNetworkManager : MonoBehaviour
{
    public WorldState state = WorldState.Idle;

    public static RemoteNetworkManager Instance;

    public  RemoteClient rc
    {
        get;
        private set;
    }

    private string Server_IP = "127.0.0.1";
    private MainThreadLoop ml;

    void Awake()
    {
        Instance = this;
        ml = gameObject.AddComponent<MainThreadLoop>();
    }

    private string nm;

    public void StartGame(string uname)
    {
        if (state == WorldState.Idle)
        {
            nm = uname;
            StartCoroutine(InitConnect());
        }
    }

    //加载场景 初始化网络 初始化玩家模型等等
    IEnumerator InitConnect()
    {
        state = WorldState.Connecting;
        rc = new RemoteClient(ml);
        rc.evtHandler = EvtHandler;
        rc.msgHandler = MsgHandler;
        rc.Connect(Server_IP, 10001);

        while (lastEvt == RemoteClientEvent.None && state == WorldState.Connecting)
        {
            yield return null;
        }
        if (lastEvt == RemoteClientEvent.Connected)
        {
            state = WorldState.Connected;
            yield return StartCoroutine(InitName());
            yield return StartCoroutine(StartMatch());
            yield return StartCoroutine(LoadScene());
        }
    }

    private int myId = 0;
    public int GetMyId() {
        return myId;
    }

    private IEnumerator  InitName()
    {
        var cg = CGPlayerCmd.CreateBuilder();
        cg.Cmd = "Login2";

        var data = KBEngine.Bundle.GetPacketFull(cg);
        yield return StartCoroutine(rc.SendWaitResponse(data.data, data.fid, (packet) =>
        {
            var proto = packet.protoBody as GCPlayerCmd;
            var cmds = proto.Result.Split(' ');
            myId = System.Convert.ToInt32(cmds [1]);
            //ObjectManager.objectManager.RefreshMyServerId(myId);
        }));
    }

    private RoomInfo roomInfo = null;

    private IEnumerator StartMatch()
    {
        var cg = CGPlayerCmd.CreateBuilder();
        cg.Cmd = "Match2";
        var data = KBEngine.Bundle.GetPacketFull(cg);
        yield return StartCoroutine(rc.SendWaitResponse(data.data, data.fid, (packet) =>
        {
            var cmd = packet.protoBody as GCPlayerCmd;
            roomInfo = cmd.RoomInfo;
        }));
        if (roomInfo != null && roomInfo.Id != -1)
        {
        }
    }


    private IEnumerator LoadScene()
    {
        WorldManager.worldManager.WorldChangeScene(8, false);
        yield break;
    }

    private RemoteClientEvent lastEvt = RemoteClientEvent.None;

    private void EvtHandler(RemoteClientEvent evt)
    {
        Debug.LogError("RemoteClientEvent: " + evt);
        lastEvt = evt;
        if (lastEvt == RemoteClientEvent.Close)
        {
            WindowMng.windowMng.ShowNotifyLog("和服务器断开连接：" + state);
            if (state != WorldState.Idle)
            {
                Debug.LogError("ConnectionClosed But WorldNotClosed");
                state = WorldState.Idle;
                //StartCoroutine(RetryConnect());
            }
        } else if (lastEvt == RemoteClientEvent.Connected)
        {
            WindowMng.windowMng.ShowNotifyLog("连接服务器成功：" + state);
        }
    }

    private void MsgHandler(KBEngine.Packet packet)
    {
    }
}

