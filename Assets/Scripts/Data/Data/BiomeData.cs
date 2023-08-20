using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "BiomeData", menuName = "Biome/BiomeData", order = 0)]
public class BiomeData : ScriptableObject
{
    [System.Serializable]
    public enum RangeType
    {
        None,
        Smaller,
        Range,
        Larger,
    }

    [SerializeField]
    private int order;
    public int Order => order;

    [SerializeField]
    private string key;
    public string Key => key;

    [SerializeField]
    private string biomeName;
    public string BiomeName => biomeName;

    [SerializeField]
    private Sprite icon;
    public string Icon => Icon;

    [SerializeField]
    [TextArea]
    private string context;
    public string Context => context;

    [SerializeField]
    private Color waterColor = Color.blue;
    public Color WaterColor => waterColor;

    [SerializeField]
    private float wetAmount = 0f;
    public float WetAmount => wetAmount;

    [SerializeField]
    private float snowAmount = 0f;
    public float SnowAmount => snowAmount;

    [SerializeField]
    private float sandAmount = 0f;
    public float SandAmount => sandAmount;

    [SerializeField]
    private bool useSurfaceRange = true;
    public bool UseSurfaceRange => useSurfaceRange;

    [SerializeField]
    [ShowIf("useSurfaceRange")]
    private RangeType surfaceRangeType;

    [SerializeField]
    [ShowIf("useSurfaceRange")]
    private AmountRangeFloat surfaceRange;
    public AmountRangeFloat SurfaceRange => surfaceRange;

    [SerializeField]
    private bool useHumidityRange = true;
    public bool UseHumidityRange => useHumidityRange;

    [SerializeField]
    [ShowIf("useHumidityRange")]
    private RangeType humidityRangeType;

    [SerializeField]
    [ShowIf("useHumidityRange")]
    private AmountRangeFloat humidityRange;
    public AmountRangeFloat HumidityRange => humidityRange;

    [SerializeField]
    private bool useTemperatureRange = true;
    public bool UseTemperatureRange => useTemperatureRange;

    [SerializeField]
    [ShowIf("useTemperatureRange")]
    private RangeType temperatureRangeType;

    [SerializeField]
    [ShowIf("useTemperatureRange")]
    private AmountRangeFloat temperatureRange;
    public AmountRangeFloat TemperatureRange => temperatureRange;

    [SerializeField]
    private GameObject enviromentPrefab;
    public GameObject EnviromentPrefab => enviromentPrefab;

    public bool ContainStatus(StatusInfo statusInfo)
    {
        if (useSurfaceRange)
        {
            var surfaceAmount = statusInfo.GetElement(StatusType.Surface).CalculateTotalAmount();

            switch (surfaceRangeType)
            {
                case RangeType.None:
                    break;
                case RangeType.Smaller:
                    if (surfaceAmount <= surfaceRange.min)
                        break;
                    else
                        return false;
                case RangeType.Range:
                    if (surfaceRange.ContainExcusive(surfaceAmount))
                        break;
                    else
                        return false;
                case RangeType.Larger:
                    if (surfaceAmount >= surfaceRange.max)
                        break;
                    else
                        return false;
            }
        }

        if (useHumidityRange)
        {
            var humidityAmount = statusInfo.GetElement(StatusType.Humidity).CalculateTotalAmount();

            switch (humidityRangeType)
            {
                case RangeType.None:
                    break;
                case RangeType.Smaller:
                    if (humidityAmount <= surfaceRange.min)
                        break;
                    else
                        return false;
                case RangeType.Range:
                    if (surfaceRange.ContainExcusive(humidityAmount))
                        break;
                    else
                        return false;
                case RangeType.Larger:
                    if (humidityAmount >= surfaceRange.max)
                        break;
                    else
                        return false;
            }
        }

        if (useTemperatureRange)
        {
            var temperatureAmount = statusInfo.GetElement(StatusType.Temperature).CalculateTotalAmount();

            switch (temperatureRangeType)
            {
                case RangeType.None:
                    break;
                case RangeType.Smaller:
                    if (temperatureAmount <= surfaceRange.min)
                        break;
                    else
                        return false;
                case RangeType.Range:
                    if (surfaceRange.ContainExcusive(temperatureAmount))
                        break;
                    else
                        return false;
                case RangeType.Larger:
                    if (temperatureAmount >= surfaceRange.max)
                        break;
                    else
                        return false;
            }
        }

        return true;
    }
}
