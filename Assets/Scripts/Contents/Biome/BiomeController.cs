using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BiomeController : MonoBehaviour
{
    //TODO :: 크리쳐, 식생 생성된거 관리해야되요.

    [SerializeField]
    private BiomeData biomeData;

    public float showTime = 1f;
    public UnityEvent beginShowEvent;
    public UnityEvent endShowEvent;

    public float hideTime = 1f;
    public UnityEvent beginHideEvent;
    public UnityEvent endHideEvent;

    private Coroutine waitForEventCoroutine;

    [SerializeField]
    private GameObject biomeAlerterPrefab;

    public void Show()
    {
        Debug.Log($"Show Biome => {this.gameObject.name}");

        var biomeAlerter = UIController.Instance.CreateWorldUI(biomeAlerterPrefab).GetComponent<UIBiomeAlerter>();
        biomeAlerter.SetMessage($"{biomeData.BiomeName} 에 도달했습니다.");

        if (waitForEventCoroutine != null)
            StopCoroutine(waitForEventCoroutine);

        beginShowEvent?.Invoke();
        waitForEventCoroutine = StartCoroutine(WaitForEvent(showTime, endShowEvent));

    }

    public void Hide()
    {
        Debug.Log($"Hide Biome => {this.gameObject.name}");

        if (waitForEventCoroutine != null)
            StopCoroutine(waitForEventCoroutine);

        beginHideEvent?.Invoke();
        endHideEvent.AddListener(() =>
        {
            Destroy(this.gameObject);
        });
        waitForEventCoroutine = StartCoroutine(WaitForEvent(hideTime, endHideEvent));
    }

    private IEnumerator WaitForEvent(float time, UnityEvent endEvent)
    {
        yield return new WaitForSeconds(time);
        endEvent?.Invoke();
    }

}
