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
    public class CGLoadPackInfo : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet){
            playerData.LoadPackInfo(packet);
        }
    }

    public class CGGetKeyValue : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet){
            playerData.GetKeyValue(packet);
        }
    }

    /// <summary>
    /// new  NewUserStory 
    /// </summary>
    public class CGSetKeyValue : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet){
            playerData.SetKeyValue(packet);
        }
    }

    public class CGLevelUpEquip : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet){
            playerData.LevelUpEquip(packet);
        }
    }
    public class CGLevelUpGem : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet){
            playerData.LevelUpGem(packet);
        }
    }

    public class CGSellUserProps : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet){
            playerData.SellUserProps(packet);
        }
    }
}   
