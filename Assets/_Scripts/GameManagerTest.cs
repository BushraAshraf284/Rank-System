using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerTest
{
    private static GameManagerTest instance;

    public static GameManagerTest Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManagerTest();
            }
            return instance;
        }
    }
    public delegate void Delegates();
    public Delegates OnGameInitialized;
    public Delegates AfterGameInitialized;

    public void InitializeGame()
    {
        OnGameInitialized.Invoke();
        AfterGameInitialized.Invoke();
    }

   
   

    

  

}
