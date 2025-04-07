using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : IManager
{
    private GMStateMachine stateMachine;

    private void Awake()
    {


        stateMachine = new GMStateMachine();
        //stateMachine.ChangeState();
    }

    private void Start()
    {
        
    }

    public void Init()
    {
    }

    public void Clear()
    { 
    }
}
