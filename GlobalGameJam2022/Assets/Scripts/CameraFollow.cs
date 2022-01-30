using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float xOffset;

    public Vector3 minValues, maxValues;

    public float smoothSpeed = 0.125f;

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if(target.localScale.x > 0f)
        {
            offset.x = xOffset;
        }
        else
        {
            offset.x = -xOffset;
        }
        
        Vector3 targetPosition = target.position + offset;

        Vector3 boundPosition = new Vector3(Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x), 
                                            Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y), 
                                            Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z));

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }

    public void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
