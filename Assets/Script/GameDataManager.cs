using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameDataManager
{

    private static string userID;

    public static Stagedata StageData;

    public static string UserID {  get { return userID; } set { userID = value; } }

}
