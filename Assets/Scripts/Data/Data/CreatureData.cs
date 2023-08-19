using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Creature/CreatureData")]
public class CreatureData : ScriptableObject
{
    [SerializeField]
    private string key;
    public string Key => key;

    [SerializeField]
    [TextArea]
    private string creatureName;
    public string CreatureName => creatureName;

    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    [TextArea]
    private string context;
    public string Context => context;

    [SerializeField]
    private GameObject creaturePrefab;
    // 생물의 프리팹
    public GameObject CreaturePrefab => creaturePrefab;

    // 생물 스폰에 관한 정보
    [SerializeField]
    private float spawnProbability;
    public float SpawnProbability => spawnProbability;
}
