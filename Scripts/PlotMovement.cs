using System.IO;  // For file operations
using UnityEngine;

public class PlotMovement : MonoBehaviour
{
    public Rigidbody rb;  // Reference to the Rigidbody
    private string filePath;

    void Start()
    {
        // File path to store the CSV
        filePath = Application.dataPath + "/RigidbodyData.csv";
        
        // Write CSV header
        File.WriteAllText(filePath, "Time,PositionX,PositionY,PositionZ,VelocityX,VelocityY,VelocityZ\n");
    }

    void FixedUpdate()
    {
        // Get position, velocity, and time
        Vector3 pos = rb.position;
        Vector3 vel = rb.velocity;
        float time = Time.time;

        // Create a line of CSV data
        string line = time + "," + pos.x + "," + pos.y + "," + pos.z + "," + vel.x + "," + vel.y + "," + vel.z + "\n";

        // Append data to the CSV file
        File.AppendAllText(filePath, line);
    }
}

