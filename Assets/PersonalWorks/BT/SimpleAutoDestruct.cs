using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAutoDestruct : MonoBehaviour
{
    [SerializeField] private float time = 5.0f;
    [SerializeField] private bool autoDestruct = true;

    private void Start()
    {
        if (autoDestruct) Invoke("InvokeDestroy", time);
    }

    public void Destruct(float _time)
    {
        Invoke("InvokeDestroy", _time);
    }

    private void InvokeDestroy()
    {
        Destroy(gameObject);
    }
}
