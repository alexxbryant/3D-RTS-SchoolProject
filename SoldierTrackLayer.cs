using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierTrackLayer : MonoBehaviour {
    bool exitingTrain;
    bool runningTowardsTrack;
    bool layingTrack;
    bool returningToTrain;
    bool boardedTrain;
    float timer;
    public Transform trainCart;
    public float soldierRunSpeed;
    Vector3 locationOfNextTrack;
    Vector3 originalRotation;
    Vector3 spotOnTrain;

    NavMeshAgent navMeshAgent;

    // Use this for initialization
    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        exitingTrain = runningTowardsTrack = layingTrack = runningTowardsTrack = boardedTrain = false;
        timer = 0;
        originalRotation = transform.rotation.eulerAngles;
        spotOnTrain = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (exitingTrain)
        {
            timer += Time.deltaTime;
            if(timer < .5f)
            {
           //     transform.Translate(Vector3.forward * Time.deltaTime * soldierRunSpeed);
                
            }
            else
            {
                runningTowardsTrack = true;
                exitingTrain = false;
                timer = 0;
            }
        }
        if (runningTowardsTrack)
        {
            transform.LookAt(locationOfNextTrack);
            //transform.position = Vector3.MoveTowards(transform.position, locationOfNextTrack, Time.deltaTime * soldierRunSpeed);
            navMeshAgent.destination = locationOfNextTrack;
            navMeshAgent.desiredVelocity.Set(soldierRunSpeed, 0, soldierRunSpeed);
            if(Mathf.Abs(transform.position.x - locationOfNextTrack.x) < .1)
            {
                runningTowardsTrack = false;
                layingTrack = true;
            }
        }
        if (returningToTrain)
        {
            transform.LookAt(spotOnTrain);
            //transform.position = Vector3.MoveTowards(transform.position, spotOnTrain, Time.deltaTime * soldierRunSpeed);
            navMeshAgent.destination = spotOnTrain;
            /* if (Mathf.Abs(transform.position.x - spotOnTrain.x) < .01)
             {
                 returningToTrain = false;
                 boardedTrain = true;
                 transform.GetComponent<TrainTravel>().enabled = true;
                 transform.rotation = Quaternion.Euler(originalRotation);
             }*/
        }
	}
    public void StartLayingTrack(Vector3 trackLocation)
    {
        spotOnTrain = transform.position;
        transform.GetComponent<TrainTravel>().enabled = false;
        exitingTrain = true;
        locationOfNextTrack = trackLocation;
    }
    public void RunBackToTrain()
    {
        layingTrack = false;
        returningToTrain = true;
    }
    public bool BackOnTrain()
    {
        return boardedTrain;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Track")
        {
            runningTowardsTrack = false;
            layingTrack = true;
        }
        if(col.collider.tag == "TrainCart")
        {
            returningToTrain = false;
            boardedTrain = true;
            transform.GetComponent<TrainTravel>().enabled = true;
            transform.rotation = Quaternion.Euler(originalRotation);
        }
    }
}
