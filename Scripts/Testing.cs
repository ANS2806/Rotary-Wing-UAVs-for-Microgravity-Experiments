using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Rigidbody ourDrone;
    public float DownwardForce;
    // Start is called before the first frame update
    void Start()
    {
        ourDrone = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ourDrone.AddForce(Vector3.down * DownwardForce, ForceMode.Acceleration);
    }
}
