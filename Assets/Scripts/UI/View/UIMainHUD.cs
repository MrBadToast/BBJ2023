using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainHUD : UIBaseView
{
    UIPausePopup pausePopup;

    public override void Init(UIData uiData)
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausePopup == null)
        {
            pausePopup = (UIPausePopup)UIController.Instance.OpenPopup("Pause");

            pausePopup.endCloseEvent.AddListener(() =>
            {
                pausePopup = null;
            });
        }
    }

}
