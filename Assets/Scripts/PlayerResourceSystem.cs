
using Unity.Entities;

public class PlayerResourceSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<ConsumedTag>().ForEach((Entity e, ref ConsumableObject resource) =>
        {
            var tmp = resource;
            Entities.ForEach((ref Player player) =>
            {
                var currentCount = player.resourceCount;
                player.resourceCount = currentCount + tmp.resourceCount;
            });

            PostUpdateCommands.DestroyEntity(e);
        });
    }
}
