using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;

    /// <summary>
    /// Sets the initial values of the car.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Updates the car's movement
    /// </summary>
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Destroy(gameObject, 50.0f);
    }
}
