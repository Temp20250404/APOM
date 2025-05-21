using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MiniMapObject
{
    MiniMapCamera,
}
public class UI_MiniMap : UI_Base
{
    public Player player;
    private Camera miniMapCamera;
    public override void Init()
    {
        Bind<Camera>(typeof(MiniMapObject));
        miniMapCamera = Get<Camera>((int)(MiniMapObject.MiniMapCamera));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
            return;
        Vector3 playerPos = player.transform.position;
        playerPos.y = 10.0f;
        miniMapCamera.transform.position = playerPos;
    }
}
