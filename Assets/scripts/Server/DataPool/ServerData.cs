﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ChuMeng
{
    /// <summary>
    /// 服务端存储的用户数据 一个Json文件
    ///{"username":{}}
    /// </summary>
    public class ServerData
    {
        public static ServerData Instance = null;
        public PlayerInfo.Builder playerInfo;


        public ServerData(){
            Instance = this;
        }
        public void LoadData(){
            string fpath = Path.Combine (Application.persistentDataPath, "server.json");
            var exist = File.Exists (fpath);
            FileStream fs = null;
            if (exist) {
                fs = new FileStream (fpath, FileMode.Open);
            }

            if (fs == null) {
                playerInfo = PlayerInfo.CreateBuilder();
            }else {
                byte[] buffer;
                try {
                    long len = fs.Length;
                    buffer = new byte[len];
                    int count;
                    int sum = 0;
                    while ((count = fs.Read(buffer, sum, (int)(len-sum))) > 0) {
                        sum += count;
                    }
                } finally {
                    fs.Close ();
                }
                playerInfo = PlayerInfo.CreateBuilder().MergeFrom(buffer);

            }

        }

        /// <summary>
        /// 保存玩家数据到磁盘上面
        /// </summary>
        public void SaveUserData(){
            Log.Sys("SaveUserData");
            string fpath = Path.Combine (Application.persistentDataPath, "server.json");
            var msg = playerInfo.Build();
            using (FileStream outfile = new FileStream(fpath, FileMode.OpenOrCreate)) {

                msg.WriteTo(outfile);
            }
        }



    }
}
