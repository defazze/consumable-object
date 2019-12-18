
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateAfter(typeof(StepPhysicsWorld))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
unsafe public class PlayerCollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld _physicsWorldSystem;
    private EndSimulationEntityCommandBufferSystem _commandBufferSystem;

    [RequireComponentTag(typeof(Player))]
    struct CollisionJob : IJobForEachWithEntity<PhysicsCollider, Translation, Rotation>
    {
        [ReadOnly] public PhysicsWorld physicsWorld;
        [ReadOnly] public ComponentDataFromEntity<ConsumableObject> consumedObjectIndex;
        public EntityCommandBuffer.Concurrent commandBuffer;

        public void Execute(Entity e, int index, ref PhysicsCollider collider, ref Translation translation, ref Rotation rotation)
        {
            ColliderDistanceInput distanceInput = new ColliderDistanceInput
            {
                Collider = collider.ColliderPtr,
                MaxDistance = .01f,
                Transform = new RigidTransform(rotation.Value, translation.Value),
            };

            NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);
            var collisionWorld = physicsWorld.CollisionWorld;
            if (collisionWorld.CalculateDistance(distanceInput, ref hits))
            {
                foreach (var hit in hits)
                {
                    var entity = collisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                    if (entity != e)
                    {
                        if (consumedObjectIndex.HasComponent(entity))
                        {
                            var consumedEntity = commandBuffer.CreateEntity(index);
                            var consumedObject = consumedObjectIndex[entity];
                            commandBuffer.AddComponent(index, consumedEntity, consumedObject);
                            commandBuffer.AddComponent(index, consumedEntity, new ConsumedTag());
                            commandBuffer.DestroyEntity(index, entity);

                        }
                    }
                }
            }

            hits.Dispose();
        }
    }

    protected override void OnCreate()
    {
        _physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
        _commandBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var consumedObjectIndex = GetComponentDataFromEntity<ConsumableObject>();

        var collisionJob = new CollisionJob
        {
            physicsWorld = _physicsWorldSystem.PhysicsWorld,
            commandBuffer = _commandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            consumedObjectIndex = consumedObjectIndex
        };

        var jobHandle = collisionJob.Schedule(this, inputDeps);
        _commandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}

