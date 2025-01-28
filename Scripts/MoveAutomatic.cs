using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MoveAutomatic : MonoBehaviour
{
    Rigidbody ourDrone;
    public float upForce = 9.81f;
    private float timer = 0.0f;
    private int counter = 0;
    private bool waiting = true;

    public List<float> yValues = new List<float>();

    void Start()
    {
        ourDrone = GetComponent<Rigidbody>();
        StreamReader reader = new StreamReader("D:\\Microgravity UAV\\Assets\\yLocations.csv");
        string line = reader.ReadLine(); // Skip header row if needed
        

        while ((line = reader.ReadLine()) != null)
        {
            string[] values = line.Split(',');
            float yValue = float.Parse(values[0]); 
            yValues.Add(yValue);
        }
        reader.Close();
    }
    void FixedUpdate()
    {
        ourDrone.AddRelativeForce(Vector3.up * upForce);
        if(waiting)
        {
            timer += Time.fixedDeltaTime;
            Debug.Log("Waiting...");
        
            if(timer>= 3.0f)
            {
                timer = 0.0f;
                waiting = false;
                if (counter < yValues.Count)
                {
                    float y = yValues[counter];
                    Vector3 newPos = new Vector3(transform.position.x, y, transform.position.z);
                    ourDrone.MovePosition(newPos);
                    Debug.Log("New Position: " + newPos);
                    counter++;
                }
                else
                {
                    Debug.Log("All positions used");
                    this.enabled = false;
                }
            }
        }
        else
        {
            waiting = true;
            Debug.Log("done");
        }
    }
}