using UnityEngine;

public class TurnPoint : MonoBehaviour
{
    public float steeringSpeed = 100f;
    public float maxSteeringAngle = 30f;
    public float steeringReturnSpeed = 150f;
    private float currentSteeringAngle = 0f;

    void Update()
    {
        Steering();
    }

    void Steering()
    {
        float steeringInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            steeringInput = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            steeringInput = 1f;
        }

        if (steeringInput != 0f)
        {
            currentSteeringAngle += steeringInput * steeringSpeed * Time.deltaTime;
        }
        else
        {
            currentSteeringAngle = Mathf.MoveTowards(currentSteeringAngle, 0f, steeringReturnSpeed * Time.deltaTime);
        }

        currentSteeringAngle = Mathf.Clamp(currentSteeringAngle, -maxSteeringAngle, maxSteeringAngle);

        transform.localRotation = Quaternion.Euler(0f, currentSteeringAngle, 0f);
    }

    public float GetSteeringAngle()
    {
        return currentSteeringAngle;
    }
}