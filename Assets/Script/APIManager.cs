using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIManager : MonoBehaviour
{
    public static Dictionary<string, APIBase> APIlist = new Dictionary<string, APIBase>();

    public APIBase SelectAPI;

    public static APIManager apimanager;

    private void Start()
    {
        if (apimanager == null)
        {
            apimanager = this;
            DontDestroyOnLoad(gameObject);
            CommandSetting();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CommandSetting()
    {
        APIlist.Add("CreateUser", new CreateUserAPI());
        APIlist.Add("StageData", new StageDataShow());
        APIlist.Add("StageDetail", new StageDateDetail());
        APIlist.Add("DataUpdate", new DataUpdate());
    }

    public void IndexAPI(string name)
    {
        // TODO 原因はここ、コマンドが登録されていなくユーザーデータ登録が作れない
        Debug.Log(APIlist.Count);
        SelectAPI = APIlist[name];
    }

    public async void Execute()
    {
        SelectAPI.Setting();

        await SelectAPI.Execute();

        Debug.Log("API通信が終了しました");
    }
}
