using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting : Singleton<GlobalSetting>
{
    public int mouseSensitivity;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
}
