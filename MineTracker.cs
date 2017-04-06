using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTracker : MonoBehaviour {
    float goldSaved;
    //Other resources TBD
	// Use this for initialization
	void Start () {
        goldSaved = 10;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void IncreaseGoldAmount(float amt)
    {
        goldSaved += amt;
    }
    public void DecreaseGoldAmount(float amt)
    {
        goldSaved -= amt;
    }
    public float GetGoldSaved()
    {
        return goldSaved;
    }
}
