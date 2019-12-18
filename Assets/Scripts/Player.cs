
using Unity.Entities;
using Unity.Mathematics;

public struct Player : IComponentData
{
    public float3 target;
    public bool moved;

    public int resourceCount;
}
