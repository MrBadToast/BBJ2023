using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class CheatMode : Singleton<CheatMode>
{
    UICheatModePopupData cheatModePopupData;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            if(cheatModePopupData == null) { 
                cheatModePopupData  = new UICheatModePopupData();
                UIController.Instance.OpenPopup(cheatModePopupData);
            }
            else {
                UIController.Instance.ClosePopup(cheatModePopupData);
                cheatModePopupData = null;
            }
        }
    }


}
