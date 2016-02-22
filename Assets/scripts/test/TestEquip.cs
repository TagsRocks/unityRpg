#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class TestEquip : MonoBehaviour {

    public int equipId;
    [ButtonCallFunc()] public bool AddEquip;
    public void  AddEquipMethod() {
        ChuMeng.PlayerData.AddEquipInPackage(equipId, 0, 0);
    }
}

#endif