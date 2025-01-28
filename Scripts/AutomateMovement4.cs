using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutomateMovement4 : MonoBehaviour
{
    //Defining all the variables 
    Rigidbody ourDrone;
    public float upForce; // Initialize upForce to a reasonable value
    private Vector3 targetPosition;
    private float moveTime;
    private List<float> yValues = new List<float>();
    private List<float> zValues = new List<float>();
     private List<float> times = new List<float>();
    private int currentTargetIndex = 0;
    private bool isMoving = false;
    public Vector3 force;
    
    void Awake()
    {
        //Getting the rigidbody component from the UAV game object
        ourDrone = GetComponent<Rigidbody>();
        //Debug.Log("started");
        //Loading the CSV file data using a function, defines the file location path
        LoadCSVData("D:\\Microgravity UAV\\Assets\\Locations.csv");
        //If the number of values under this header is greater than 0 get the first 
        //target position and set boolean to true
        if (yValues.Count > 0 && zValues.Count > 0 && times.Count > 0)
        {
            targetPosition = new Vector3(transform.position.x, yValues[0], zValues[0]);
            moveTime = times[0];
            isMoving = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if boolean is true
        if (isMoving)
        {
            //Debug.Log("yes!");
            // Use PID controller to move towards the target position by adding force
            Controller(targetPosition.y, targetPosition.z,moveTime);

            
            // Check if we have reached the target position
            if ((Mathf.Abs(ourDrone.position.y - targetPosition.y) < 5f)) // Adjust threshold as needed
	          {   
                //Debug.Log("position achieved");
			    //if we reached the position increase the index for the position
                currentTargetIndex++;
                //Debug.Log(currentTargetIndex);
                // check if the new index value is less than number of y values & z values given
                if ((currentTargetIndex < yValues.Count) & (currentTargetIndex < zValues.Count))
                {
		            //set the next y value as target position
                    targetPosition.y = yValues[currentTargetIndex];
                    targetPosition.z = zValues[currentTargetIndex];
                    moveTime = times[currentTargetIndex];
                    Debug.Log("next target set" + targetPosition.y + ", " + targetPosition.z + "Time: " + moveTime);
                }
                else
                {
                    //if no more y values are available set boolean to false and display msg
                    isMoving = false; // Stop moving if all targets are used
                    //Debug.Log("All positions used.");
                }
            }
            else{
                // If Y condition is met but Z condition is not met
                if (Mathf.Abs(ourDrone.position.y - targetPosition.y) < 0.1f && Mathf.Abs(ourDrone.position.z - targetPosition.z) >= 0.3f)
                {
                    Debug.Log("Y condition met, but Z condition not met. Current Z: " + ourDrone.position.z + ", Target Z: " + targetPosition.z);
                }
    
                // If Z condition is met but Y condition is not met
                if (Mathf.Abs(ourDrone.position.y - targetPosition.y) >= 0.1f && Mathf.Abs(ourDrone.position.z - targetPosition.z) < 0.3f)
                {
                    Debug.Log("Z condition met, but Y condition not met. Current Y: " + ourDrone.position.y + ", Target Y: " + targetPosition.y);
                }
            }
        }
        else
        {
		        //if boolean is false (usually after all positions are used unless file error/empty)
		        //use controller to hover about the last target position
            Controller(targetPosition.y, targetPosition.z,moveTime);
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
                    if (values.Length >= 2 && 
                        float.TryParse(values[0], out float yValue) &&
                        float.TryParse(values[1], out float zValue) && 
                        float.TryParse(values[2], out float time))
                    {
                        yValues.Add(yValue);
                        zValues.Add(zValue);
                        times.Add(time);
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Error reading CSV file: " + e.Message);
        }
    }

    void Controller(float targetY, float targetZ, float time)
    {
        //Debug.Log("target position is: " + targetY  + "," + targetZ);
        // Proportional and derivative gains
        float kp_y = 0.3f;  
        float kd_y = 0.1f;
        float ki_y = 0.6f;
        float kp_z = 3.0f;
        float kd_z = 3.0f;
        float ki_z = 1.0f;

        float currentY = ourDrone.position.y;
        float currentZ = ourDrone.position.z;

        // Calculate error 
        float errorY = targetY - currentY;
        float errorZ = targetZ - currentZ;

        
        float errorRateY = ourDrone.velocity.y;
        float errorRateZ = ourDrone.velocity.z;

        //Calculate error integral
        float errorIntegralY = errorY * Time.fixedDeltaTime;
        float errorIntegralZ = errorZ * Time.fixedDeltaTime;

        errorIntegralY += errorY * Time.fixedDeltaTime;
        errorIntegralZ += errorZ * Time.fixedDeltaTime;

        // Apply Proportional-Derivative control
        float controly = (kp_y * errorY) - (kd_y * errorRateY) - (ki_y * errorIntegralY);
        float controlz = (kp_z * errorZ) - (kd_z * errorRateZ) - (ki_z * errorIntegralZ);

        //Calculate gravitational force
        float gravityForce = ourDrone.mass * 9.81f;
        //Debug.Log(gravityForce);

        //Calculate the force needed
        float forceNeededY = (ourDrone.mass * 2 * (targetY-currentY)/ (time * time)) + gravityForce;
        float forceNeededZ = (ourDrone.mass * 2 * (targetZ-currentZ)/ (time * time));

        // Proportional-Derivative control for smooth motion
        force = new Vector3(0f, forceNeededY + controly, forceNeededZ + controlz);
        //Debug.Log(force);

        // Add force to the Rigidbody
        ourDrone.AddRelativeForce(force);
    }
}
