using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
public class StageDate
{
    public bool[] date = new bool[5];
}
public class TitleManager : MonoBehaviour
{
    public Animator FadeAnimator;
    public static string userid;
    FirebaseAuth _auth;
    DatabaseReference reference;
    FirebaseUser _user;
    public FirebaseUser UserData { get { return _user; } }
    public delegate void CreateUser(bool result);
    // Start is called before the first frame update
    void Awake()
    {
        // 初期化
        _auth = FirebaseAuth.DefaultInstance;

        // すでにユーザーが作られているのか確認
        if (_auth.CurrentUser.UserId == null)
        {
            // まだユーザーができていないためユーザー作成
           Create((result) =>
            {
                if (result)
                {
                    Debug.Log($"成功: #{_user.UserId}");
                }
                else
                {
                    Debug.Log("失敗");
                }
            });
        }
        else
        {
            _user = _auth.CurrentUser;
            
            Debug.Log($"ログイン中: #{_user.UserId}");
        }
    }
    private void Start()
    {
        userid = _auth.CurrentUser.UserId;

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void Create(CreateUser callback)
    {
        _auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                callback(false);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                callback(false);
                return;
            }

            _user = task.Result;
            Debug.Log($"User signed in successfully: {_user.DisplayName} ({_user.UserId})");
            callback(true);
        });
    }

    public void Fade()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        FadeAnimator.SetTrigger("In");
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => FadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        SceneManager.LoadScene(1);

    }

    public async void CreateUserDate()
    {


        StageDate date = new StageDate();
        date.date[0] = true;
        string json = JsonUtility.ToJson(date);

        Debug.Log("制作中…");
        await reference.Child(_user.UserId).SetRawJsonValueAsync(json);

        Debug.Log("データ作成");
    }
}
