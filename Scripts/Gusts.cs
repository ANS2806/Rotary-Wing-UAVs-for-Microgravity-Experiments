using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gusts : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 gustDirection = Vector3.right;  // Direction of the gust
    public Vector3 gustDirection2 = Vector3.right;  // Direction of the gust
    public float gustForce = 10.0f;        // Force in Newtons
    private float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        timeElapsed = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed > 3.2f && timeElapsed < 3.6f){
            rb.AddForce(gustDirection.normalized * gustForce, ForceMode.Force);
            Debug.Log("Gust applied 1");
        }
        if (timeElapsed > 4.0f && timeElapsed < 4.4f){
            rb.AddForce(gustDirection2.normalized * gustForce, ForceMode.Force);
            Debug.Log("Gust applied 2");
        }
        if (timeElapsed > 4.8f && timeElapsed < 5.2f){
            rb.AddForce(gustDirection.normalized * gustForce, ForceMode.Force);
            Debug.Log("Gust applied 3");
        }
    }
}
