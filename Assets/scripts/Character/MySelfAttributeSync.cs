using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class MySelfAttributeSync : MonoBehaviour
    {
        public void NetworkAttribute(AvatarInfo info) {
            if(info.HasTeamColor) {
                GetComponent<NpcAttribute>().SetTeamColorNet(info.TeamColor);
            }
        }
    }
}