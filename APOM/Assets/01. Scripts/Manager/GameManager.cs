using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>, IManager
{
    private GMStateMachine stateMachine;

    private void Awake()
    {


        stateMachine = new GMStateMachine();
        //stateMachine.ChangeState();
    }

    public void Init()
    { 
    }

    public void Clear()
    { 
    }
}
