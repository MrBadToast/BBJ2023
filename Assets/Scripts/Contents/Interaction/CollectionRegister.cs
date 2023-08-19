using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionRegister : MonoBehaviour
{
    [SerializeField]
    private CreatureData creatureData;

    private void Start()
    {
        SaveLoadSystem.Instance.AddCollection(creatureData.Key);
    }
}
