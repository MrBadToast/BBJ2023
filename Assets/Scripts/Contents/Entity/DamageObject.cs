using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class DamageObject : MonoBehaviour
{
    [System.Serializable]
    public enum StateType
    {
        Wait,
        Hit,
        Attack,
    }

    [SerializeField]
    private StateType stateType = StateType.Wait;

    [SerializeField]
    [ReadOnly]
    protected EntityStatus entityStatus;

    [SerializeField]
    protected bool isFollowOwner = false;
    [SerializeField]
    [ReadOnly]
    protected Transform ownerTransform;

    [SerializeField]
    [ReadOnly]
    protected List<GameObject> hitObjectList = new List<GameObject>();
    [SerializeField]
    [ReadOnly]
    protected List<Collider2D> hitColliderList = new List<Collider2D>();

    [SerializeField]
    protected LayerMask hitLayerMask;

    [HideInInspector]
    public UnityEvent<Collider2D> hitEvent;

    [SerializeField]
    protected StatusCalculator damageCalculator;

    [SerializeField]
    protected bool isMultiHitable = false;

    [SerializeField]
    protected GameObject hitVFXPrefab;

    [SerializeField]
    [ShowIf("isMultiHitable")]
    protected int hitCount;

    [SerializeField]
    [ShowIf("isMultiHitable")]
    protected float hitLifeTime;

    protected virtual void Start()
    {
        stateType = StateType.Hit;
    }

    public virtual void SetFollower(Transform owner)
    {
        isFollowOwner = true;
        ownerTransform = owner;
    }

    public virtual void ChangeState(StateType stateType)
    {
        this.stateType = stateType;
    }

    public void SetEntityStatus(EntityStatus entityStatus)
    {
        this.entityStatus = entityStatus;
    }

    public void SetMultiHit(int hitCount, float hitLifeTime)
    {
        isMultiHitable = true;
        this.hitCount = hitCount;
        this.hitLifeTime = hitLifeTime;
    }

    public void SetHitVFX(GameObject hitVFXPrefab)
    {
        this.hitVFXPrefab = hitVFXPrefab;
    }

    public void SetDamageCalculator(StatusCalculator damageCalculator)
    {
        this.damageCalculator = damageCalculator;
    }

    protected virtual void FixedUpdate()
    {
        if (!isFollowOwner)
            return;

        transform.position = ownerTransform.position;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (stateType != StateType.Hit)
        {
            return;
        }

        var entityObject = collision.gameObject;

        if (!hitLayerMask.Contains(entityObject.layer) || hitObjectList.Contains(entityObject))
        {
            return;
        }

        hitObjectList.Add(entityObject);
        hitColliderList.Add(collision);

        hitEvent.Invoke(collision);
    }

    public virtual void Invoke()
    {
        foreach (var hitCollider in hitColliderList)
        {
            hitEvent.Invoke(hitCollider);
        }
    }

    protected virtual void Clear()
    {
        hitObjectList?.Clear();
        hitEvent?.RemoveAllListeners();
    }


    public virtual void StartAttack()
    {
        if (isMultiHitable)
            StartCoroutine(UpdateTickAttack());
        else
            AttackHit();
    }

    public virtual void AttackHit()
    {
        foreach (var entity in hitObjectList)
        {
            var damageable = entity.GetComponent<IDamageable>();

            if (damageable == null)
                return;

            var damageInfo = new DamageInfo();
            damageInfo.damage = damageCalculator.Calculate(entityStatus.currentStatus);
            damageInfo.hitPoint = entity.transform.position;

            var hitVFX = Instantiate(hitVFXPrefab);
            hitVFX.transform.position = damageInfo.hitPoint;

            damageable.OnDamage(damageInfo);
        }
    }

    IEnumerator UpdateTickAttack()
    {
        var remainHitCount = hitCount;
        var tickTime = hitLifeTime / hitCount;
        var waitForTick = new WaitForSecondsRealtime(tickTime);

        while (remainHitCount > 0)
        {
            AttackHit();
            --remainHitCount;
            yield return waitForTick;
        }

        Destroy();
    }

    protected virtual void OnDisable()
    {
        Clear();
    }

    protected virtual void OnDestroy()
    {
        Clear();
    }

    protected virtual void Destroy() {
        Destroy(this.gameObject);
    }

}
