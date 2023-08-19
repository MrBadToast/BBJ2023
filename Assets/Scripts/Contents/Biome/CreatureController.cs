using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreatureController : MonoBehaviour
{
    private float lifeTime;
    public UnityEvent OnCreateEvent;
    public UnityEvent OnDestroyEvent;
    
    private void Awake()
    {
        lifeTime = 10f;
        OnCreateEvent.Invoke();
    }

    public void SetLifeTime(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }

    private void Update()
    {
        if (lifeTime < 0)
        {
            // 삭제
            DestoryCreature();
        }

        lifeTime -= Time.deltaTime;
    }

    public void DestoryCreature()
    {
        // TODO: Animation 추가
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        OnDestroyEvent.Invoke();
    }
}
