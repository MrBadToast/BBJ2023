using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableHandler : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EntityStatus rootStatus;

    public DamageInfo OnDamage(DamageInfo damageInfo)
    {
        return rootStatus.OnDamage(damageInfo);
    }
}
