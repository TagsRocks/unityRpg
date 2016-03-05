﻿﻿﻿using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    /// <summary>
    /// 玩家自己的属性 从服务器上同步 
    /// </summary>
    public class MySelfAttributeSync : MonoBehaviour
    {
        public void NetworkAttribute(AvatarInfo info) {
            var attr = GetComponent<NpcAttribute>();
            if(info.HasTeamColor) {
                attr.SetTeamColorNet(info.TeamColor);
            }
            if(info.HasIsMaster) {
                attr.SetIsMasterNet(info.IsMaster);
            }
        }
    }
}