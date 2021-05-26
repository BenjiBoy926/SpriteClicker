using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteDirection
{
    [Tooltip("Determine if the sprite can move horizontally")]
    public bool horizontal = true;
    [Tooltip("Determine if the sprite can move vertically")]
    public bool vertical = true;
    [Tooltip("Determine if the sprite can move along the diagonal")]
    public bool diagonal = false;

    public List<Vector2> availableDirections
    {
        get
        {
            List<Vector2> list = new List<Vector2>();

            // Put horizontal into the list
            if(horizontal)
            {
                list.Add(Vector2.right);
                list.Add(Vector2.left);
            }
            // Put vertical into the list
            if(vertical)
            {
                list.Add(Vector2.up);
                list.Add(Vector2.down);
            }
            // Put diagonal into the list
            if(diagonal)
            {
                list.Add(Vector2.one.normalized);
                list.Add(-Vector2.one.normalized);
                list.Add(new Vector2(1, -1).normalized);
                list.Add(new Vector2(-1, 1).normalized);
            }

            return list;
        }
    }

    public List<Vector2> CurateAvailableDirections(Vector3 position, Bounds viewBounds)
    {
        List<Vector2> curatedDirections = new List<Vector2>(availableDirections);

        // If the sprite is too far to the left, remove all left-bound directions
        if (position.x < viewBounds.min.x)
        {
            curatedDirections.RemoveAll(v => v.x < 0.1f);
        }
        // If the sprite is too far to the right, remove all right-bound directions
        if (position.x > viewBounds.max.x)
        {
            curatedDirections.RemoveAll(v => v.x > -0.1f);
        }
        // If the sprite is too far down, remove all down-bound directions
        if (position.y < viewBounds.min.y)
        {
            curatedDirections.RemoveAll(v => v.y < 0.1f);
        }
        // If the sprite is too far up, remove all upward-bound directions
        if (position.y > viewBounds.max.y)
        {
            curatedDirections.RemoveAll(v => v.y > -0.1f);
        }

        return curatedDirections;
    }
}
