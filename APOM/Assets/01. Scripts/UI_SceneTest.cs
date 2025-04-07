using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SceneTest : UI_Scene
{
    enum Text
    {
        Text,
    }

    public override void Init()
    {
        base.Init();

        // Bind<UnityEngine.UI.Text, Button 등>(typeof(enum 값))
        Bind<UnityEngine.UI.Text>(typeof(Text));
        GetText((int)Text.Text).text = "Hello World!";

        
    }
}
