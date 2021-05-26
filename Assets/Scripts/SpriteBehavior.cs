using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteBehavior", menuName ="Sprite Behavior")]
public class SpriteBehavior : ScriptableObject
{
    [Header("Movement")]

    [Tooltip("Speed that the sprite moves at")]
    public OptionallyRandomFloat speed;
    [Tooltip("Time sprite moves in the same direction before changing directions")]
    public OptionallyRandomFloat directionSwitchInterval;
    [Tooltip("Space inside the viewing area that the sprite will not venture out of")]
    public Vector2 onscreenMargins;
    [Tooltip("Settings for how the sprite is able to move")]
    public SpriteDirection direction;

    [Header("Happy/Angry")]

    [Tooltip("Color of the sprite when it is in the happy state")]
    public Color happyColor;
    [Tooltip("Color of the sprite when it is in the angry state")]
    public Color angryColor;
    [Tooltip("Interval that the sprite waits before becoming angry")]
    public OptionallyRandomFloat angryInterval;
    [Tooltip("The amount of time that the sprite stays angry")]
    public OptionallyRandomFloat angryDuration;

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
            curatedDirections = direction.CurateAvailableDirections(rb.position, view);
            selectedDirection = curatedDirections[Random.Range(0, curatedDirections.Count)];
            rb.velocity = selectedDirection * speed.Get();

            // If the sprite went out of bounds, give the sprite a small nudge back in bounds
            // so that !view.Contains doesn't trigger again immediately
            if (!view.Contains(rb.position))
            {
                rb.position += selectedDirection * 0.1f;
            }

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
        renderer.color = happyColor;

        while(true)
        {
            // Wait for the angry interval, then become angry
            yield return new WaitForSeconds(angryInterval.Get());
            renderer.color = angryColor;

            // Wait for the angry duration, then become happy again
            yield return new WaitForSeconds(angryDuration.Get());
            renderer.color = happyColor;
        }
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

    public bool SpriteIsAngry(SpriteRenderer renderer)
    {
        return renderer.color == angryColor;
    }
}
