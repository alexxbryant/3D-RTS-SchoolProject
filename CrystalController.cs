using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour {
    bool spinning = true;
    public float maxSpinSpeed = 500;
    public float minSpinSpeed = 10;
    float spinSpeed;
    float timer = 0;
    Vector3 startPos; //To eventually be set where player opens their hand
    Vector3 finishPos;
	// Use this for initialization
	void Start () {
        spinSpeed = minSpinSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (spinning)
        {
            if (spinSpeed < maxSpinSpeed)
                spinSpeed*=1.01f;
           transform.Rotate(new Vector3(Time.deltaTime * spinSpeed, Time.deltaTime * spinSpeed, 0));
        }
        if(timer >= 3)
        {
            timer = 0;
        }
	}
}
