using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BiomeController : MonoBehaviour
{
    //TODO :: 农府媚, 侥积 积己等芭 包府秦具登夸.

    public float showTime = 1f;
    public UnityEvent beginShowEvent;
    public UnityEvent endShowEvent;

    public float hideTime = 1f;
    public UnityEvent beginHideEvent;
    public UnityEvent endHideEvent;

    private Coroutine waitForEventCoroutine;

    public void Show()
    {
        if (waitForEventCoroutine != null)
            StopCoroutine(waitForEventCoroutine);

        beginShowEvent?.Invoke();
        waitForEventCoroutine = StartCoroutine(WaitForEvent(showTime, endShowEvent));

    }

    public void Hide()
    {
        if (waitForEventCoroutine != null)
            StopCoroutine(waitForEventCoroutine);

        beginHideEvent?.Invoke();
        waitForEventCoroutine = StartCoroutine(WaitForEvent(hideTime, endHideEvent));
    }

    private IEnumerator WaitForEvent(float time, UnityEvent endEvent)
    {
        yield return new WaitForSeconds(time);
        endEvent?.Invoke();
    }

}
