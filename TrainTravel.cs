using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTravel : MonoBehaviour {
    //public Vector3 destination;
    public Transform destination;
    public float speed;
    float scale;
    bool shouldLayTracks;

	void Start () {

        speed = 10;
        //destination = new Vector3((transform.position.x + 400) * scale, transform.position.y, transform.position.z); //unscaled to Scaler
        scale = GameObject.Find("Scaler").transform.lossyScale.x;
        GlobalVariables.trainMoving = true;
        shouldLayTracks = true;
	}
	
	void Update () {
        if (GlobalVariables.trainMoving && destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime * speed);
        }
        else
        {

        }
        if (transform.position == destination.position)
        {
            shouldLayTracks = false;
        }
    }
    public void StopMoving()
    {
        GlobalVariables.trainMoving = false;
    }
    public void ResumeMoving()
    {
        GlobalVariables.trainMoving = true;
    }
    public bool ShouldLayTracks()
    {
        return shouldLayTracks;
    }
}
