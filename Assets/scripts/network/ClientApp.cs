﻿
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using KBEngine;
using System;

public class ClientApp : UnityEngine.MonoBehaviour
{
    public static KBEngineApp gameapp = null;
    public static GameObject client;
    public static ClientApp Instance;
    public int updateInterval;

    /*
     * Player Position Update Frequency
     */ 
    public int updateIntervalOnSerialize;
    int nextSendTickCount = Environment.TickCount;
    int nextSendTickCountOnSerialize = Environment.TickCount;
    public string url = "10.1.2.223";
    public int port = 17000;
    public string testUrl = "192.168.2.5";
    public int testPort = 20000;
    public bool debug = false;
    public int heartBeat = 8;

    void Awake()
    {
        client = gameObject;
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        UnityEngine.MonoBehaviour.print("client app start");
        gameapp = new KBEngineApp(this);
        KBEngineApp.url = "http://10.1.2.210";

        KBEngineApp.app.clientType = 1; //Mobile 
        if (debug)
        {
            KBEngineApp.app.ip = testUrl;
            KBEngineApp.app.port = Convert.ToUInt16(testPort);

            //var s = 
            new ChuMeng.DemoServer();

        } else
        {
            KBEngineApp.app.ip = url;
            KBEngineApp.app.port = Convert.ToUInt16(port);
        }

        //ConnectServer();
        StartCoroutine(CheckConnectState());
    }

    void ConnectServer()
    {
    }
    //First Connect
    //Check State OK?
    IEnumerator CheckConnectState()
    {
        while (true)
        {
            bool conSuc = false;
            while (!conSuc)
            {
                var ret = KBEngine.KBEngineApp.app.login_loginapp();
                if (!ret)
                {
                    Debug.LogError("FirstConnect Error");
                    //WaitTry
                    yield return new WaitForSeconds(1);
                }else {
                    ChuMeng.WindowMng.windowMng.ShowNotifyLog("网络连接成功");
                    conSuc = true;
                }
                yield return new WaitForSeconds(1);
            }

            //Check Net not connect then reconnect
            while(true) {
                if(!KBEngine.KBEngineApp.app.networkInterface().valid()){
                    ChuMeng.WindowMng.windowMng.ShowNotifyLog("网络中断");
                    break;
                }
                yield return new WaitForSeconds(1);
            }

            yield return new WaitForSeconds(1);
        }

    }

    void OnDestroy()
    {
        UnityEngine.MonoBehaviour.print("clientapp destroy");
        if (KBEngineApp.app != null)
        {
            KBEngineApp.app.destroy();
            UnityEngine.MonoBehaviour.print("client app over " + gameapp.isbreak + " over = " + gameapp.kbethread.over);
        }
        if (ChuMeng.DemoServer.demoServer != null)
        {
            ChuMeng.DemoServer.demoServer.ShutDown();
        }
    }

    void Update()
    {
        KBEUpdate();
    }

    //处理网络数据
    void KBEUpdate()
    {
        //处理网络回调
        gameapp.UpdateMain();

        //处理主角移动和其它玩家移动报文
        if (Environment.TickCount > this.nextSendTickCountOnSerialize && ChuMeng.ObjectManager.objectManager != null)
        {
            //KBEngineApp.app.RunViewUpdate();
            ChuMeng.ObjectManager.objectManager.RunViewUpdate();
            nextSendTickCountOnSerialize = Environment.TickCount + updateIntervalOnSerialize;
        }
        //KBEngine.Event.processOutEvents ();
    }

}
