using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutomateMovement3 : MonoBehaviour
{
    Rigidbody ourDrone;
    public float upForce; // Initialize upForce to a reasonable value
    private Vector3 targetPosition;
    private List<float> yValues = new List<float>();
    private int currentTargetIndex = 0;
    private bool isMoving = false;
    private float time = 2.0f;
    private bool isHover = false;
    
    void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();
        LoadCSVData("D:\\Microgravity UAV\\Assets\\yLocations.csv");
        if (yValues.Count > 0)
        {
            targetPosition = new Vector3(transform.position.x, yValues[0], transform.position.z);
            isMoving = true;
            //Debug.Log("I have some numbers!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (isMoving)
        {
            Debug.Log("yes!");
            // Apply control to move towards the target position
            Controller(targetPosition.y);

            // Check if we have reached the target position
            if (Mathf.Abs(ourDrone.position.y - targetPosition.y) < 0.05f) // Adjust threshold as needed
            {
                Debug.Log("position achieved");
                currentTargetIndex++;
                Debug.Log(currentTargetIndex);
                if (currentTargetIndex < yValues.Count)
                {
                    targetPosition.y = yValues[currentTargetIndex];
                    Debug.Log("next target set" + targetPosition.y);
                }
                else
                {
                    
                    isMoving = false; // Stop moving if all targets are used
                    Debug.Log("All positions used.");
                    //Hovering();
                }
            }
        }
        else
        {
            Controller(targetPosition.y);
        }
    }
    void LoadCSVData(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = reader.ReadLine(); // Skip header row if needed

                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    if (float.TryParse(values[0], out float yValue))
                    {
                        yValues.Add(yValue);
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Error reading CSV file: " + e.Message);
        }
    }

    void Controller(float targetY)
    {
        Debug.Log("target position is: " + targetY);
        // Proportional and derivative gains
        float kp = 1.5f;  
        float kd = 0.8f;
        float ki = 0.5f;

        float currentY = ourDrone.position.y;

        // Calculate error 
        float errorY = targetY - currentY;

        // Calculate the rate of change of the error 
        float errorRate = ourDrone.velocity.y;

        //Calculate error integral
        float errorIntegral = errorY * Time.fixedDeltaTime;
        errorIntegral += errorY * Time.fixedDeltaTime;

        // Apply Proportional-Derivative control
        float control = (kp * errorY) - (kd * errorRate) + (ki * errorIntegral);

        //Calculate gravitational force
        float gravityForce = ourDrone.mass * 9.81f;

        //Calculate the force needed
        float forceNeeded = (ourDrone.mass * 2 * (targetY-currentY)/ (time * time)) + gravityForce;

        // Proportional-Derivative control for smooth motion
        Vector3 force = new Vector3(0f, forceNeeded + control, 0f);

        // Add force to the Rigidbody
        ourDrone.AddRelativeForce(force);
    }
}
