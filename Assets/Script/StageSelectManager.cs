using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MiniJSON;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public struct MissonData
{
    public long misson1, misson2, misson3, openstagenumber, fruit;
    public string stagenumber;
}
public class StageSelectManager : MonoBehaviour
{
    public GameObject Canvas, StageNumber;
    public GameObject[] StageMas;
    public static MissonData missonData = new MissonData();
    public bool stagedetail;

    //UI
    public GameObject Stagedetail;
    public Text[] Missons;
    public Image[] Stars;
    public Text StageNumbertext;
    public int release;

    public DatabaseReference reference;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("release"))
        {
            release = PlayerPrefs.GetInt("release");
        }
        else
            release = 0;

        Debug.Log(release);
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < StageMas.Length; i++)
        {
            Vector3 position = Camera.main.WorldToScreenPoint(StageMas[i].transform.position);
            GameObject Object = Instantiate(StageNumber, position + new Vector3(0, 30f), Quaternion.identity, Canvas.transform);
            Object.GetComponent<TextMeshProUGUI>().text = "1-" + (i + 1);
        }

        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        StageDate date = new StageDate();
        try
        {
            FirebaseDatabase.DefaultInstance
              .GetReference("Z0yMnR9lkeOXbMZXFVSlrFzUKch2")
              .GetValueAsync().ContinueWithOnMainThread(task =>
              {
                  if (task.IsFaulted)
                  {
                      // Handle the error...
                  }
                  else if (task.IsCompleted)
                  {
                      DataSnapshot snapshot = task.Result;
                      // Do something with snapshot...
                      string json = snapshot.GetRawJsonValue();
                      Debug.Log(json);
                      JsonUtility.FromJsonOverwrite(json, date);
                  }
              });
        } catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        Debug.Log(date.date.Length);
        for (int i = 0; i < date.date.Length; i++)
        {
            
            if (date.date[i] == true)
            {
                StageMas[i].GetComponent<CircleCollider2D>().enabled = true;
                StageMas[i].GetComponent<SpriteRenderer>().color = new Color(255f, 255, 255f);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetStageID(string stageid)
    {
        //if (!stagedetail)
        //{
        //    var textAsset = Resources.Load(stageid) as TextAsset;
        //    var jsonText = textAsset.text;

        //    // 文字列を json に合わせて構成された辞書に変換
        //    var missionjson = Json.Deserialize(jsonText) as Dictionary<string, object>;
        //    Debug.Log((string)missionjson["stagenumber"]);
        //    Debug.Log((long)missionjson["combo"]);
        //    Debug.Log((long)missionjson["erase"]);
        //    Debug.Log((long)missionjson["score"]);

        //    //データ更新
        //    missonData.misson1 = (long)missionjson["combo"];
        //    missonData.misson2 = (long)missionjson["erase"];
        //    missonData.misson3 = (long)missionjson["score"];
        //    missonData.openstagenumber = (long)missionjson["openstage"];
        //    missonData.fruit = (long)missionjson["fruit"];
        //    missonData.stagenumber = (string)missionjson["stagenumber"];
        //    //表示更新
        //    StageNumbertext.text = missonData.stagenumber;
        //    Missons[0].text = "コンボを" + missonData.misson1 + "達成する";
        //    Missons[1].text = missonData.misson2 + "個消す";
        //    Missons[2].text = "スコアを" + missonData.misson3 + "以上獲得する";

        //    for (int i = 0; i < Stars.Length; i++)
        //    {
        //        if (PlayerPrefs.HasKey(missonData.stagenumber + i))
        //        {
        //            Debug.Log("データが存在する");
        //            Stars[i].color = new Color(255f, 255f, 255f);
        //        }
        //        else
        //        {
        //            Stars[i].color = new Color(0f, 0f, 0f);
        //        }
        //    }
        Stagedetail.SetActive(true);
        //    stagedetail = true;
        //}
    }

    public void Decision()
    {
        SceneManager.LoadScene(2);
    }

    public void Close()
    {
        stagedetail = false;
    }

    public void dataClear()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("セーブデータを削除しました");
    }

    public void DebugBtn()
    {
        reference.Child("Z0yMnR9lkeOXbMZXFVSlrFzUKch2").Child("date").Child("1").SetValueAsync(true);
    }
}
