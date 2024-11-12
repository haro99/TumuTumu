using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameDataManager
{
    public static GameDataManager gameDataManager;

    public string UserID;
    
    public GameDataManager()
    {  
        gameDataManager = new GameDataManager();
    }

}
