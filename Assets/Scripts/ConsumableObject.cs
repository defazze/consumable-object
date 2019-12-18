
using Unity.Entities;

public struct ConsumableObject : IComponentData
{
    public ConsumableObjectType type;
    public int resourceCount;
}
