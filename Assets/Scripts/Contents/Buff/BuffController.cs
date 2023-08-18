using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BuffController : MonoBehaviour
{
    [SerializeField]
    private EntityStatus entityStatus;
    public EntityStatus EntityStatus { get { return entityStatus; } }

    public List<BuffBehaviour> buffBehaviourList;

    private void Awake()
    {
        entityStatus = GetComponent<EntityStatus>();
    }

    public void AddBuff(BuffData buffData)
    {
        Debug.Log($"buffData.name : {buffData.name}");
        var buffObject = Instantiate(buffData.BuffBehaviourObject, transform);

        var buffBehaviour = buffObject.GetComponent<BuffBehaviour>();
        buffBehaviourList.Add(buffBehaviour);

        buffBehaviour.SetBuffData(buffData);
        buffBehaviour.SetBuffController(this);

        buffBehaviour.StartBuff();
    }


    public void RemoveBuff(BuffData buffData)
    {
        var buffBehaviour = buffBehaviourList.Find(item => item.EqualBuffData(buffData));
        buffBehaviour.EndBuff();
    }

    public void RemoveBuff(BuffBehaviour buffBehaviour)
    {
        if (buffBehaviour == null)
            return;

        buffBehaviourList.Remove(buffBehaviour);
        Destroy(buffBehaviour.gameObject);
    }

    public void RemoveAllBuffByCrowd(CrowdType crowdType)
    {
        buffBehaviourList.RemoveAll(item => item.CrowdType == crowdType);
    }

}
