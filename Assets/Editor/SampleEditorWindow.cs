using UnityEngine;
using UnityEditor;
using System.IO;


public class StageMission
{
    public int m1, m2, m3;

    public StageMission(int mission1, int mission2, int mission3)
    {
        m1 = mission1;
        m2 = mission2;
        m3 = mission3;
    }
}

public class SampleEditorWindow : EditorWindow
{
    [MenuItem("StageData/Create")]
    private static void Open()
    {
        string path = Application.dataPath;
        TextAsset csvFile = Resources.Load("StageData_m") as TextAsset; // ResourcesにあるCSVファイルを格納
        StringReader reader = new StringReader(csvFile.text); // TextAssetをStringReaderに変換

        int count = 0;
        // ステージデータのCSVからJSONファイルを作成する
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // 1行ずつ読み込む

            if (count > 0)
            {
                string[] StageData = line.Split(',');
                StageMission mission = new StageMission(int.Parse(StageData[0]), int.Parse(StageData[1]), int.Parse(StageData[2]));
                string json = JsonUtility.ToJson(mission);
                Debug.Log(json);
                File.Create(path + "/Resources" + "Sample" + count + ".json");
            }
            count++;
        }
        Debug.Log("End");
    }
}