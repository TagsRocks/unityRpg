using UnityEngine;
using System.Collections;
using MyLib;
using System.Text.RegularExpressions;

public class MainUI2 : IUserInterface
{
    UIInput nameInput;
    Regex reg = new Regex("[a-zA-Z0-9]");

    void Awake()
    {
        nameInput = GetInput("NameInput");
        SetCallback("createChar", OnEnter);
        nameInput.value = ServerData.Instance.playerInfo.Roles.Name;
    }

    void OnEnter()
    {

        if (string.IsNullOrEmpty(nameInput.value))
        {
            Util.ShowMsg(Localization.Get("NameNotEmpty"));
            return;
        }
        if (nameInput.value.Length > 30)
        {
            Util.ShowMsg(Localization.Get("NameTooLong"));
        }
        foreach (var s in nameInput.value)
        {
            if(!IsChinese(s) && !IsAlpheNum(s)) {
                Util.ShowMsg(Localization.Get("NameNoSpecial"));
                return;
            }
        }
        ServerData.Instance.playerInfo.Roles.Name = nameInput.value;
        ServerData.Instance.playerInfo.Roles.Job = Job.ARMOURER;
        WorldManager.worldManager.WorldChangeScene(6, false);
    }

    private bool IsChinese(char s)
    {
        Log.GUI("ChineseCode: "+(int)s);
        if (s >= 0x4E00 && s <= 0x2FA1F)
        {
            return true;
        }
        return false;
    }

    private bool IsAlpheNum(char s)
    {
        var st = "";
        st += s;
        if (reg.IsMatch(st))
        {
            return true;
        }
        return false;
    
    }
}