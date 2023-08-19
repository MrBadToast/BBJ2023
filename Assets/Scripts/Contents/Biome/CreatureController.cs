using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreatureController : MonoBehaviour
{
    [SerializeField]
    private AmountRangeFloat lifeTimeRange;
    private float lifeTime;

    // Animator
    private Animator _animator;
    
    public UnityEvent OnCreateEvent;
    public UnityEvent OnDestroyEvent;

    private void Awake()
    {
        lifeTime = lifeTimeRange.GetRandomAmount();
    }

    private void Start()
    {
        OnCreateEvent.Invoke();
        _animator = GetComponent<Animator>();
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

    private void LateUpdate()
    {
        this.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void DestoryCreature()
    {
        _animator.SetTrigger("deSpawn");
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        OnDestroyEvent.Invoke();
    }
}
