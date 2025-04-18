using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestEditorWindow : EditorWindow
{
    private static TestEditorWindow window;

    [MenuItem("Window/Test Editor")]
    private static void Setup()
    {
        //window = GetWindow<TestEditorWindow>
    }
}
