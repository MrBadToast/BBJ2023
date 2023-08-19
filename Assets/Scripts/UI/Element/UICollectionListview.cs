using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICollectionListview : UIListView<UICollectionSlot>
{
    [SerializeField]
    private CollectionContainer collectionContainer;

    [SerializeField]
    private GameObject infoPopup;
    [SerializeField]
    private UIBaseText infoNameText;
    private UIBaseText infoDescriptionText;
    private UIBaseImage infoIconImage;

    public void Start()
    {
        CreateCollectionSlots();
    }


    public void CreateCollectionSlots()
    {
        var haveKeyList = SaveLoadSystem.Instance.SaveLoadData.collectionKeyList;
        var keys = new List<string>();
        foreach (var key in collectionContainer.DataTable.Keys)
        {
            keys.Add(key);
        }

        foreach (var haveKey in haveKeyList)
        {
            keys.Remove(haveKey);
        }

        foreach (var key in haveKeyList)
        {
            var content = AddContent();
            content.SetData(this, collectionContainer.DataTable[key], true);
        }

        foreach (var key in keys)
        {
            var content = AddContent();
            content.SetData(this, collectionContainer.DataTable[key], false);
        }
    }

    public void ShowInfoPopup(Vector3 position, CreatureData creatureData)
    {
        infoNameText.SetText(creatureData.CreatureName);
        infoDescriptionText.SetText(creatureData.Context);
        infoIconImage.SetImage(creatureData.Icon);

        infoPopup.SetActive(true);
    }

    public void HideInfoPopup()
    {
        infoPopup.SetActive(false);
    }
}
