using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageableMask
{
    [SerializeField]
    private string ownerKey;
    [SerializeField]
    private List<string> identityList = new List<string>();

    public bool ContainsOwner(string ownerKey)
    {
        return this.ownerKey == ownerKey;
    }

    public bool ContainsValue(string identityKey)
    {
        return identityList.Contains(identityKey);
    }
}
