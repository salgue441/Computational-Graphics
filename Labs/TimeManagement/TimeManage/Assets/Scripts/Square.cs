using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    /// <summary>
    /// Enables the square to be clicked on.
    /// </summary>
    public void OnEnable()
    {
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    /// <summary>
    /// Disables the square from being clicked on.
    /// </summary>
    public void OnDisable()
    {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }

    /// <summary>
    /// Checks if the square can move.
    /// </summary>
    private void TimeCheck()
    {
        // if (TimeManager.Minute % 10 == 0)
        // {
        //    StartCoroutine(MoveSquare());
        //}

        StartCoroutine(MoveSquare());
    }

    /// <summary>
    /// Moves the square to a new position
    /// </summary>
    private IEnumerator MoveSquare()
    {
        float minX = -446f;
        float maxX = 1000f;
        float minY = -69f;
        float maxY = 600f;

        Vector3 startPosition = new Vector3(minX, minY, 0);
        transform.position = startPosition;

        float timeToMove = 3f;

        while (true) 
        {
            Vector3 targetPosition = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                0
            );

            float timeElapsed = 0f;

            Vector3 currentPosition = transform.position;

            while (timeElapsed < timeToMove)
            {
                float t = timeElapsed / timeToMove;
                t = Mathf.SmoothStep(0f, 1f, t);

                transform.position = Vector3.Lerp(currentPosition, targetPosition, t);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;

            yield return new WaitForSeconds(1f);
        }
    }

}
