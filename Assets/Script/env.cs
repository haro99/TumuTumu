using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class env
{
    private static string url = "http://127.0.0.1:8000";

    public static string URL { get { return url; } }

    private static string userid = string.Empty;

    public static string UserID { get { return userid; } set { userid = value; } }
}
