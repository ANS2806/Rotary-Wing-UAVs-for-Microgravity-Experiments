using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class DragTesting : MonoBehaviour
{
    public Rigidbody rb;  // Reference to the Rigidbody
    private string filePath;
    public float DragCoefficient; //Drag Coefficient for the body
    public float Density; //Air density
    public float frontalArea; //Frontal Area of the body

    // Start is called before the first frame update
    void Start()
    {
        // File path to store the CSV
        filePath = Application.dataPath + "/DragTest.csv";
        
        // Write CSV header
        File.WriteAllText(filePath, "Time,PositionX,PositionY,PositionZ,VelocityX,VelocityY,VelocityZ\n");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //rb.AddForce(0f, 20f, 0f);
        // Get position, velocity, and time
        Vector3 pos = rb.position;
        Vector3 vel = rb.velocity;
        float speed = vel.magnitude;
        float time = Time.time;

        //Adding velocity
        float dragForceMagnitude = 0.5f*Density*speed*speed*frontalArea*DragCoefficient;
        Vector3 dragForce = -vel.normalized * dragForceMagnitude;
        Debug.Log(" Drag Force: " + dragForce);
        rb.AddForce(dragForce);

        // Create a line of CSV data
        string line = time + "," + pos.x + "," + pos.y + "," + pos.z + "," + vel.x + "," + vel.y + "," + vel.z + "\n";

        // Append data to the CSV file
        File.AppendAllText(filePath, line);
    }
}
