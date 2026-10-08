using UnityEngine;

public class Player : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 15f;
    public float boostMaxSpeed = 25f;
    public float deceleration = 8f;
    public float brakeAmount = 0.5f;
    public float handbrakeAmount = 0.3f;
    public float jumpForce = 8f;
    public float driftAmount = 0.8f;
    public float driftRecovery = 2f;
    public TurnPoint turnPoint;
    private Rigidbody rb;
    private float currentSpeed;
    private Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentSpeed = 0f;
        movementDirection = transform.forward;
    }

    void Update()
    {
        Accelerate();
        TurnCar();
        Jump();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Accelerate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            float maxAllowedSpeed = maxSpeed;

            if (Input.GetKey(KeyCode.B))
            {
                maxAllowedSpeed = boostMaxSpeed;
            }

            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxAllowedSpeed);
        }
        else
        {
            currentSpeed -= deceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }
    }

    void MovePlayer()
    {
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        float speed = currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= brakeAmount;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            speed *= handbrakeAmount;

            movementDirection = Vector3.Lerp(movementDirection, transform.forward, driftAmount * Time.fixedDeltaTime);

            verticalInput *= -1f;
        }
        else
        {
            movementDirection = Vector3.Lerp(movementDirection, transform.forward, driftRecovery * Time.fixedDeltaTime);
        }

        movementDirection.y = 0f;
        movementDirection.Normalize();

        Vector3 targetVelocity = Vector3.zero;

        if (verticalInput != 0f)
        {
            targetVelocity = movementDirection * speed * verticalInput;
        }

        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;
    }

    void TurnCar()
    {
        if (turnPoint == null)
        {
            return;
        }

        float steeringAngle = turnPoint.GetSteeringAngle();

        float turnAmount = steeringAngle * currentSpeed * 0.1f;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount * Time.deltaTime, 0f));
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //checks for the ground
            if (Physics.Raycast(transform.position, Vector3.down, 1.5f))
            {
                Vector3 velocity = rb.linearVelocity;
                velocity.y = jumpForce;
                rb.linearVelocity = velocity;
            }
        }
    }
}