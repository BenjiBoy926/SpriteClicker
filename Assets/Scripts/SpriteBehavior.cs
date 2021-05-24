using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteBehavior", menuName ="SpriteBehavior")]
public class SpriteBehavior : ScriptableObject
{
    [Header("Movement")]

    [Tooltip("Speed that the sprite moves at")]
    public OptionallyRandomFloat speed;
    [Tooltip("Time sprite moves in the same direction before changing directions")]
    public OptionallyRandomFloat directionSwitchInterval;
    [Tooltip("Space inside the viewing area that the sprite will not venture out of")]
    public Vector2 onscreenMargins;

    // List of available directions for the sprite
    private List<Vector2> availableDirections = new List<Vector2>()
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right,
        Vector2.one, -Vector2.one, new Vector2(-1, 1), new Vector2(1, -1)
    };

    [Header("Happy/Angry")]

    [Tooltip("Color of the sprite when it is in the happy state")]
    public Color happyColor;
    [Tooltip("Color of the sprite when it is in the angry state")]
    public Color angryColor;
    [Tooltip("Interval that the sprite waits before becoming angry")]
    public OptionallyRandomFloat angryInterval;
    [Tooltip("The amount of time that the sprite stays angry")]
    public OptionallyRandomFloat angryDuration;
    [HideInInspector]
    public bool isAngry = false;

    public IEnumerator MovementRoutine(Rigidbody2D rb, Camera viewingCamera)
    {
        Bounds view = CameraViewingBounds(viewingCamera);
        // Time of the app when the last direction switched
        float directionSwitchCurrent;
        // Time of the app when the next direction switch should start
        float directionSwitchNext;
        // Directions available to the sprite to randomly move in
        List<Vector2> curatedDirections;
        // Selected direction out of the directions available
        Vector2 selectedDirection;

        while(true)
        {
            // Curate the available directions, then set the velocity to a random direction
            curatedDirections = CurateAvailableDirections(rb, view);
            selectedDirection = curatedDirections[Random.Range(0, curatedDirections.Count)];
            rb.velocity = selectedDirection.normalized * speed.Get();

            // Set the time of this switch and the next switch
            directionSwitchCurrent = Time.time;
            directionSwitchNext = Time.time + directionSwitchInterval.Get();

            // Wait until it is time to switch directions, or until we are too far offscreen
            yield return new WaitUntil(() =>
            {
                return Time.time > directionSwitchNext || !view.Contains(rb.position);
            });
        }
    }

    public IEnumerator AngerRoutine(SpriteRenderer renderer)
    {
        SetAngry(false, renderer);

        while(true)
        {
            // Wait for the angry interval, then become angry
            yield return new WaitForSeconds(angryInterval.Get());
            SetAngry(true, renderer);

            // Wait for the angry duration, then become happy again
            yield return new WaitForSeconds(angryDuration.Get());
            SetAngry(false, renderer);
        }
    }

    public void SetAngry(bool angry, SpriteRenderer renderer)
    {
        isAngry = angry;
        if (angry) renderer.color = angryColor; 
        else renderer.color = happyColor;
    }

    private Bounds CameraViewingBounds(Camera camera)
    {
        // Get the center of the viewing area
        Vector3 center = camera.transform.position;

        // Get the size of the viewing area using the 
        float verticalSize = camera.orthographicSize * 2f;
        float horizontalSize = verticalSize * camera.aspect;
        Vector3 size = new Vector3(horizontalSize - (2f * onscreenMargins.x), verticalSize - (2f * onscreenMargins.y), 100f);

        // Return a bounds that represents the viewing bounds of the camera
        return new Bounds(center, size);
    }

    // Modify the directions available for the sprite to move 
    // based on the position of the rigidbody relative to the view's bounds
    private List<Vector2> CurateAvailableDirections(Rigidbody2D rb, Bounds viewBounds)
    {
        List<Vector2> curatedDirections = new List<Vector2>(availableDirections);

        // If the sprite is too far to the left, remove all left-bound directions
        if(rb.position.x < viewBounds.min.x)
        {
            curatedDirections.RemoveAll(v => v.x < 0.1f);
        }
        // If the sprite is too far to the right, remove all right-bound directions
       if(rb.position.x > viewBounds.max.x)
        {
            curatedDirections.RemoveAll(v => v.x > -0.1f);
        }
        // If the sprite is too far down, remove all down-bound directions
        if (rb.position.y < viewBounds.min.y)
        {
            curatedDirections.RemoveAll(v => v.y < 0.1f);
        }
        // If the sprite is too far up, remove all upward-bound directions
        if (rb.position.y > viewBounds.max.y)
        {
            curatedDirections.RemoveAll(v => v.y > -0.1f);
        }

        return curatedDirections;
    }
}
