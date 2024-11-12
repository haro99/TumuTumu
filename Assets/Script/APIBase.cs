using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;


public interface APIBase
{
    void Setting();

    UniTask  Execute();

    void Error();

}

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
        }
        else if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text);
            CreateUser createUser = JsonUtility.FromJson<CreateUser>(request.downloadHandler.text);
            Debug.Log(createUser.code);
            //GameDataManager.gameDataManager.UserID = createUser.userid;
            Debug.Log(createUser.userdata.userid);
        }
    }

    public void Error() 
    {

    }
}