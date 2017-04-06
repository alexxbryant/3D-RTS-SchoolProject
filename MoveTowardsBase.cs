using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsBase : MonoBehaviour {
    public GameObject castleBase;
    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
    void FixedUpdate()
    {
        if(transform.position != castleBase.transform.position)
            transform.position = Vector3.MoveTowards(transform.position, castleBase.transform.position, 3);

    }

}
