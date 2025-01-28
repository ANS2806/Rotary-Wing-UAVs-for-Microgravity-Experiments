using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaObject: MonoBehaviour
{
    public List<GameObject> waypoints;
    public float initialSpeed = 10;
    private int index = 0;

    void Start()
    {
        
    }

    void Update()
    {
        //index number of waypoint
        
        //position of the waypoint
        Vector3 destination = waypoints[index].transform.position;
        //new position of the object 
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, initialSpeed*Time.deltaTime);
        //assign the position transform of object as new position
        transform.position = newPos;

        // Get the direction vector to the next waypoint
        Vector3 directionToWaypoint = destination - transform.position;

        // Create a rotation where the object's z-axis points toward the waypoint
        Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, directionToWaypoint) * transform.rotation;

        // Smoothly rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        //distance between both points
        float distance = Vector3.Distance(transform.position, destination);
        //if the distance to the waypoint is too little skip ahead to the next waypoint
        //Debug.Log(distance);
        if(distance <= 0.001)
        {
            if(index<waypoints.Count - 1){
                index++;
            }
        }
    }
}
