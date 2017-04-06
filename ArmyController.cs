using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour {
    // Use this for initialization
    public Transform train;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SendOutWholeArmyToFight()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<SoldierNav>().SetDestination(train);
            child.GetComponent<SoldierNav>().TurnConstantCheckTo(true);
        }
    }
}
