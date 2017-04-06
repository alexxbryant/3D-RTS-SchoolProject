using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrainNavigator : MonoBehaviour {
    NavMeshAgent navAgent;
    public Transform Destination;
    public Transform mainMenu;

    Transform lastCol;
    float trainStopTimer;
    bool timerRunning;
	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.destination = Destination.position;
        trainStopTimer = 0;
        timerRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
        //   transform.rotation = Quaternion.Euler(-90, 0, 0);
        transform.position = new Vector3(transform.position.x, 6, transform.position.z);
        if(timerRunning)
            trainStopTimer += Time.deltaTime;
        if(trainStopTimer >= 5)
        {
            ShowNextTrainTrack();
        }
        if(trainStopTimer >= 10)
        {
            trainStopTimer = 0;
            timerRunning = false;
            navAgent.Resume();
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Track")
        {
            Debug.Log("Train stopped");
            navAgent.Stop();
            timerRunning = true;
            //Turn on meshrenderer of next track
            lastCol = col.transform.parent;
           /* int colPos = col.transform.GetSiblingIndex();
            if (colPos < col.transform.parent.parent.childCount - 1)
                col.transform.parent.parent.GetChild(colPos + 1).GetComponent<MeshRenderer>().enabled = true;
            else
                Debug.Log("child count: " + (col.transform.parent.parent.childCount - 1) + " col pos is: " + colPos +" parent is: " + col.transform.parent.parent.name);
            */
        }

    }
    void ShowNextTrainTrack()
    {
        Debug.Log("shownexttrack called, last col is: " + lastCol.name);
        int colPos = lastCol.GetSiblingIndex();
        if (colPos < lastCol.parent.childCount - 1)
            lastCol.parent.GetChild(colPos + 1).GetComponent<MeshRenderer>().enabled = true;
        else
            Debug.Log("child count: " + (lastCol.parent.childCount - 1) + " col pos is: " + colPos + " parent is: " + lastCol.parent.name);

    }
    void TrainWins()
    {
        mainMenu.gameObject.SetActive(false);

    }
}
