using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawnArea : MonoBehaviour
{
    public Vector3 spawnAreaSize = Vector3.one;

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, spawnAreaSize * 0.5f);
    }

    public Vector3 GetRandomPosition()
    {
        return transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x * 0.5f, spawnAreaSize.x * 0.5f)
            , Random.Range(-spawnAreaSize.y * 0.5f, spawnAreaSize.y * 0.5f)
            , Random.Range(-spawnAreaSize.z * 0.5f, spawnAreaSize.z * 0.5f));
    }
}
