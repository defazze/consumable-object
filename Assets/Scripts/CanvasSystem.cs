using Unity.Entities;
using UnityEngine.UI;

public class CanvasSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Player player) =>
        {
            var tmp = player;

            Entities.ForEach((Text text) =>
            {
                text.text = $"Dye: {tmp.resourceCount}";
            });
        });
    }
}
