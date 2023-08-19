using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PPModifier : MonoBehaviour
{
    private WorldController worldController;

    [SerializeField] private Volume heatVolume;
    [SerializeField] private Volume humidVolume;

    private void OnEnable()
    {
        worldController = FindFirstObjectByType<WorldController>();
        worldController.updateStatusEvent.AddListener(OnStatusUpdate);
    }

    private void OnDisable()
    {
        worldController.updateStatusEvent.RemoveListener(OnStatusUpdate);
    }

    float temperatureCache = -1f;
    float humidityCache = 0f;

    public void OnStatusUpdate(StatusInfo statusInfo)
    {
        if (worldController == null) return;

        if(statusInfo.StatusDic[StatusType.Temperature].GetAmount() != temperatureCache)
        {
            temperatureCache = statusInfo.StatusDic[StatusType.Temperature].GetAmount();
            heatVolume.weight = temperatureCache;
        }
        if(statusInfo.StatusDic[StatusType.Humidity].GetAmount() != humidityCache)
        {
            humidityCache = statusInfo.StatusDic[StatusType.Humidity].GetAmount();
            humidVolume.weight = humidityCache;
        }
    }
}
