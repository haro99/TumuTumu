using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fruit : MonoBehaviour
{
    public GameObject movepos;
    public string fruitname;
    public bool erasing;
    public CircleCollider2D circleCollider;
    public Rigidbody2D RB;
    // Start is called before the first frame update
    void Start()
    {
        movepos = GameObject.Find("MovePos");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Erase()
    {
        this.transform.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete(() =>
        {
            Destroy(this.gameObject);
        });

        erasing = true;
    }
}
