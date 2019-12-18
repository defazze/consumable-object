
using Unity.Entities;
using UnityEngine;

public class PlayerInputSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            var player = GetSingleton<Player>();
            player.target = pos;
            player.moved = true;
            SetSingleton(player);
        }
    }
}
