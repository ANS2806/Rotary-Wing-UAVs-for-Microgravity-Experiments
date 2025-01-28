using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotorForce : MonoBehaviour
{
    public GameObject FLM;
    public GameObject FRM;
    public GameObject BLM;
    public GameObject BRM;
    public Rigidbody rb;
    public float flm;
    public float frm;
    public float blm;
    public float brm;
    public float flmLow;
    public float frmLow;
    public float blmLow;
    public float brmLow;
    public KeyCode turnOffKey = KeyCode.Space;
    public KeyCode turnOnKey = KeyCode.U;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(turnOffKey))
        {
            rb.AddForceAtPosition(transform.up*flmLow, FLM.transform.position);
            rb.AddForceAtPosition(transform.up*frmLow, FRM.transform.position);
            rb.AddForceAtPosition(transform.up*blmLow, BLM.transform.position);
            rb.AddForceAtPosition(transform.up*brmLow, BRM.transform.position);
            Debug.Log("Velocity is: " + rb.velocity);
        }
        if(Input.GetKey(turnOnKey))
        {
            rb.AddForceAtPosition(transform.up*flm, FLM.transform.position);
            rb.AddForceAtPosition(transform.up*frm, FRM.transform.position);
            rb.AddForceAtPosition(transform.up*blm, BLM.transform.position);
            rb.AddForceAtPosition(transform.up*brm, BRM.transform.position);
            Debug.Log("Velocity is: " + rb.velocity);
        }     
    }
}
