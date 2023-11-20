using UnityEngine;

/// <summary>
/// The `PlayerController` class is responsible for controlling the player's
/// input and behaviour.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float deceleration = 5.0f;
    [SerializeField] private float maxSpeed = 20.0f;
    [SerializeField] private float turnSpeed = 150.0f;
    [SerializeField] private float brakeStrength = 20.0f;
    [SerializeField] private float collisionImpactForce = 10.0f;
    [SerializeField] private float steerAngle = 30.0f;
    [SerializeField] private Transform[] wheelMeshes = new Transform[4];
    [SerializeField] private float cameraTransitionDuration = 0.20f;
    [SerializeField] private KeyCode cameraSwitch = KeyCode.C;
    [SerializeField] private string inputId;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera hoodCamera;

    public float CurrentSpeed { get; private set; }

    private Rigidbody rb;
    private float inputVertical;
    private float inputHorizontal;
    private bool isTransitioning = false;
    private float transitionTimer = 0.0f;

    /// <summary>
    /// Sets the initial values of the car. 
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.centerOfMass = new Vector3(0f, -0.9f, 0.2f);
        rb.mass = 1500f;
        rb.drag = 0.1f;

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        mainCamera.enabled = true;
        hoodCamera.enabled = false;
        cameraTransitionDuration =
            Mathf.Clamp(cameraTransitionDuration, 0.0f, float.MaxValue);

        InitializeMeshes();
    }

    /// <summary>
    /// Updates the car's movement and steering.
    /// </summary>
    private void Update()
    {
        GetInput();
        HandleSteering();
        ChangeCameraView(Time.deltaTime);
    }

    /// <summary>
    /// Gets the main camera and hood camera.
    /// </summary>
    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("MainCamera" + inputId.ToString()).GetComponent<Camera>();
        hoodCamera = GameObject.FindWithTag("HoodCamera" + inputId.ToString()).GetComponent<Camera>();

        if (mainCamera == null)
            Debug.LogError("Main camera not found for input ID: " + inputId);

        if (hoodCamera == null)
            Debug.LogError("Hood camera not found for input ID: " + inputId);
    }


    /// <summary>
    /// Fixed Update is called at a fixed interval and is independent of
    /// the frame rate. Calls the `HandleMotor()` and `MoveWheels()` methods.
    /// </summary>
    private void FixedUpdate()
    {
        HandleMotor();
        MoveWheels();
    }

    /// <summary>
    /// Initializes the wheel meshes
    /// </summary>
    private void InitializeMeshes()
    {
        string[] wheelMeshesName = { "Wheel_fl", "Wheel_fr", "Wheel_rl", "Wheel_rr" };
        for (int i = 0; i < wheelMeshes.Length; i++)
        {
            wheelMeshes[i] = transform.Find(wheelMeshesName[i]);

            if (wheelMeshes[i] == null)
            {
                Debug.LogError("Wheel mesh not found for: " + wheelMeshesName[i]);
            }
        }
    }

    /// <summary>
    /// Changes the player's camera view.
    /// </summary>
    /// <param name="deltaTime">The time between the current and previous frame.</param>
    private void ChangeCameraView(float deltaTime)
    {
        if (Input.GetKeyDown(cameraSwitch) && !isTransitioning)
        {
            isTransitioning = true;
            transitionTimer = 0.0f;

            mainCamera.enabled = !mainCamera.enabled;
            hoodCamera.enabled = !hoodCamera.enabled;

            // Ensure only one AudioListener is active
            AudioListener mainListener = mainCamera.GetComponent<AudioListener>();
            AudioListener hoodListener = hoodCamera.GetComponent<AudioListener>();

            if (mainListener != null)
                mainListener.enabled = mainCamera.enabled;

            if (hoodListener != null)
                hoodListener.enabled = hoodCamera.enabled;
        }

        if (isTransitioning)
        {
            transitionTimer += deltaTime;
            if (transitionTimer >= cameraTransitionDuration)
            {
                transitionTimer = cameraTransitionDuration;
                isTransitioning = false;
            }

            float t = transitionTimer / cameraTransitionDuration;
            mainCamera.transform.SetPositionAndRotation(Vector3.Lerp(
                mainCamera.transform.position, hoodCamera.transform.position, t),
                Quaternion.Slerp(mainCamera.transform.rotation, hoodCamera.transform.rotation, t));
        }
    }

    /// <summary>
    /// Gathers the player's input for vehicle movement.
    /// </summary>
    private void GetInput()
    {
        inputHorizontal = Input.GetAxis("Horizontal" + inputId.ToString());
        inputVertical = Input.GetAxis("Vertical" + inputId.ToString());
    }

    /// <summary>
    /// Handles the vehicle's acceleration, deceleration, and braking based on player input.
    /// </summary>
    private void HandleMotor()
    {
        bool isHandbrakeOn = Input.GetKey(KeyCode.Space);
        float currentDrag = isHandbrakeOn ? brakeStrength : 0.1f;

        rb.drag = currentDrag;

        if (inputVertical > 0 && !isHandbrakeOn)
        {
            float targetSpeed = inputVertical > 0 ? maxSpeed : -maxSpeed / 2;
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, targetSpeed, Time.deltaTime * acceleration);
        }

        else if (inputVertical < 0 && !isHandbrakeOn)
        {
            if (CurrentSpeed > 0)
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, -maxSpeed, Time.deltaTime * acceleration);

            else
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, -maxSpeed / 2, Time.deltaTime * acceleration);
        }

        // Natural deceleration until speed is 0
        else if (inputVertical == 0 && !isHandbrakeOn)
        {
            CurrentSpeed = Approach(CurrentSpeed, 0, Time.deltaTime * deceleration);
        }

        else if (isHandbrakeOn)
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, 0, Time.deltaTime * brakeStrength);

        rb.velocity = transform.forward * CurrentSpeed;
    }

    /// <summary>
    /// Performs a linear interpolation between two values.
    /// </summary>
    /// <param name="current">: Current value</param>
    /// <param name="target">: Target value</param>
    /// <param name="delta">: Delta time</param>
    /// <returns>A float value </returns>
    private float Approach(float current, float target, float delta)
    {
        if (current < target)
            return Mathf.Min(current + delta, target);

        else
            return Mathf.Max(current - delta, target);
    }

    /// <summary>
    /// Handles the vehicle's steering based on player input.
    /// </summary>
    private void HandleSteering()
    {
        Quaternion turnRotation = Quaternion.Euler(0f, 
            inputHorizontal * turnSpeed * Time.deltaTime, 0f);

        rb.MoveRotation(rb.rotation * turnRotation);
    }

    /// <summary>
    /// Spins the meshes of the wheels to simulate movement.
    /// </summary>
    private void MoveWheels()
    {
        float rotationSpeed = CalculateWheelRotationSpeed();

        for (int i = 0; i < wheelMeshes.Length; i++)
        {
            wheelMeshes[i].Rotate(Vector3.right, rotationSpeed, Space.Self);

            if (i < 2) 
            {
                Vector3 localEulerAngles = wheelMeshes[i].localEulerAngles;
                localEulerAngles.y = inputHorizontal * steerAngle;
                wheelMeshes[i].localEulerAngles = localEulerAngles;
            }
        }
    }

    /// <summary>
    /// Calculates the rotation speed of the wheels based on the 
    /// vehicle's current speed.
    /// </summary>
    /// <returns>Rotation speed of the wheels</returns>
    private float CalculateWheelRotationSpeed()
    {
        float wheelCircumference = 2 * Mathf.PI * wheelMeshes[0].localScale.y;
        float rotationSpeed = (CurrentSpeed / wheelCircumference) * Time.deltaTime * 360;

        return rotationSpeed;
    }

    /// <summary>
    /// Handles Collisions with the car
    /// </summary>
    /// <param name="collision">The collision data associated with this collision event.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > collisionImpactForce)
        {
            CurrentSpeed -= collision.relativeVelocity.magnitude / 10;
            
            if (collision.gameObject.TryGetComponent<Rigidbody>(out var hitRb))
            {
                hitRb.AddForce(0.1f * rb.mass * -collision.relativeVelocity,
                    ForceMode.Impulse); 

                ReduceSpeedOnCollsion(collision);
            }
        }
    }

    /// <summary>
    /// Reduces the vehicle's speed in response to a collision.
    /// </summary>
    /// <param name="collision">The collision data associated with this collision event.</param
    private void ReduceSpeedOnCollsion(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > collisionImpactForce)
        {
            CurrentSpeed -= collision.relativeVelocity.magnitude / 10;
        }
    }
}
