using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackLayer : MonoBehaviour {

    Vector3 nextSpawnSpot;
    Vector3 lastSpawnSpot;
    public Vector3 sizeOfTrack;
    public TrainTravel trainMovementScript;
    public GameObject TracksParent;
    public GameObject TrackPiece;
    public Transform soldiers;
    Transform lastTrack;
    GameObject trackSpawn;
    //Rigidbody trainBody;

    static bool shouldLay;
    static bool trackPieceBroke;
	// Use this for initialization
	void Start () {
        TracksParent = GameObject.Find("Tracks");
        if (!TracksParent)
            Debug.Log("didn't find tracks parent");
        SetNextSpawnSpot();
        shouldLay = true;
        trackPieceBroke = false;
        lastSpawnSpot = nextSpawnSpot;
        lastTrack = TracksParent.transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void FixedUpdate()
    {
        if (!trainMovementScript.ShouldLayTracks())
        {
            shouldLay = false;
            Debug.Log("Done laying tracks");
        }else
        {
         //   Debug.Log("Shouldlay is true");
        }
    }
    void LayAnotherTrack()
    {
        Debug.Log("lay another track");
        trackSpawn = (GameObject)Instantiate(TrackPiece, nextSpawnSpot, Quaternion.identity, transform);
        trackSpawn.GetComponent<MeshRenderer>().enabled = false;
        trackSpawn.transform.SetParent(TracksParent.transform);
        trackSpawn.transform.localScale = sizeOfTrack;
    }
    void SetNextSpawnSpot()
    {
        Debug.Log("set next spawn spot");
        lastSpawnSpot = nextSpawnSpot;
        Transform lastChild = TracksParent.transform.GetChild(0);
        foreach(Transform child in TracksParent.transform)
        {
            lastChild = child;
            
        }
        lastTrack = lastChild;
        nextSpawnSpot = lastChild.GetChild(0).position;
        nextSpawnSpot.x += lastChild.GetComponent<MeshRenderer>().bounds.extents.x;
      //  Debug.Log("ex: " + lastChild.GetComponent<MeshRenderer>().bounds.extents.x);
    }
    IEnumerator Delay()
    {
        if (!trackPieceBroke)
        {
            Debug.Log("delay called");
            //Must ensure that trackspawn continues to exist before doing anything
            
            SendOutSoldiers();
            yield return new WaitForSeconds(10);
            LayAnotherTrack();
            yield return new WaitForSeconds(30);
            if (trackSpawn)
                trackSpawn.GetComponent<MeshRenderer>().enabled = true;
            if(trackSpawn)
                foreach(Transform child in soldiers)
                {
                    child.GetComponent<SoldierTrackLayer>().RunBackToTrain();
                }
           if(trackSpawn)
                yield return new WaitForSeconds(10);
           if(trackSpawn)
                trainMovementScript.ResumeMoving();
        }

    }
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "stopTrigger")
        {
            if (shouldLay)
            {
                trainMovementScript.StopMoving();
                SetNextSpawnSpot();
                StartCoroutine(Delay());

            }
        }

    }
    void OnTriggerStay(Collider col)
    {
        Destroy(col.GetComponent<SphereCollider>());
    }
    void OnTriggerExit(Collider col)
    {
      
    }
    void SendOutSoldiers()
    {
        foreach(Transform child in soldiers)
            child.GetComponent<SoldierTrackLayer>().StartLayingTrack(nextSpawnSpot);

    }

    //Public functions
    public void LastTrackKilled()
    {
        Debug.Log("LastTrackKIlled()");
        nextSpawnSpot = lastSpawnSpot;
        trackPieceBroke = true;
        //SetNextSpawnSpot();
        trackPieceBroke = false;
        Debug.Log("after last track killed, new last track is " + lastTrack.name);
        StartCoroutine(Delay());
       
        
        
    }
}
