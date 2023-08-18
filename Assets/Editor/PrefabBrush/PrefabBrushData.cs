using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabBrushElement
{
    public GameObject prefabObject;
    public float percent = 100f;
}

[CreateAssetMenu(fileName = "PrefabBrushData", menuName = "PrefabBrush/BrushData", order = 0)]
public class PrefabBrushData : ScriptableObject
{
    [SerializeField]
    private List<PrefabBrushElement> brushElementList = new List<PrefabBrushElement>();
    public List<PrefabBrushElement> BrushElementList => brushElementList;

    public GameObject GetRandomPrefab()
    {
        var pickupList = new List<GameObject>();
        var percentList = new List<float>();

        for (var i = 0; i < brushElementList.Count; ++i)
        {
            pickupList.Add(brushElementList[i].prefabObject);
            percentList.Add(brushElementList[i].percent);
        }

        return RandomUtility.Pickup<GameObject>(percentList, pickupList);
    }
}
