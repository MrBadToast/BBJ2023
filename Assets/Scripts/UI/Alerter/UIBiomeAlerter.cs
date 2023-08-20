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
    }

    public void SetMessage(string message) {

        nameText.SetText(message);
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
