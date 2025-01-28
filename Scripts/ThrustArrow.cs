using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustArrow : MonoBehaviour
{
    public AutomateMovement5 AutomateMovement5;  // Reference to the drone's script
    public Transform arrow;  // The arrow object to visually represent F_res

    private void Update()
    {
        float magnitude = AutomateMovement5.force.magnitude;
        if (magnitude!= 0){
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, AutomateMovement5.force);
            arrow.rotation = targetRotation;  // Apply the rotation to the arrow
        
            float scaleFactor = magnitude*0.1f;
            Vector3 newScale = new Vector3(scaleFactor, arrow.localScale.y, arrow.localScale.z);
            arrow.localScale = newScale;
        }
    }
}
