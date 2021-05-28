using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteComponent : MonoBehaviour
{
    [Tooltip("Reference to the rigidbody used to move the sprite")]
    public Rigidbody2D rb;
    [Tooltip("Refernece to the sprite rendered used to render the sprite")]
    public SpriteRenderer sprite;
    [Tooltip("Reference to the animator used to animate the sprite")]
    public Animator animator;
    [Tooltip("Reference to the scriptable object that defines this sprite's behavior")]
    public SpriteBehavior behavior;
    [Tooltip("Event invoked when the sprite is successfully clicked while angry")]
    public UnityEvent successfulClickEvent;

    // Store the couritines for later in case we ever need to stop them
    private Coroutine movementRoutine;
    private Coroutine angerRoutine;

    private void Start()
    {
        movementRoutine = StartCoroutine(behavior.MovementRoutine(rb, sprite, Camera.main));
        angerRoutine = StartCoroutine(behavior.AngerRoutine(sprite, animator));
    }

    private void OnMouseDown()
    {
        if(behavior.SpriteIsAngry(sprite))
        {
            StopCoroutine(angerRoutine);
            angerRoutine = StartCoroutine(behavior.AngerRoutine(sprite, animator));

            // Invoke the successful click event
            successfulClickEvent.Invoke();
        }
    }
}
