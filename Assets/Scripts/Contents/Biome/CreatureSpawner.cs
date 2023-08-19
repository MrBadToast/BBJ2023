using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreatureSpawner : MonoBehaviour
{
    [System.Serializable]
    public class GroupData
    {
        public CreatureData creatureData;
        public CreatureSpawnArea spawnArea;
    }

    // 스폰을 위한 생물들의 데이터
    public List<GroupData> creatureDataList;
    private List<float> spawnProbabilityList;

    // spawn timer
    [SerializeField]
    private AmountRangeFloat spawnTimeRange;

    private float spawnTimer;
    
    // Number of Spawned Creature
    private int _numOfSpawnedCreature;
    
    // Unity Events
    public UnityEvent<CreatureData> beforeSpawnCreatureEvent;
    public UnityEvent<GameObject> AfterSpawnCreatureEvent;

    private void Start()
    {
        spawnTimer = 1000f;
        spawnProbabilityList = new List<float>();

        foreach (var creature in creatureDataList)
        {
            spawnProbabilityList.Add(creature.creatureData.SpawnProbability);
        }

        spawnTimer = spawnTimeRange.GetRandomAmount();
    }

    private void Update()
    {
        // 생물을 스폰하기 위한 타이머
        if (spawnTimer < 0)
        {
            // 타이머 초기화
            spawnTimer = spawnTimeRange.GetRandomAmount();
            SpawnCreature();
        }

        spawnTimer -= Time.deltaTime;
    }

    private void SpawnCreature()
    {
        // Spawn할 생물을 선정
        GroupData groupData = RandomUtility.Pickup(spawnProbabilityList, creatureDataList);
        
        var creatureData = groupData.creatureData;

        beforeSpawnCreatureEvent.Invoke(creatureData);

        GameObject createdObject = Instantiate(creatureData.CreaturePrefab, groupData.spawnArea.GetRandomPosition(), Quaternion.identity);

        ++_numOfSpawnedCreature;
        createdObject.GetComponent<CreatureController>().OnDestroyEvent.AddListener(() =>
        {
            RemovedCreature();
        });
        
        AfterSpawnCreatureEvent.Invoke(createdObject);
    }

    public void RemovedCreature()
    {
        --_numOfSpawnedCreature;
    }
}
