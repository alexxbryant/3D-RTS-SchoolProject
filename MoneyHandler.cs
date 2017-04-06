using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyHandler : MonoBehaviour {
    public Transform claimedMines;
    public Transform castleMenu;
    public float goldSaved;

	// Use this for initialization
	void Start () {
        goldSaved = 10;
    //    castleMenu.Find("Panel").Find("insetPanel").Find("text").GetComponent<Text>().text = "Gold Saved: " + goldSaved;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddMoreGold(float amount)
    {
        goldSaved += amount;
        castleMenu.Find("Panel").Find("insetPanel").Find("text").GetComponent<Text>().text = "Gold Saved: " + goldSaved;

    }
}
