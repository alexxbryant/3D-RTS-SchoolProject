using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : MonoBehaviour {
    bool enemySoldiersDead;
    bool trainDead;
	// Use this for initialization
	void Start () {
        enemySoldiersDead = false;
        trainDead = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void setEnemySoldiersDead(bool status)
    {
        enemySoldiersDead = status;
    }
    public void setTrainDead(bool status)
    {
        trainDead = status;
    }
}
