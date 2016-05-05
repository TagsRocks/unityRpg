using UnityEngine;
using System.Collections;
using MyLib;
using System.Text.RegularExpressions;

//汉字 英文 字母 数字 符号 其它不允许
public class EnterGame : MyLib.IUserInterface
{
    UIInput nameInput;
    Regex reg = new Regex("[a-zA-Z0-9]");
    void Awake()
    {
        nameInput = GetInput("NameInput");
        SetCallback("createChar", OnEnter);
    }

    void OnEnter()
    {
        if (string.IsNullOrEmpty(nameInput.value))
        {
            Util.ShowMsg("名字不能为空");
            return;
        }
        if (nameInput.value.Length > 30)
        {
            Util.ShowMsg("名字太长，不能超过30个英文，10个汉字");
        }
        foreach (var s in nameInput.value)
        {
            if(!IsChinese(s) && !IsAlpheNum(s)) {
                Util.ShowMsg("名字只支持中文和英文以及数字，其它字符不支持！");
                return;
            }
        }

        //RemoteNetworkManager.Instance.StartGame(nameInput.value);
    }

    private bool IsChinese(char s)
    {
        if (s >= 0x20000 && s <= 0xFA2D)
        {
            return true;
        }
        return false;
    }
    private bool IsAlpheNum(char s) {
        var st = "";
        st += s;
        if(reg.IsMatch(st)) {
            return true;
        }
        return false;
    }

}
