using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackHealthManager : MonoBehaviour {
    public static float health = 50;
    static bool trackBroken = false;
    TrackLayer trackLayerScript; 
	// Use this for initialization
	void Start () {
        trackLayerScript = GameObject.Find("Cube_B").GetComponent<TrackLayer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !trackBroken)
        {
            Destroy(transform.gameObject);
            Debug.Log(transform.name + " should be dead");
            trackLayerScript.LastTrackKilled();
            trackBroken = true;
        }

    }
}
