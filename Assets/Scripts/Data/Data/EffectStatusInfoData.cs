using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "EffectStatusInfoData", menuName = "Status/EffectStatusInfoData", order = 0)]
public class EffectStatusInfoData : ScriptableObject
{
    [SerializeField]
    private StatusInfo addStatus;
    public StatusInfo AddStatus => addStatus;

    [SerializeField]
    private StatusInfo subStatus;
    [SerializeField]
    public StatusInfo SubStatus => subStatus;


    [Button("AutoGenerate")]
    public void AutoGenerate()
    {
        addStatus.StatusDic.Clear();

        IEnumerable<StatusType> StatusTypeList =
                Enum.GetValues(typeof(StatusType)).Cast<StatusType>();

        foreach (StatusType statusType in StatusTypeList)
        {
            if (statusType == StatusType.None)
                continue;

            addStatus.StatusDic.Add(statusType, new StatusElement() { name = statusType.ToString(), type = statusType });
            subStatus.StatusDic.Add(statusType, new StatusElement() { name = statusType.ToString(), type = statusType });
        }
    }

}
