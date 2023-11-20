using UnityEngine;

/// <summary>
/// CameraFollow script maintains a specified offset 
/// from the target object (car).
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private string inputId;

    /// <summary>
    /// Start is called before the first frame update.
    /// Sets the car as the target object.
    /// </summary>
    private void Start()
    {
        if (target != null)
        {
            GameObject carObject = GameObject.FindGameObjectWithTag("Player" + inputId.ToString());

            if (carObject != null)
                target = carObject.transform;

            else
                Debug.LogError("CameraFollow: Car object not found.");
        }

        offset = new Vector3(0f, 5f, -10f);
        
        transform.position = target.position + target.TransformDirection(offset);
        transform.LookAt(target);
    }

    /// <summary>
    /// Sets the camera to follow the car's position and rotation after the car has moved and 
    /// rotated.
    /// </summary>
    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 currentOffset = target.TransformDirection(offset);

            transform.SetLocalPositionAndRotation(target.position + currentOffset, target.rotation);
            transform.LookAt(target);
        }
    }
}