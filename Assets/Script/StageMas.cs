using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMas : MonoBehaviour
{
    public StageSelectManager StageSelectManager;
    public string stageid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("押された" + gameObject.name);
        StageSelectManager.GetStageID(stageid);
    }
}
