using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEditor.PackageManager.Requests;

/// <summary>
/// APIでの通信のベースクラス
/// </summary>
public interface APIBase
{
    //urlを設定する関数、ベースURLに追加するパラーメータ設定など
    void Setting();

    //API通信を行う関数
    UniTask  Execute();

    //通信エラー後の関数
    void Error();

}

/// <summary>
/// ユーザーID作成API
/// </summary>
public class CreateUserAPI:APIBase
{
    public string parm;
    public void Setting()
    {
        this.parm = "/api/tumutumu/create";
    }
    public async UniTask Execute()
    {
        UnityWebRequest request = UnityWebRequest.Get(env.URL + parm);

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            Error();
        }
        else if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text);
            CreateUser createUser = JsonUtility.FromJson<CreateUser>(request.downloadHandler.text);
            Debug.Log(createUser.code);
            GameDataManager.UserID = createUser.userdata.userid;
            PlayerPrefs.SetString("userid", createUser.userdata.userid);
            Debug.Log("IDを発行しました:" + createUser.userdata.userid);
        }
    }

    public void Error() 
    {
        //ここでエラー時のUIを表示する
    }
}

public class StageDataShow:APIBase
{
    public string parm;

    public void Setting()
    {
        parm = "/api/stagedata/";
    }

    public async UniTask Execute()
    {
        UnityWebRequest request = UnityWebRequest.Get(env.URL + parm + GameDataManager.UserID);

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            Error();
        }
        else if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text);
            StageDatas stageDatas = JsonUtility.FromJson<StageDatas>(request.downloadHandler.text);
            Debug.Log(stageDatas.code);

            StageSelectManager.stageSelectManager.StageOpen(stageDatas.open);
        }
    }

    public void Error()
    {

    }
}

public class StageDateDetail:APIBase
{
    public string parm;

    public void Setting()
    {
        parm = "/api/stagedata/";
    }

    public async UniTask Execute()
    {
        UnityWebRequest request = UnityWebRequest.Get(env.URL + parm + GameDataManager.UserID + "/" + 1);

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            Error();
        }
        else if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text);
            StageData stagedata = JsonUtility.FromJson<StageData>(request.downloadHandler.text);
            Debug.Log(stagedata.code);

            StageSelectManager.stageSelectManager.StageDetailShow(stagedata);
        }
    }

    public void Error()
    {

    }
}

public class DataUpdate:APIBase
{

    public string parm;

    public void Setting()
    {
        parm = "/api/dataupdate";
    }

    public async UniTask Execute()
    {
        // リクエストオブジェクトを JSON に変換（byte配列）
        string reqJson = JsonUtility.ToJson(GameDataManager.StageData);
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(reqJson);

        // HTTP（POST）の情報を設定
        var request = new UnityWebRequest(env.URL + parm, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            Error();
        }
        else if (request.responseCode == 200)
        {

        }
    }

    public void Error()
    {

    }
}