using UnityEngine;
using System.Collections;
using MyLib;

public class RemoteLoginInit : MonoBehaviour
{
    public static RemoteLoginInit Instance;

    void Awake()
    {
        Instance = this;

        if (SaveGame.saveGame == null)
        {
            var saveGame = new GameObject("SaveGame");
            saveGame.AddComponent<SaveGame>();
            saveGame.GetComponent<SaveGame>().InitData();
            saveGame.GetComponent<SaveGame>().InitServerList();
        }
    }

    void Start()
    {
        WindowMng.windowMng.PushView("UI/EnterGame");
    }
}
