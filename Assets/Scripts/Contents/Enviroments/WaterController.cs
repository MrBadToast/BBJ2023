using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterController : MonoBehaviour
{
    private WorldController worldController;

    [SerializeField]
    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock mpb;

    [SerializeField]
    private AmountRangeFloat heightScaleRange;

    [SerializeField]
    private AmountRangeFloat heightYRange;

    [SerializeField]
    private float heightLerpTime = 1f;
    [SerializeField]
    private float colorLerpTime = 1f;

    public UnityEvent<float> updateHeightY;

    private Coroutine updateHeight;
    private Coroutine updateColor;

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        worldController = FindObjectOfType<WorldController>();
    }

    private void Start()
    {
        worldController.updateStatusEvent.AddListener(UpdateStatus);
        worldController.changeBiomeEvent.AddListener(ChangeColor);
    }

    private void UpdateStatus(StatusInfo statusInfo)
    {
        var surfaceAmount = statusInfo.GetElement(StatusType.Surface).CalculateTotalAmount();
        var height = heightScaleRange.GetRange((surfaceAmount + 1) * 0.5f);
        ChangeHeight(height);
    }

    public void ChangeHeight(float height)
    {
        if (updateHeight != null)
            StopCoroutine(updateHeight);

        updateHeight = StartCoroutine(UpdateHeight(height));
    }

    private IEnumerator UpdateHeight(float height)
    {
        var lerpTime = 0f;
        var startHeight = transform.localScale.y;
        var localScale = transform.localScale;
        while (lerpTime < heightLerpTime)
        {
            lerpTime += Time.deltaTime;
            var lerpHeight = Mathf.Lerp(startHeight, height, lerpTime / heightLerpTime);
            localScale.y = lerpHeight;
            transform.localScale = localScale;
            yield return null;
        }
    }

    public void ChangeColor(BiomeData biomeData)
    {
        if (updateColor != null)
            StopCoroutine(updateColor);

        updateColor = StartCoroutine(UpdateColor(biomeData.WaterColor));
    }

    private IEnumerator UpdateColor(Color color)
    {
        var startColor = meshRenderer.sharedMaterial.GetColor("_BaseColor");
        var lerpTime = 0f;
        while (lerpTime < colorLerpTime)
        {
            lerpTime += Time.deltaTime;
            var lerpColor = Color.Lerp(startColor, color, lerpTime / colorLerpTime);
            mpb.SetColor("_BaseColor", lerpColor);
            meshRenderer.SetPropertyBlock(mpb);
            yield return null;
        }
    }

}
