using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollectionListview : UIListView<UICollectionSlot>
{
    [SerializeField]
    private CollectionContainer collectionContainer;

    public void Start()
    {
        var collectionKeyList = SaveLoadSystem.Instance.SaveLoadData.collectionKeyList;
    }



}
