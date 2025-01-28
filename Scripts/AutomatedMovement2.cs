using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutomatedMovement2 : MonoBehaviour
{
    Rigidbody ourDrone;
    public float upForce; // Initialize upForce to a reasonable value
    private Vector3 targetPosition;
    private List<float> yValues = new List<float>();
    private int currentTargetIndex = 0;
    private bool isMoving = false;
    private float waitTime = 3.0f; // Time to wait at each target position
    private float timer = 0.0f;

    void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();
        LoadCSVData("D:\\Microgravity UAV\\Assets\\yLocations.csv");
        if (yValues.Count > 0)
        {
            targetPosition = new Vector3(transform.position.x, yValues[0], transform.position.z);
            isMoving = true;
            Debug.Log("I have some numbers!");
        }
    }

    void FixedUpdate()
    {
        ourDrone.AddRelativeForce(Vector3.up * upForce);
        if (isMoving)
        {
            Debug.Log("yes!");
            // Apply control to move towards the target position
            Controller(targetPosition.y);

            // Check if we have reached the target position
            if (Mathf.Abs(ourDrone.position.y - targetPosition.y) < 0.1f) // Adjust threshold as needed
            {
                Debug.Log("moving...");
                // Start the wait timer
                timer += Time.fixedDeltaTime;
                Debug.Log(timer);

                if (timer >= waitTime)
                {
                    Debug.Log("not waiting");
                    // Move to the next target
                    currentTargetIndex++;
                    Debug.Log(currentTargetIndex);
                    if (currentTargetIndex < yValues.Count)
                    {
                        
                        targetPosition.y = yValues[currentTargetIndex];
                        timer = 0.0f; // Reset the wait timer
                        Debug.Log("next target set" + targetPosition.y);
                    }
                    else
                    {
                        Debug.Log("All positions used.");
                        isMoving = false; // Stop moving if all targets are used
                    }
                }
            }
            else
            {
                timer = 0.0f; // Reset wait timer if not at target
            }
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

        float currentY = ourDrone.position.y;

        // Calculate error 
        float errorY = targetY - currentY;

        // Calculate the rate of change of the error 
        float errorRate = ourDrone.velocity.y;

        // Apply Proportional-Derivative control
        float control = (kp * errorY) - (kd * errorRate);

        // Proportional-Derivative control for smooth motion
        Vector3 force = new Vector3(0f, control, 0f);

        // Add force to the Rigidbody
        ourDrone.AddRelativeForce(force);
    }
}