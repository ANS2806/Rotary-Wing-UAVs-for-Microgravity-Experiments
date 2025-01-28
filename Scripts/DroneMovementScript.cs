using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneMovementScript : MonoBehaviour
{
    Rigidbody ourDrone;
    public float upForce;
    private float thrust = 5.0f;
    public InputHandler inputHandler;
    Vector3 m_EulerAngleVelocity;
    private bool keyPressed;
    private Vector3 addedForce;
    public Vector3 F_res;
    public Vector3 upwardsForce;
    private float time = 2.0f;
    public Vector3 force;
    

    void Awake(){
        ourDrone = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        
        MovementUpDown();
        MovementDirections();
        upwardsForce = Vector3.up * upForce;
        ourDrone.AddRelativeForce(Vector3.up * upForce);
        
        if(inputHandler.yPos!=0)
        {
            Controller(inputHandler.yPos);
        }
        ResultantForceDirection();
    }

    void MovementUpDown()
    {
        if(Input.GetKey(KeyCode.W)){
            upForce = 20.0f;

        }
        else if(Input.GetKey(KeyCode.S)){
            upForce = -10.0f;
        }
        else 
        {
            upForce = 9.81f;
        }
    }
    void MovementDirections()
    {
        if(Input.GetKey(KeyCode.LeftArrow)){
            ourDrone.AddRelativeForce(Vector3.back * thrust);
            addedForce = Vector3.back * thrust;
            m_EulerAngleVelocity = new Vector3(-100, 0, 0);
            rotateInDirection(m_EulerAngleVelocity);
            keyPressed = true;
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            ourDrone.AddRelativeForce(Vector3.forward * thrust);
            addedForce = Vector3.forward * thrust;
            m_EulerAngleVelocity = new Vector3(100, 0, 0);
            rotateInDirection(m_EulerAngleVelocity);
            keyPressed = true;
        }
        if(Input.GetKey(KeyCode.UpArrow)){
            ourDrone.AddRelativeForce(Vector3.left * thrust);
            addedForce = Vector3.left * thrust;
            m_EulerAngleVelocity = new Vector3(0, 0, 100);
            rotateInDirection(m_EulerAngleVelocity);
            keyPressed = true;
        }
        if(Input.GetKey(KeyCode.DownArrow)){
            ourDrone.AddRelativeForce(Vector3.right * thrust);
            addedForce = Vector3.right * thrust;
            m_EulerAngleVelocity = new Vector3(0, 0, -100);
            rotateInDirection(m_EulerAngleVelocity);
            keyPressed = true;
        }
        if (!keyPressed){
            ourDrone.rotation = Quaternion.identity;
        }
    }
    void rotateInDirection(Vector3 m_EulerAngleVelocity)
    {
         Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
         ourDrone.MoveRotation(ourDrone.rotation * deltaRotation);
    }
    void Controller(float targetY)
    {
        // Proportional and derivative gains
        float kp = 1.0f;  
        float kd = 0.0f;  
        float ki = 0.0f;
        
        float currentY = ourDrone.position.y;
      
        // Calculate error 
        float errorY = targetY - currentY;
  
        // Calculate the rate of change of the error 
        float errorRateY = ourDrone.velocity.y;

        //Calculate error integral
        float errorIntegralY = errorY * Time.fixedDeltaTime;

        errorIntegralY += errorY * Time.fixedDeltaTime;

        // Apply Proportional-Derivative control
        float controly = (kp * errorY) - (kd * errorRateY) + (ki * errorIntegralY);

        //Calculate gravitational force
        float gravityForce = ourDrone.mass * 9.81f;
        //Debug.Log(gravityForce);

        //Calculate the force needed
        float forceNeededY = (ourDrone.mass * 2 * (targetY-currentY)/ (time * time)) + gravityForce;

        // Proportional-Derivative control for smooth motion
        force = new Vector3(0f, forceNeededY + controly, 0f);

        // Add force to the Rigidbody
        ourDrone.AddRelativeForce(force);
    }
    void ResultantForceDirection()
    {
        F_res = (upwardsForce) + (addedForce);
        //Debug.Log(F_res);
    }
}
