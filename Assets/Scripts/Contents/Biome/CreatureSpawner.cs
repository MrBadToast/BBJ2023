using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreatureSpawner : MonoBehaviour
{
    // 스폰을 위한 생물들의 데이터
    public List<CreatureScriptableObject> creatures;
    private List<float> spawnProbability;

    // spawn timer
    public float spawnTimerMax;
    private float spawnTimer;

    // Unity Events
    public UnityEvent<CreatureScriptableObject> beforeSpawnCreatureEvent;
    public UnityEvent<GameObject> AfterSpawnCreatureEvent;

    private void Start()
    {
        spawnTimer = 1000f;
        spawnProbability = new List<float>();

        foreach (var creature in creatures)
        {
            spawnProbability.Add(creature.spawnProbability);
        }

        spawnTimer = spawnTimerMax;
    }

    private void Update()
    {
        // 생물을 스폰하기 위한 타이머
        if (spawnTimer < 0)
        {
            // 타이머 초기화
            spawnTimer = spawnTimerMax;
            SpawnCreature();
        }

        spawnTimer -= Time.deltaTime;
    }

    private void SpawnCreature()
    {
        // Spawn할 생물을 선정
        CreatureScriptableObject spawnCreature = RandomUtility.Pickup(spawnProbability, creatures);
        beforeSpawnCreatureEvent.Invoke(spawnCreature);
        
        GameObject createdObject = Instantiate(spawnCreature.creaturePrefab, spawnCreature.spawnPoint.transform.position, Quaternion.identity);
        
        AfterSpawnCreatureEvent.Invoke(createdObject);
    }
}
