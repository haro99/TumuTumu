using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MiniJSON;
using TMPro;

public struct MissonData
{
    public long misson1, misson2, misson3, openstagenumber, fruit;
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
    public Text StageNumbertext;
    public int release;

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
        for(int i = 0; i< StageMas.Length;i++)
        {
            Vector3 position = Camera.main.WorldToScreenPoint(StageMas[i].transform.position);
            GameObject Object = Instantiate(StageNumber, position + new Vector3(0, 30f), Quaternion.identity, Canvas.transform);
            Object.GetComponent<TextMeshProUGUI>().text = "1-" + (i + 1);
        }
        for (int i = 0; i <= release; i++)
        {
            StageMas[i].GetComponent<CircleCollider2D>().enabled = true;
            StageMas[i].GetComponent<SpriteRenderer>().color = new Color(255f, 255, 255f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetStageID(string stageid)
    {
        if (!stagedetail)
        {
            var textAsset = Resources.Load(stageid) as TextAsset;
            var jsonText = textAsset.text;

            // 文字列を json に合わせて構成された辞書に変換
            var json = Json.Deserialize(jsonText) as Dictionary<string, object>;
            Debug.Log((string)json["stagenumber"]);
            Debug.Log((long)json["combo"]);
            Debug.Log((long)json["erase"]);
            Debug.Log((long)json["score"]);
            missonData.misson1 = (long)json["combo"];
            missonData.misson2 = (long)json["erase"];
            missonData.misson3 = (long)json["score"];
            missonData.openstagenumber = (long)json["openstage"];
            missonData.fruit = (long)json["fruit"];
            StageNumbertext.text = (string)json["stagenumber"];
            Missons[0].text = "コンボを" + missonData.misson1 + "達成する";
            Missons[1].text = missonData.misson2 + "個消す";
            Missons[2].text = "スコアを" + missonData.misson3 + "以上獲得する";

            Stagedetail.SetActive(true);
            stagedetail = true;
        }
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
}
