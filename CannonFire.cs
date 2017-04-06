using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFire : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LaunchAt(Vector3 target, float speed)
    {
        Debug.Log("shooting at target: " + target.ToString());
        Vector3 p1, p2;
        p1 = new Vector3(target.x, target.y + (target.y * .4f), target.z);
        p2 = new Vector3(target.x, target.y + (target.y * .5f), target.z);
        //Use p1 and p2 to fake an arc by changing the destination at different times

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }
}
