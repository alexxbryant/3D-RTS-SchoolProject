using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksController : MonoBehaviour {
    Vector3 spawnSpot;
    Vector3 rallyPoint;
    GameObject newSoldier;
    bool soldierWalkingToRally;
    public int costOfSoldiers;
    public float speedSoldiersRally;
    public GameObject soldierPrefab;
    public GameObject allGoodSoldiers;
    public Transform castlePanel;

	// Use this for initialization
	void Start () {
        spawnSpot = transform.Find("spawnSpot").position;
        rallyPoint = transform.Find("FlagSpot").Find("Pole").Find("InFrontOfPole").position;
        rallyPoint = new Vector3(rallyPoint.x, 0, rallyPoint.z);
        soldierWalkingToRally = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        //Keeping this in the barracks script is fine as long as only one soldier is walking to the rally at a time
        if (soldierWalkingToRally)
        {
            Vector3 tempRally = new Vector3(rallyPoint.x, newSoldier.transform.position.y, rallyPoint.z);
            newSoldier.transform.LookAt(tempRally); 
            newSoldier.transform.position = Vector3.MoveTowards(newSoldier.transform.position,tempRally, Time.deltaTime * speedSoldiersRally);
            if(Mathf.Abs(newSoldier.transform.position.x - rallyPoint.x) < .5)
            {
                soldierWalkingToRally = false;
                newSoldier.GetComponent<Animator>().SetBool("idle", true);
                newSoldier.GetComponent<Animator>().SetBool("running", false);
            }
        }	
        
	}

    public void SpawnSoldier()
    {
        if(GlobalVariables.goldSaved >= costOfSoldiers)
        {
            GlobalVariables.goldSaved -= costOfSoldiers;
            castlePanel.GetComponent<CastlePanelController>().UpdateGoldText();
            newSoldier = Instantiate(soldierPrefab, allGoodSoldiers.transform, true);
            newSoldier.transform.position = spawnSpot;
            newSoldier.GetComponent<SoldierRally>().barracks = transform;
            newSoldier.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            newSoldier.transform.rotation = Quaternion.Euler(0, newSoldier.transform.rotation.y, 0);
            soldierWalkingToRally = true;
        }

        
    }
    void OnMouseDown()
    {
        SpawnSoldier();
    }
    public void SoldierReportsAtRally()
    {

        soldierWalkingToRally = false;
        newSoldier.GetComponent<Animator>().SetBool("idle", true);
        newSoldier.GetComponent<Animator>().SetBool("running", false);
        newSoldier.GetComponent<SoldierRally>().enabled = false;
        newSoldier.GetComponent<SoldierGoFight>().enabled = true;
        newSoldier.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        newSoldier.transform.rotation = Quaternion.Euler(0, newSoldier.transform.rotation.y, 0);
        

    }
    public void SendSoliderToFightFromBarracks()
    {
        foreach(Transform child in allGoodSoldiers.transform)
        {
            child.GetComponent<Animator>().SetBool("idle", false);
            child.GetComponent<Animator>().SetBool("running", true);
            child.GetComponent<SoldierRally>().enabled = false;
            child.GetComponent<SoldierGoFight>().enabled = true;
        }
        
    }
}
