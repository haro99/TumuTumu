using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cloud : MonoBehaviour
{
    private Vector3 move;
    // Start is called before the first frame update
    void Start()
    {

        this.transform.DOMove(new Vector3(-10f, transform.position.y, 0), Random.Range(10f, 20f))
                        .SetLoops(-1, LoopType.Restart)
                        .SetEase(Ease.Linear);
                      
    }

    // Update is called once per frame
    void Update()
    {

    }
}
