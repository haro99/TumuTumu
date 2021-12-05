using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UniRx;

public enum State
{
    Stop,
    Play
}
public class GameManager : MonoBehaviour
{
    public GameObject[] Fruits;
    public GameObject[] Items;
    [SerializeField]
    private List<GameObject> list = new List<GameObject>();
    public GameObject cam, selectojbect, Drop;
    [SerializeField]
    private LayerMask layerMask;
    public AudioSource Audio;
    public string name;
    public int earnings, score, maxcombo, maxerase, release, openstagenumber, maxfruit;
    public State state;
    public Animator FadeAnimator;
    public AudioClip[] Clip;
    public Subject<int> ScoreUpdate = new Subject<int>();
    //public string[] names;

    //UI
    public Text Score, Timer;
    public Text[] Missons;
    public GameObject[] stars;
    public GameObject Message, Canvas, Point, Buttons, MissonPanel;
    // Start is called before the first frame update
    void Start()
    {
        ScoreUpdate.Subscribe(point => PointDisplay(point));
        ScoreUpdate.Subscribe(point => ScoreAdd(point));
        release = PlayerPrefs.GetInt("release");
        Missons[0].text = StageSelectManager.missonData.misson1 + "コンボ以上達成";
        Missons[1].text = StageSelectManager.missonData.misson2 + "個以上消す";
        Missons[2].text = "スコアで" + StageSelectManager.missonData.misson3 + "点以上獲得する";
        openstagenumber = (int)StageSelectManager.missonData.openstagenumber;
        maxfruit = (int)StageSelectManager.missonData.fruit;
        StartCoroutine(FruitSet(25));
        StartCoroutine(Starting());
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Play:

                if (Input.GetMouseButton(0))
                {
                    Debug.Log("押されてるよ");
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    pos.z = 0;
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 10, layerMask);
                    Debug.DrawRay(pos, Vector2.zero, Color.blue, 10);

                    if (hit)
                    {
                        if (hit.collider.tag == "Item")
                        {
                            GameObject Object = hit.collider.gameObject;
                            Item Item = Object.GetComponent<Item>();
                            List<GameObject> GetObjects = Item.list;
                            Audio.PlayOneShot(Clip[3]);
                            if (GetObjects.Count > 0)
                            {
                                int point = 0;
                                for (int i = 0; i < GetObjects.Count; i++)
                                {
                                    GetObjects[i].GetComponent<Fruit>().Erase();
                                    point += 100;
                                }
                                StartCoroutine(FruitSet(GetObjects.Count));
                                ScoreUpdate.OnNext(point);
                            }
                            Destroy(Object);
                        }
                        else
                        {
                            Debug.Log(hit.collider.gameObject);
                            GameObject obj = hit.collider.gameObject;
                            Fruit touchFruit = obj.GetComponent<Fruit>();

                            //消滅中じゃないなら
                            if (!touchFruit.erasing)
                            {
                                //初回のくだもの
                                if (name == "")
                                {
                                    list.Add(obj);
                                    Audio.PlayOneShot(Clip[2]);
                                    selectojbect = obj;
                                    name = touchFruit.fruitname;
                                    obj.transform.localScale = new Vector3(3f, 3f, 3f);
                                }//一定距離なら
                                else if (CheckDistance(obj))
                                {
                                    string getname = touchFruit.fruitname;
                                    //同じ名前でかつゲットリストに登録されてないものなら
                                    if (name == getname && !list.Contains(obj))
                                    {
                                        list.Add(obj);
                                        Audio.PlayOneShot(Clip[2]);
                                        selectojbect = obj;
                                        obj.transform.localScale = new Vector3(3f, 3f, 3f);
                                    }
                                }
                            }
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("押されてないよ");

                    //リストに入ってる果物の処理
                    if (list.Count > 1)
                    {
                        Debug.Log("3個以上揃ってるよ");
                        Audio.PlayOneShot(Clip[3]);
                        int point = 0;
                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i].GetComponent<Fruit>().Erase();
                            point += 100;
                        }
                        StartCoroutine(FruitSet(list.Count));
                        int number = list.Count - 2;
                        //for (int i = 0; i < 3; i++)
                        //{
                        //    if (name == names[i])
                        //    {
                        //        //stocks[i] += number;
                        //        break;
                        //    }
                        //}

                        //ポイント更新
                        Debug.Log(point);
                        ScoreUpdate.OnNext(point);


                        //Maxコンボ、消した数の処理
                        if (maxcombo < list.Count)
                            maxcombo = list.Count;
                        maxerase += list.Count;

                        if(list.Count>5)
                        {
                            Instantiate(Items[0], Drop.transform.position + new Vector3(Random.Range(-2, 3), 0, 0), Quaternion.identity);
                        }
                        list.Clear();

                    }
                    else
                    {
                        Debug.Log("2個未満だよ");
                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i].transform.localScale = new Vector3(2.5f, 2.5f, 1f);
                        }
                        list.Clear();
                    }
                    name = "";
                }
                break;
        }
    }

    /// <summary>
    /// 同じ果物の距離のチェック
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool CheckDistance(GameObject target)
    {
        float distance = Vector2.Distance(selectojbect.transform.position, target.transform.position);
        Debug.Log(distance);
        if (distance < 2f)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// フルーツの生成
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    IEnumerator FruitSet(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject obj = Instantiate(Fruits[Random.Range(0, maxfruit)], Drop.transform.position+ new Vector3(Random.Range(-2, 3), 0, 0), Quaternion.identity);
            obj.name = i.ToString();
            yield return null;
        }
    }

    //ゲーム終了語のリザルト
    public void GameEnd()
    {
        int clearcount = 0;
        var sequence = DOTween.Sequence();
        sequence.Append(MissonPanel.transform.DOLocalMoveY(0f, 2f).SetEase(Ease.OutBounce));
        if (StageSelectManager.missonData.misson1 <= maxcombo)
        {
            clearcount++;
            PlayerPrefs.SetInt(StageSelectManager.missonData.stagenumber + 0, 0);
            sequence.Append(stars[0].transform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutBack));
        }
        if (StageSelectManager.missonData.misson2 <= maxerase)
        {
            clearcount++;
            PlayerPrefs.SetInt(StageSelectManager.missonData.stagenumber + 1, 0);

            sequence.Append(stars[1].transform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutBack));
        }
        if (StageSelectManager.missonData.misson3 <= score)
        {
            clearcount++;
            PlayerPrefs.SetInt(StageSelectManager.missonData.stagenumber + 2, 0);
            sequence.Append(stars[2].transform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutBack));
        }
        sequence.AppendInterval(3f);

        sequence.Play()
        .OnComplete(() =>
        {
            //完了時に呼ばれる
            Debug.Log("OnComplete");
            Buttons.SetActive(true);
        });

        //ミッションをすべてクリアしたら
        if (clearcount >= 3 && release < openstagenumber)
            PlayerPrefs.SetInt("release", openstagenumber);

    }

    /// <summary>
    /// スタート処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Starting()
    {
        yield return new WaitUntil(() => FadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        var sequence = DOTween.Sequence();

        sequence.AppendInterval(3f);
        sequence.Append(MissonPanel.transform.DOLocalMoveY(1500f, 2f));
        sequence.Play()
            .OnComplete(() => 
            {
                state = State.Play;
                Audio.PlayOneShot(Clip[0]);
                StartCoroutine(CountUp());
            });
    }

    /// <summary>
    /// カウントアップ
    /// </summary>
    /// <returns></returns>
    IEnumerator CountUp()
    {
        Message.SetActive(true);
        yield return new WaitForSeconds(1);
        Message.SetActive(false);

        for (int i = 60; i >= 0; i--)
        {
            Timer.text = "残り" + i.ToString() + "秒";
            yield return new WaitForSeconds(1);
        }

        state = State.Stop;
        Message.GetComponent<Text>().text = "タイムアップ！";
        Message.SetActive(true);
        Audio.PlayOneShot(Clip[1]);
        yield return new WaitForSeconds(3);
        Message.SetActive(false);
        GameEnd();
    }

    /// <summary>
    /// タッチしたところにポイントを表示
    /// </summary>
    /// <param name="point"></param>
    private void PointDisplay(int point)
    {
        Vector3 position = Input.mousePosition;
        GameObject SetPoint = Instantiate(Point, position, Quaternion.identity, Canvas.transform);
        SetPoint.GetComponent<Text>().text = "+" + point.ToString();
        Destroy(SetPoint, 2f);
    }

    /// <summary>
    /// スコアの更新
    /// </summary>
    /// <param name="point"></param>
    private void ScoreAdd(int point)
    {
        score += point;
        Score.text = score.ToString();
    }

    public void SceneChange(int changenumber)
    {
        SceneManager.LoadScene(changenumber);
    }
}
