using UnityEngine;
using System.Collections;
using playerData = ChuMeng.PlayerData;

namespace ServerPacketHandler {
    public class CGGetCharacterInfo : IPacketHandler  {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            playerData.GetProp(packet);
        }
    }

    public class CGAddProp : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            playerData.AddProp(packet);
        }
    }
    public class CGSetProp : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            playerData.SetProp(packet);
        }
    }
    public class CGCreateCharacter : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            playerData.CreateCharacter(packet);
        }
    }
}
