using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class ColliderEventSender : MonoBehaviour
{
    [SerializeField]
    private LayerMask hitLayerMask;

    [FoldoutGroup("Ȱ��ȭ �̺�Ʈ")]
    public UnityEvent<GameObject> enableEvent;
    [FoldoutGroup("Ȱ��ȭ �̺�Ʈ")]
    public UnityEvent<GameObject> disableEvent;

    [FoldoutGroup("Collision �̺�Ʈ")]
    public UnityEvent<Collision2D> collisionEnterEvent;
    [FoldoutGroup("Collision �̺�Ʈ")]
    public UnityEvent<Collision2D> collisionStayEvent;
    [FoldoutGroup("Collision �̺�Ʈ")]
    public UnityEvent<Collision2D> collisionExitEvent;

    [FoldoutGroup("Trigger �̺�Ʈ")]
    public UnityEvent<Collider2D> triggerEnterEvent;
    [FoldoutGroup("Trigger �̺�Ʈ")]
    public UnityEvent<Collider2D> triggerStayEvent;
    [FoldoutGroup("Trigger �̺�Ʈ")]
    public UnityEvent<Collider2D> triggerExitEvent;

    private void OnEnable()
    {
        enableEvent?.Invoke(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hitLayerMask.Contains(collision.gameObject.layer))
        {
            return;
        }

        collisionEnterEvent?.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!hitLayerMask.Contains(collision.gameObject.layer))
        {
            return;
        }

        collisionStayEvent?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!hitLayerMask.Contains(collision.gameObject.layer))
        {
            return;
        }

        collisionExitEvent?.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hitLayerMask.Contains(other.gameObject.layer))
        {
            return;
        }

        triggerEnterEvent?.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!hitLayerMask.Contains(other.gameObject.layer))
        {
            return;
        }

        triggerStayEvent?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!hitLayerMask.Contains(other.gameObject.layer))
        {
            return;
        }

        triggerExitEvent?.Invoke(other);
    }

    private void OnDisable()
    {
        disableEvent?.Invoke(gameObject);
    }
}
