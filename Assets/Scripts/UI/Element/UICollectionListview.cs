using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICollectionListview : UIListView<UICollectionSlot>
{
    [SerializeField]
    private CollectionContainer collectionContainer;

    [SerializeField]
    private UIBaseText infoNameText;
    [SerializeField]
    private UIBaseText infoDescriptionText;
    [SerializeField]
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

        bool isInitialized = false;

        foreach (var key in haveKeyList)
        {
            var content = AddContent();
            content.SetData(this, collectionContainer.DataTable[key], true);

            if (!isInitialized)
            {
                content.OnSelect();
                isInitialized = true;
            }

        }

        foreach (var key in keys)
        {
            var content = AddContent();
            content.SetData(this, collectionContainer.DataTable[key], false);

            if (!isInitialized)
            {
                content.OnSelect();
                isInitialized = true;
            }
        }
    }

    public void ShowInfoPopup(Vector3 position, CreatureData creatureData, bool isHave)
    {
        infoIconImage.SetImage(creatureData.Icon);
        if (isHave)
        {
            infoIconImage.SetColor(Color.white);
            infoNameText.SetText(creatureData.CreatureName);
            infoDescriptionText.SetText(creatureData.Context);
        }
        else
        {
            infoIconImage.SetColor(Color.black);
            infoNameText.SetText("???");
            infoDescriptionText.SetText("아직 확인되지 않았습니다.");
        }

    }
}
