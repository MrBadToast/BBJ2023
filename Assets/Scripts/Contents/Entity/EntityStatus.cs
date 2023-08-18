using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class EntityStatus : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected StatusInfoData originStatusData;
    [SerializeField]
    public StatusInfo currentStatus;

    [SerializeField]
    protected GameObject uiDamageText;

    [SerializeField]
    protected bool useHpGauge = false;

    [ShowIf("useHpGauge")]
    [SerializeField]
    protected bool autoCreateHpGauge = false;

    [ShowIf("autoCreateHpGauge")]
    [SerializeField]
    protected GameObject uiHpGaugePrefab;

    [ShowIf("useHpGauge"), HideIf("autoCreateHpGauge")]
    [SerializeField]
    protected UIStatusGauge uiHpGauge;

    public bool isDeath = false;
    public bool isCanHit = true;

    public UnityEvent damageEvent;
    public UnityEvent<float, float> updateHpEvent;
    public UnityEvent<float, float> updateMpEvent;

    public UnityEvent<bool, EntityStatus> updateDeathEvent;

    protected virtual void Awake()
    {
        SetOriginStatus();
    }

    protected virtual void Start()
    {
        var hpElement = currentStatus.GetElement(StatusType.HP);
        var maxHPElement = currentStatus.GetElement(StatusType.MaxHP);

        hpElement.updateAmountAction += (value) =>
        {
            updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        };

        maxHPElement.updateAmountAction += (value) =>
        {
            updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        };

        updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        
        var mpElement = currentStatus.GetElement(StatusType.MP);
        var maxMPElement = currentStatus.GetElement(StatusType.MaxMP);

        mpElement.updateAmountAction += (value) =>
        {
            updateMpEvent?.Invoke(mpElement.CalculateTotalAmount(), maxMPElement.CalculateTotalAmount());
        };

        maxMPElement.updateAmountAction += (value) =>
        {
            updateMpEvent?.Invoke(mpElement.CalculateTotalAmount(), maxMPElement.CalculateTotalAmount());
        };

        updateMpEvent?.Invoke(mpElement.CalculateTotalAmount(), maxMPElement.CalculateTotalAmount());
        

        if (useHpGauge)
        {
            if (autoCreateHpGauge)
            {
                var uiGauge = UIController.Instance.CreateWorldUI(uiHpGaugePrefab);
                uiHpGauge = uiGauge.GetComponent<UIStatusGauge>();
                uiGauge.GetComponent<UITargetFollower>().SetTarget(this.transform);
            }

            uiHpGauge.UpdateStatusInfo(this);
        }
    }

    public virtual void SetOriginStatus()
    {
        if (originStatusData != null)
            currentStatus.Copy(originStatusData.StausDic);
    }

    public virtual void SetCurrentStatus(StatusInfo statusInfo)
    {
        currentStatus = statusInfo;
    }

    public virtual bool GetCriticalSuccess()
    {
        var tryPercent = Random.Range(0f, 100f);
        return tryPercent <= currentStatus.GetElement(StatusType.CriticalPercent).CalculateTotalAmount();
    }

    [Button("µ¥¹ÌÁö")]
    public virtual DamageInfo OnDamage(DamageInfo damageInfo)
    {
        if (isDeath || !isCanHit)
        {
            damageInfo.isHit = false;
            damageInfo.isKill = false;
            return damageInfo;
        }

        damageInfo.isHit = true;

        var hpElement = currentStatus.GetElement(StatusType.HP);
        var maxHPElement = currentStatus.GetElement(StatusType.MaxHP);

        var damageText = UIController.Instance.CreateWorldUI(uiDamageText);
        damageText.GetComponent<UIDamageText>().SetDamageAmount(damageInfo.hitPoint, damageInfo.damage);

        hpElement.SubAmount(damageInfo.damage);

        updateHpEvent?.Invoke(hpElement.CalculateTotalAmount(), maxHPElement.CalculateTotalAmount());
        damageEvent?.Invoke();

        if (damageInfo.isKnockBack)
        {
            var knockBackable = GetComponent<IKnockBackable>();
            if (knockBackable != null)
            {
                knockBackable.OnKnockBack(damageInfo.hitDirection, damageInfo.knockBackPower
                    , damageInfo.knockBackXAxisCurve, damageInfo.knockBackYAxisCurve, damageInfo.knockBackTime);
            }
        }

        if (hpElement.CalculateTotalAmount() <= 0)
        {
            isDeath = true;
            damageInfo.isKill = true;

            if(autoCreateHpGauge && uiHpGauge != null)
            {
                Destroy(uiHpGauge.gameObject);
            }
            updateDeathEvent?.Invoke(isDeath, this);
        }

        return damageInfo;
    }


}
