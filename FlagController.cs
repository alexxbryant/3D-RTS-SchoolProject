using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour {
    bool moving;
    float yVal;
    float originalY;
    float bottomY;
	// Use this for initialization
	void Start () {
        moving = false;
        yVal = -2.5f;
        bottomY = yVal;
        originalY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, yVal, transform.position.z), Time.deltaTime*20);
        }
	}
  void OnTriggerEnter(Collider col)
    {
//        Debug.Log("Trigger Entered, col: " + col.name);
        if(col.name == "Top")
        {
            moving = false;
            transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
        }else if(col.name == "Bottom")
        {
            yVal = originalY;
        }
    }
    public void StartFlagMovement()
    {
        moving = true;
        yVal = bottomY;
    }
}
