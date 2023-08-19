using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CollectionContainer", menuName = "Collcetion/CollectionContainer", order = 0)]

public class CollectionContainer : ScriptableObject
{
    [SerializeField]
    private SerializableDictionary<string, CreatureData> dataTable;
    public SerializableDictionary<string, CreatureData> DataTable => dataTable;




}
