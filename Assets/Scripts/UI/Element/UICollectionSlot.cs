using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollectionSlot : MonoBehaviour
{
    private UICollectionListview collectionListview;
    private CreatureData creatureData;
    private bool isHave = false;

    [SerializeField]
    private UIBaseImage iconImage;
    [SerializeField]
    private UIBaseText nameText;

    public void SetData(UICollectionListview collectionListview, CreatureData creatureData, bool isShow = false)
    {
        this.collectionListview = collectionListview;
        this.creatureData = creatureData;

        iconImage.SetImage(creatureData.Icon);
        if (!isShow)
        {
            iconImage.SetColor(Color.white);
            nameText.SetText("???");
        }
        else
        {
            nameText.SetText(creatureData.CreatureName);
        }

        isHave = isShow;
    }

    public void OnSelect()
    {
        collectionListview.ShowInfoPopup(transform.position, creatureData, isHave);
    }

}
