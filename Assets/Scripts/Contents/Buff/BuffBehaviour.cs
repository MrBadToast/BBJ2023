using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public abstract class BuffBehaviour : MonoBehaviour
{
    [SerializeField]
    protected BuffController controller;

    [SerializeField]
    protected BuffData buffData;

    [SerializeField]
    protected CrowdType crowdType;
    public CrowdType CrowdType { get { return crowdType; } }

    [SerializeField]
    protected StatusInfo currentBuffStatus;

    [SerializeField]
    protected float currentLifeTime;

    [SerializeField]
    protected bool isActive = false;

    [ShowInInspector]
    [ReadOnly]
    protected int stackCount = 0;

    public GameObject prefabsEffect = null;
    protected GameObject effectObject = null;

    public UnityEvent<BuffBehaviour> startBuffEvent;
    public UnityEvent<BuffBehaviour, float> updateBuffEvent;
    public UnityEvent<BuffBehaviour> endBuffEvent;

    public bool EqualBuffData(BuffData buffData)
    {
        return this.buffData == buffData;
    }

    public virtual void SetBuffController(BuffController controller)
    {
        this.controller = controller;
    }

    public virtual void SetBuffData(BuffData buffData)
    {
        this.buffData = buffData;
    }

    public BuffData GetBuffData()
    {
        return buffData;
    }

    public float GetLifeTime() {
        return currentLifeTime;
    }

    public virtual void Initialized()
    {
        if (buffData == null)
            return;

        currentBuffStatus.Copy(buffData.StausDic);
    }

    public virtual void StartBuff()
    {
        Initialized();

        isActive = true;
        ResetLifeTime();

        var targetStatus = controller.EntityStatus;
        targetStatus.currentStatus.AddStatusInfo(currentBuffStatus);

        startBuffEvent?.Invoke(this);
        updateBuffEvent?.Invoke(this, currentLifeTime);
    }

    protected virtual void Update()
    {
        if (!isActive)
            return;

        currentLifeTime -= Time.deltaTime;
        updateBuffEvent?.Invoke(this, currentLifeTime);

        if (currentLifeTime <= 0f)
        {
            EndBuff();
        }
    }

    public virtual void EndBuff()
    {
        isActive = false;

        var targetStatus = controller.EntityStatus;
        targetStatus.currentStatus.SubStatusInfo(currentBuffStatus);

        controller.RemoveBuff(this);
        endBuffEvent?.Invoke(this);
    }

    public void AddStack()
    {
        ++stackCount;
        if (buffData.IsOverlap)
        {
            ResetLifeTime();
        }
        else
        {
            AddLifeTime();
        }
    }

    public virtual void AddLifeTime()
    {
        currentLifeTime += buffData.LifeTime;
    }
    public virtual void AddLifeTime(float addLifeTime)
    {
        currentLifeTime += addLifeTime;
    }

    public virtual void ResetLifeTime()
    {
        currentLifeTime = buffData.LifeTime;
    }
}


