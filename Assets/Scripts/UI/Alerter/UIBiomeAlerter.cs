using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBiomeAlerter : UIBaseView
{
    [SerializeField]
    private float showTime = 1.5f;

    [SerializeField]
    private UIBaseText nameText;

    public override void Init(UIData uiData)
    {
        var alertData = (UIBiomeAlertData)uiData as UIBiomeAlertData;

        nameText.SetText(alertData.alertMessage);
    }

    public override void EndOpen()
    {
        base.EndOpen();
        StartCoroutine(WaitForShowTime(showTime));
    }

    private IEnumerator WaitForShowTime(float time)
    {
        yield return new WaitForSeconds(time);
        Close();

    }
}
