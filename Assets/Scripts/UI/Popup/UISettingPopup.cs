using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPopup : UIBasePopup
{
    public override void Init(UIData uiData)
    {

    }

    public override void Close()
    {
        SaveLoadSystem.Instance.Save();
        base.Close();
    }
}
