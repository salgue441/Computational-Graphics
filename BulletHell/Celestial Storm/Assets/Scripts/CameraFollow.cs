using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float height = 5f;

    private Vector3 resPosition;

    void LateUpdate()
    {
        resPosition = target.position - target.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, resPosition, smoothSpeed);
        transform.LookAt(target);
    }
}
