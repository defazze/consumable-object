using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ConsunableObjectPrefab : MonoBehaviour, IConvertGameObjectToEntity
{
    public ConsumableObjectType type;
    public int resourceCount;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ConsumableObject { type = type, resourceCount = resourceCount });
    }

}
