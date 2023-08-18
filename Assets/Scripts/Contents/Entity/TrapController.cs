using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField]
    private LayerMask damageableMask;

    [SerializeField]
    private float damageAmount;

    [SerializeField]
    private float knockBackPower;
    [SerializeField]
    private AnimationCurve knockBackXAxisCurve;
    [SerializeField]
    private AnimationCurve knockBackYAxisCurve;
    [SerializeField]
    private float knockBackTime;
    [SerializeField]
    private Vector2 knockBackDirection;

    [SerializeField] 
    private bool isReSpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!damageableMask.Contains(collision.gameObject.layer))
            return;

        var damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable == null)
            return;

        var damageInfo = new DamageInfo();

        damageInfo.ownerTag = "Trap";
        damageInfo.damage = damageAmount;
        damageInfo.hitPoint = collision.transform.position;

        damageInfo.isKnockBack = true;
        damageInfo.knockBackPower = knockBackPower;
        damageInfo.knockBackXAxisCurve = knockBackXAxisCurve;
        damageInfo.knockBackYAxisCurve = knockBackYAxisCurve;
        damageInfo.knockBackTime = knockBackTime;

        var diffDirection = Mathf.Sign(collision.transform.position.x - transform.position.x);
        damageInfo.hitDirection = knockBackDirection;
        damageInfo.hitDirection.x *= diffDirection;
        damageInfo.isReSpawn = isReSpawn;

        damageable.OnDamage(damageInfo);
    }
}
