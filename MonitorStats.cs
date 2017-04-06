using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorStats : MonoBehaviour {
    public Transform playerBase;
    public Transform enemyBase;
	// Use this for initialization
	void Start () {
        playerBase = GetComponent<Transform>();
        enemyBase = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void FixedUpdate()
    {
        if(!playerBase.gameObject.activeInHierarchy || !enemyBase.gameObject.activeInHierarchy)
        {
            Debug.Log("Game Over");
            transform.GetChild(1).gameObject.SetActive(true);
            if (!playerBase.gameObject.activeInHierarchy)
            {
                Debug.Log("You lose");
            }else
            {
                Debug.Log("You win!");
            }
        }else
        {
        }
    }
}
