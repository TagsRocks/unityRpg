using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
namespace ChuMeng
{
    /// <summary>
    /// Stream Load Room Data PerFrame A Piece
    /// </summary>
    public class RoomData : MonoBehaviour
    {
        [System.Serializable]
        public class RoomPosRot{
            public GameObject prefab;
            public Vector3 pos;
            public Quaternion rot;
            public Vector3 scale = Vector3.one;
        }
        public List<RoomPosRot> Prefabs = new List<RoomPosRot>();

    }
}
