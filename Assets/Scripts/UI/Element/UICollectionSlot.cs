using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollectionSlot : MonoBehaviour
{
    [SerializeField]
    private UIBaseImage iconImage;

    public void SetData(CreatureData creatureData, bool isShow = false)
    {
        iconImage.SetImage(creatureData.Icon);
        if (!isShow)
        {
            iconImage.SetColor(Color.black);
        }
    }
}
