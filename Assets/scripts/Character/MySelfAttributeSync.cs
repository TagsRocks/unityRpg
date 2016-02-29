using UnityEngine;
using System.Collections;

namespace ChuMeng
{
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