using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIManager : MonoBehaviour
{
    public static Dictionary<string, APIBase> APIlist = new Dictionary<string, APIBase>();

    public APIBase SelectAPI;

    public static APIManager apimanager;

    public APIManager()
    {
        apimanager = this;
    }

    public void CommandSetting()
    {
        APIlist.Add("CreateUser", new CreateUserAPI());
    }

    public void IndexAPI(string name)
    {
        SelectAPI = APIlist[name];
    }

    public async void Execute()
    {
        SelectAPI.Setting();

        await SelectAPI.Execute();

        Debug.Log("APIí êMÇ™èIóπÇµÇ‹ÇµÇΩ");
    }
}
