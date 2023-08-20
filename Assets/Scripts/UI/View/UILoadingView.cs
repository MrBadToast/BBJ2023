using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UILoadingView : UIBaseView
{
    [SerializeField]
    private LoadingTipData loadingTipData;

    [SerializeField]
    private UIBaseText loadingText;

    public AmountRangeFloat viewTimeRange;

    private Coroutine viewWaitCoroutine;

    public override void Init(UIData uiData)
    {

    }

    protected override void Start()
    {

    }

    public override void Open()
    {
        gameObject.SetActive(true);
        foreach (var animator in openAnimatorElementList)
        {
            animator.tweenAnimator.PlayAnimation(animator.tweenKey);
        }
        loadingText.SetText(loadingTipData.GetRandomData());
        StartCoroutine(CoWaitForViewTime());
    }

    private IEnumerator CoWaitForViewTime()
    {
        var time = viewTimeRange.GetRandomAmount();

        while (time >= 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        SceneLoader.Instance.LoadNextScene();
    }

}
