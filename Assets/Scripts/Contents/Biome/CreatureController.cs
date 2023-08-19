using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CreatureController : MonoBehaviour
{
    [SerializeField]
    private AmountRangeFloat lifeTimeRange;
    private float lifeTime;

    [SerializeField] 
    private float signatureTimerMax;
    private float signatureTimer;
    
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
            DestroyCreature();
        }

        if (signatureTimer < 0)
        {
            signatureTimer = signatureTimerMax;

            if (Random.Range(0, 2).Equals(0))
            {  
                _animator.SetTrigger("signature");
            }
        }

        signatureTimer -= Time.deltaTime;
        lifeTime -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        this.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void DestroyCreature()
    {
        _animator.SetTrigger("deSpawn");
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        OnDestroyEvent.Invoke();
    }
}
