using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum StatusType
{
    None = 0,
    /// <summary>
    /// 수면
    /// </summary>
    Surface = 100,
    /// <summary>
    /// 습도
    /// </summary>
    Humidity = 101,
    /// <summary>
    /// 온도
    /// </summary>
    Temperature = 102,
}
