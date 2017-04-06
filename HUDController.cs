using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    MineTracker mineTracker;
    Text goldText;
	// Use this for initialization
	void Start () {
        mineTracker = GameObject.Find("Cube_A").GetComponent<MineTracker>();
        goldText = transform.GetChild(0).GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void UpdateGoldAmt()
    {
      //  goldText.text = "Gold: " + mineTracker.GetGoldSaved();
    }
}
