using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeScript:MonoBehaviour
{
    public Animator FadeAnimator;
    public Image Image;
    private void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitUntil(() => FadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Image.enabled = false;
    }
}
