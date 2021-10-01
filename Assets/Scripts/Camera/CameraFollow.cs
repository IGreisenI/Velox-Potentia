using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothspeed;
    public Vector3 offset;

    private void Start()
    {
        smoothspeed = 0.01f;
        offset = new Vector3(0, 3, -5);
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothspeed);

        transform.position = smoothedPosition;

        transform.LookAt(transform);

    }
}
