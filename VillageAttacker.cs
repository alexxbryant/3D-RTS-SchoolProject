using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageAttacker : MonoBehaviour {
    public GameObject village;
    public GameObject cannonBallPrefab;
    GameObject cannonBall;
    Transform nextTarget;
    float coolDown;
    float timer;
    bool readyToFire;
    int numHousesAttacked;

    CannonFire cannonFireScript;
	void Start () {
        village = GameObject.Find("Village");
        coolDown = 3;
        timer = 0;
        readyToFire = false;
        numHousesAttacked = 0;
      //  cannonBall = GameObject.Find("CannonBall");
      //  cannonBall.GetComponent<MeshRenderer>().enabled = false;
     //   cannonFireScript = cannonBall.GetComponent<CannonFire>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer >= coolDown)
        {
            readyToFire = true;
            timer = 0;
        }
    }
    void FixedUpdate()
    {
        if (readyToFire)
        {
            SelectNextTarget();
            // transform.LookAt(nextTarget, Vector3.left);
           // cannonFireScript.LaunchAt(nextTarget.position, 10);
            AttackNextTarget();
        }
    }
    void SelectNextTarget()
    {
        Debug.Log("Called SelectNextTarget from Village Attacker");
        float x = 0;
        float y = 0;
        float distance;
        float smallestDistance = 10000;
        nextTarget = village.transform.GetChild(0);
        foreach (Transform child in village.transform)
        {
            x = Mathf.Abs(transform.position.x - child.position.x);
            y = Mathf.Abs(transform.position.y - child.position.y);
            distance = Mathf.Sqrt((x * x) + (y * y));
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                nextTarget = child;
            }
        }
    }
    void AttackNextTarget()
    {
        Debug.Log("Called AttackNextTarget from Village Attacker");

        nextTarget.GetChild(3).gameObject.SetActive(true); //The fourth child is the fire;
        //cannonBall = (GameObject)Instantiate(cannonBallPrefab, new Vector3(5,5,5), Quaternion.identity);
        //cannonBall.GetComponent<MeshRenderer>().enabled = true;
        //cannonFireScript.LaunchAt(nextTarget.position, 10);
        //cannonBall.GetComponent<TrainTravel>().enabled = false;
        readyToFire = false;
        numHousesAttacked++;
    }
    public int GetHousesAttacked()
    {
        return numHousesAttacked;
    }
}
