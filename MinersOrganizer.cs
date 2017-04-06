using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinersOrganizer : MonoBehaviour {


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
  /*  public void SendOutUnassignedMinerToExplore(Transform zone)
    {
        foreach(Transform child in transform)
        {
            if (!child.GetComponent<MinerNav>().isAssignedToAMine)
            {
                child.GetComponent<MinerNav>().SetMineZone(zone);
                child.GetComponent<MinerNav>().SearchForNextMine();

            }
        }
    }
    public void TurnOnZoneCanvas(Transform zone)
    {
        zone.Find("MineZoneCanvas").Find("Panel").gameObject.SetActive(true);
    }*/

}
