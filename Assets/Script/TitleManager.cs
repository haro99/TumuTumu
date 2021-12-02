using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Animator FadeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
