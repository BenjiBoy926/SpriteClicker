using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteSpawner : MonoBehaviour
{
    [Tooltip("Factory object used to create the sprites")]
    public SpriteFactory factory;
    [Tooltip("Total number of sprites to created")]
    public int totalSprites = 3;
    [Tooltip("Event invoked when any sprite component is successfully clicked")]
    public UnityEvent<SpriteComponent> anySpriteClickedEvent;

    private void Start()
    {
        for(int i = 0; i < totalSprites; i++)
        {
            SpriteComponent copy = factory.CreateSprite();
            copy.successfulClickEvent.AddListener(() => anySpriteClickedEvent.Invoke(copy));
        }
    }
}
