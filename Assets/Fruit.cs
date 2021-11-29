﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject movepos;
    public string fruitname;
    public bool move;
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
        if (move)
        {
            float range = Vector2.Distance(gameObject.transform.position, movepos.transform.position);
            if (range < 0.5f)
                Destroy(gameObject);

            Vector3 distance = movepos.transform.position - gameObject.transform.position;
            gameObject.transform.position += distance * Time.deltaTime;
        }
    }

    public void Moveing()
    {
        circleCollider.enabled = false;
        RB.simulated = false;
        move = true;
    }
}
