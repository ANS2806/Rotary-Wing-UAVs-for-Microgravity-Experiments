using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomateMovement5 : MonoBehaviour
{
    public Rigidbody ourDrone;
    public Vector3 force; // Public variable to store the final force applied
    public float DragCoefficient; //Drag Coefficient for the body
    public float Density; //Air density
    public float frontalArea; //Frontal Area of the body

    private float timeElapsed;
    private float prevVelocityY;
    private float acceleration;

    // PID Controller variables
    private float errorIntegralY = 0f;
    private float previousErrorY = 0f;

    private const float integralMax = 10f; // Clamping limit for integral term

    public float kp = 0.3f; // Proportional gain
    public float ki = 0.1f; // Integral gain
    public float kd = 0.0001f; // Derivative gain

    void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();
        timeElapsed = 0f;
        acceleration = 0f;
        prevVelocityY = 0f;
    }

    void FixedUpdate()
    {
        timeElapsed += Time.fixedDeltaTime;
        //ourDrone.position += new Vector3(0f, 0f, 0.01f);
        float currentVelocityY = ourDrone.velocity.y;
        float speed = ourDrone.velocity.magnitude;

        float dragForceMagnitude = 0.5f*Density*speed*speed*frontalArea*DragCoefficient;
        Vector3 dragForce = -ourDrone.velocity.normalized * dragForceMagnitude;
        //Debug.Log(" Drag Force: " + dragForce);
        ourDrone.AddForce(dragForce);

        // Calculate current acceleration as change in velocity over time
        float currentAccelerationY = (currentVelocityY - prevVelocityY) / Time.fixedDeltaTime;
        prevVelocityY = currentVelocityY;

        // Determine the desired acceleration phase
        if (timeElapsed <= 3.0f)
        {
            acceleration = 4.9f; // Accelerated climb
        }
        else if (timeElapsed <= 6.0f)
        {
            acceleration = -9.8f; // Microgravity phase
        }
        else
        {
            acceleration = 1f; // Recovery phase  
        }

        // Compute error based on desired vs. current acceleration
        float errorY = acceleration - currentAccelerationY;
        //Debug.Log($"desired acceleration: {acceleration}, current acceleration: {currentAccelerationY}");
        //Debug.Log(errorY);
        errorIntegralY = Mathf.Clamp(errorIntegralY + errorY * Time.fixedDeltaTime, -integralMax, integralMax);
        float errorDerivativeY = (errorY - previousErrorY) / Time.fixedDeltaTime;
        float pidOutputY = (kp * errorY) + (ki * errorIntegralY);
        previousErrorY = errorY;
        //Debug.Log($"error integral: {errorIntegralY}, error derivative:{errorDerivativeY}, PID output acceleration: {pidOutputY}");

        // Compute force needed to adjust acceleration (force = mass * acceleration)
        float desiredForceY = ourDrone.mass * pidOutputY;
        //Debug.Log($"Force Applied: {desiredForceY}");

        force = new Vector3(0f, desiredForceY, 0f);
        ourDrone.AddForce(force);
    }
}
