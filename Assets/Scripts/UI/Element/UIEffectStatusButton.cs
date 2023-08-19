using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIEffectStatusButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private WorldController worldController;

    [SerializeField]
    private EffectStatusInfoData effectStatusInfoData;

    public UnityEvent onPointerEnterEvent;
    public UnityEvent onPointerExitEvent;
    public UnityEvent onClickEvent;

    private void Awake()
    {
        worldController = FindObjectOfType<WorldController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                worldController?.UpdateStatus(effectStatusInfoData.AddStatus, effectStatusInfoData.SubStatus);
                break;
            case PointerEventData.InputButton.Right:
                worldController?.UpdateStatus(effectStatusInfoData.SubStatus, effectStatusInfoData.AddStatus);
                break;
            case PointerEventData.InputButton.Middle:
                break;
        }

        onClickEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExitEvent?.Invoke();
    }
}
