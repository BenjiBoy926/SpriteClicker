using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public Vector3 startSize = Vector3.one;
    public Vector3 endSize = Vector3.one * 3f;
    public float explodeSpeed = 3f;

    private void Start()
    {
        StartCoroutine(ExplodeRoutine());
    }

    private IEnumerator ExplodeRoutine()
    {
        float currentTime = 0f;
        transform.localScale = startSize;

        while(transform.localScale.sqrMagnitude < endSize.sqrMagnitude)
        {
            transform.localScale = Vector3.Lerp(startSize, endSize, currentTime);
            currentTime += Time.deltaTime * explodeSpeed;
            yield return null;
        }

        Destroy(gameObject);
    }
}
