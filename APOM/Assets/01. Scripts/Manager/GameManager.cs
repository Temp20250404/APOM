using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : IManager
{
    public bool cursorVisible { get; private set; }

    public void Init()
    {
        SetCursorVisible(false);
    }

    public void Update()
    {
        CheckVisibleCursor();
    }

    public void Clear()
    { 
    }

    private void SetCursorVisible(bool _isVisible)
    {
        Cursor.visible = _isVisible;
        cursorVisible = _isVisible;

        if (_isVisible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void CheckVisibleCursor()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            SetCursorVisible(true);
        }
        else
        {
            SetCursorVisible(false);
        }
    }
}
