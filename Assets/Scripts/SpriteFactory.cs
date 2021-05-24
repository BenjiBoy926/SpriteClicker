using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteFactory
{
    [Tooltip("Prefab instantiated when the factor is called to create a sprite")]
    public SpriteComponent spritePrefab;

    public SpriteComponent CreateSprite()
    {
        return Object.Instantiate(spritePrefab);
    }
}
