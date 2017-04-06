using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierRally : MonoBehaviour {
    public Transform barracks;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
 /*   void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Soldier")
        {
            Debug.Log("bumped soldier");
            barracks.GetComponent<BarracksController>().SoldierReportsAtRally();
        }
    } */
}
