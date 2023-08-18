using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "BiomeData", menuName = "Biome/BiomeData", order = 0)]
public class BiomeData : ScriptableObject
{
    [SerializeField]
    private string key;
    public string Key => key;

    [SerializeField]
    private Sprite icon;
    public string Icon => Icon;

    [SerializeField]
    [TextArea]
    private string context;
    public string Context => context;   
  
    [SerializeField]
    private StatusInfo statusInfo;
    public StatusInfo StatusInfo => statusInfo;

    [SerializeField]
    private GameObject enviromentPrefab;
    public GameObject EnviromentPrefab => enviromentPrefab;

    [Button("AutoGenerate")]
    public void AutoGenerate()
    {
        statusInfo.StatusDic.Clear();

        IEnumerable<StatusType> StatusTypeList =
                Enum.GetValues(typeof(StatusType)).Cast<StatusType>();

        foreach (StatusType statusType in StatusTypeList)
        {
            if (statusType == StatusType.None)
                continue;

            statusInfo.StatusDic.Add(statusType, new StatusElement() { name = statusType.ToString(), type = statusType });
        }
    }
}
