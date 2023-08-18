using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CreatureData", menuName = "ScriptabObject/SpawnCreature")]
public class CreatureScriptableObject : ScriptableObject
{
    // 생물의 프리팹
    public GameObject creaturePrefab;

    // 생물 스폰에 관한 정보
    public float spawnProbability;
    public float spawnTime;
    
    // 생물의 스폰 지점
    public GameObject spawnPoint;
}
