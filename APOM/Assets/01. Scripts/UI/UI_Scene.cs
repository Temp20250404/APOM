using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChat
{
    void ReceiveChat(string message, uint channel);
}

public class UI_Scene : UI_Base
{
    //public UI_Chat UI_Chat = null;
    public override void Init()
    {

    }


    //Managers.UI.SetCanvas(gameObject, true);

    //UI_Chat = GetComponentInChildren<UI_Chat>(true);

    //UI_Scene[] children = GetComponentsInChildren<UI_Scene>(true);

    //foreach (var ui in children)
    //{
    //    if (ui != this)
    //        ui.Init();
    //}


    //public GameObject Find(GameObject _gameObject)
    //{
    //    // 자식 중 UI_Chat 찾기
    //    UI_Chat = GetComponentInChildren<UI_Chat>(true);

    //    return gameObject;
    //}
}
