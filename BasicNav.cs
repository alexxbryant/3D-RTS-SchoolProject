using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicNav : MonoBehaviour {
    public Transform destination;
	// Use this for initialization
	void Start () {
        transform.GetComponent<NavMeshAgent>().SetDestination(destination.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
