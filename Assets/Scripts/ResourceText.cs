using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class ResourceText : MonoBehaviour
{
    void Start()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var text = entityManager.CreateEntity();
        entityManager.AddComponentObject(text, GetComponent<Text>());
    }
}
