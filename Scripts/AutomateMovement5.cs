using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomateMovement5 : MonoBehaviour
{
    public Rigidbody ourDrone;
    public Vector3 force; // Public variable to store the final force applied

    private float timeElapsed;
    private float acceleration;

    // PID Controller variables
    private float errorIntegralY = 0f;
    private float previousErrorY = 0f;
    private const float integralMax = 10f; // Clamping limit for integral term

    public float kp = 0.3f; // Proportional gain
    public float ki = 0.6f; // Integral gain
    public float kd = 0.1f; // Derivative gain

    void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();
        timeElapsed = 0f;
        acceleration = 0f;
    }

    void FixedUpdate()
    {
        //Vector3 currentPosition = ourDrone.position;
        timeElapsed += Time.fixedDeltaTime;

        // Determine the acceleration phase
        if (timeElapsed <= 2.5f)
        {
            acceleration = 4.9f; // Constant upward acceleration for the accelerated climb phase
        }
        else if (timeElapsed <= 5.0f)
        {
            acceleration = -9.8f; // Constant downward acceleration for the microgravity phase
        }
        else
        {
            acceleration = 0f; // Constant acceleration during the recovery phase
        }

        // Compute the desired force
        float gravityForce = ourDrone.mass * 9.81f;
        float desiredForceY = ourDrone.mass * acceleration + gravityForce;

        // PID controller logic for Y-axis
        float currentY = ourDrone.position.y;
        float targetY = currentY + 1f; // Simulated target (incrementing the position slightly for control)
        float errorY = targetY - currentY;
        errorIntegralY = Mathf.Clamp(errorIntegralY + errorY * Time.fixedDeltaTime, -integralMax, integralMax);
        float errorDerivativeY = (errorY - previousErrorY) / Time.fixedDeltaTime;
        float pidOutputY = (kp * errorY) + (ki * errorIntegralY) + (kd * errorDerivativeY);
        previousErrorY = errorY;

        // Final force calculation with PID correction
        force = new Vector3(0f, desiredForceY + pidOutputY, 0f);

        // Apply the force to the drone
        ourDrone.AddForce(force);
        //currentPosition.z += 0.01f;
        
        //ourDrone.MovePosition(currentPosition);
    }
}

