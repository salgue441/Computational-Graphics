using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float thrustForce = 100f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float maxVelocity = 50f;

    private float pitchInput;
    private float yawInput;
    private float rollInput;
    private float thrustInput;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.angularDrag = 0f;
        rb.drag = 0f;
    }

    private void Update()
    {
        GetInputs();
    }

    /// <summary>
    /// Gets the inputs from the player.
    /// </summary>
    private void GetInputs()
    {
        pitchInput = Input.GetAxis("Pitch");
        yawInput = Input.GetAxis("Yaw");
        rollInput = Input.GetAxis("Roll");
        thrustInput = Input.GetAxis("Thrust");
    }

    /// <summary>
    /// Moves the shuttle with realistic space physics.
    /// </summary>
    private void FixedUpdate()
    {
        ApplyThrust();
        ApplyRotation();
    }

    private void ApplyThrust()
    {
        if (thrustInput > 0)
        {
            rb.AddRelativeForce(thrustForce * thrustInput * Time.deltaTime * Vector3.forward);
        }

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    private void ApplyRotation()
    { 
        float pitch = pitchInput * rotationSpeed * Time.deltaTime;
        float yaw = yawInput * rotationSpeed * Time.deltaTime;
        float roll = rollInput * rotationSpeed * Time.deltaTime;

        transform.Rotate(pitch, yaw, roll);

        if (pitchInput != 0 || yawInput != 0 || rollInput != 0)
        {
            rb.angularDrag = 0.5f;
        }
        else
        {
            rb.angularDrag = 0f;
        }
    }
}
