using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausePopup : UIBasePopup
{
    public override void Init(UIData uiData)
    {

    }

    public override void BeginOpen()
    {
        Time.timeScale = 0f;
        base.BeginOpen();
    }

    public override void EndClose()
    {
        base.EndClose();
        Time.timeScale = 1f;
    }
}
