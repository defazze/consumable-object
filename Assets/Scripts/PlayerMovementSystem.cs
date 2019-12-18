
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerMovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Player player, ref Translation translation) =>
        {
            if (player.moved)
            {
                var newPos = Vector3.MoveTowards(translation.Value, player.target, Time.DeltaTime * 2);
                translation.Value = newPos;

                if (Vector3.Distance(newPos, player.target) <= .05f)
                {
                    player.moved = false;
                }
            }
        });
    }
}
