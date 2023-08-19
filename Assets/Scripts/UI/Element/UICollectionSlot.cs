using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollectionSlot : MonoBehaviour
{
    private UICollectionListview collectionListview;
    private CreatureData creatureData;

    [SerializeField]
    private UIBaseImage iconImage;

    public void SetData(UICollectionListview collectionListview, CreatureData creatureData, bool isShow = false)
    {
        this.collectionListview = collectionListview;
        this.creatureData = creatureData;

        iconImage.SetImage(creatureData.Icon);
        if (!isShow)
        {
            iconImage.SetColor(Color.black);
        }
    }

    public void OnPointerEnter()
    {
        collectionListview.ShowInfoPopup(transform.position, creatureData);
    }

    public void OnPointerExit()
    {
        collectionListview.HideInfoPopup();
    }
}
