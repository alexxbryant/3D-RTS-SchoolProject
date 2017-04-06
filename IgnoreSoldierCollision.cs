using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSoldierCollision : MonoBehaviour {
    public Transform Soldiers;
	// Use this for initialization
	void Start () {
        foreach(Transform child in Soldiers)
        {
            Physics.IgnoreCollision(child.GetComponent<Collider>(), GetComponent<Collider>());

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
