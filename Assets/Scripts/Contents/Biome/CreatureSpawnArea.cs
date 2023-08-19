using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawnArea : MonoBehaviour
{
    [SerializeField]
    private Vector3 spawnAreaSize = Vector3.one;
    [SerializeField]
    private bool useSnapFloor = false;

    [SerializeField]
    private Color areaColor = Color.blue;

    private void OnDrawGizmos()
    {
        Gizmos.color = areaColor;
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }

    public Vector3 GetRandomPosition()
    {
        var randomPosition = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x * 0.5f, spawnAreaSize.x * 0.5f)
            , Random.Range(-spawnAreaSize.y * 0.5f, spawnAreaSize.y * 0.5f)
            , Random.Range(-spawnAreaSize.z * 0.5f, spawnAreaSize.z * 0.5f));

        if (useSnapFloor)
        {
            RaycastHit hit;
            Physics.Raycast(randomPosition, Vector3.down, out hit);
            if (hit.collider != null)
            {
                return hit.point;
            }
        }

        return randomPosition;
    }
}
