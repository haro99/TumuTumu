using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public string userid;
}
[Serializable]
public class Stagedata
{
    public string tumutumu_id;
    public string stagenumber;
    public bool mission1;
    public bool mission2;
    public bool mission3;
    public bool open;
}

[Serializable]
public class ResponseBase
{
    public int code = 0;
}

[Serializable]
public class CreateUser:ResponseBase
{
    public UserData userdata;
}

[Serializable]
public class StageData:ResponseBase
{
    public Stagedata Stagedata;
}

[Serializable]
public class Stagedatas:ResponseBase
{
    public bool[] open;
}