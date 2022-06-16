using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float angle = 180;

    float seconds;

    Transform _transform;
    public float rotationSpeed;

    public void StartRotating(float time)
    {   
        _transform = gameObject.transform;
        Vector3 rotationVector = new Vector3(0, angle, 0);
        Quaternion targetQuarternion = Quaternion.Euler(rotationVector);
        RotateOverTime(gameObject.transform, targetQuarternion, time);
    }

    public void StartRotatingBackwards(float time)
    {
        Vector3 rotationVector = new Vector3(0, 2 * angle, 0);
        Quaternion targetQuarternion = Quaternion.Euler(rotationVector);
        RotateOverTime(_transform, targetQuarternion, time);
    }
    private void RotateOverTime(Transform transformToRotate, Quaternion targetRotation, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(RotateOverTimeIE(transformToRotate, targetRotation, duration));
    }

    private IEnumerator RotateOverTimeIE(Transform transformToRotate, Quaternion targetRotation, float duration)
    {
        var startRotation = transformToRotate.rotation;

        var timePassed = 0f;
        while (timePassed < duration)
        {
            var factor = timePassed / duration;
            // optional add ease-in and -out
            //factor = Mathf.SmoothStep(0, 1, factor);

            transformToRotate.rotation = Quaternion.Lerp(startRotation, targetRotation, factor);
            // or
            //transformToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, factor);

            // increae by the time passed since last frame
            timePassed += Time.deltaTime;

            // important! This tells Unity to interrupt here, render this frame
            // and continue from here in the next frame
            yield return null;
        }

        // to be sure to end with exact values set the target rotation fix when done
        transformToRotate.rotation = targetRotation;
    }
}
