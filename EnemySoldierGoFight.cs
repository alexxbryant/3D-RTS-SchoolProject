using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierGoFight : MonoBehaviour {
    public Transform goodSoldiers;
    Transform currentEnemy;
    static bool attackingGoodSoldiers = false;
    SoldierHealthManager healthScript;
    float shootingCooldownTimer;
	// Use this for initialization
	void Start () {
        shootingCooldownTimer = 0f;
        if (!goodSoldiers)
        {
            goodSoldiers = GameObject.Find("GoodSoldiers").transform;
        }
        healthScript = transform.GetComponent<SoldierHealthManager>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckForGoodSoldiers();
        if (attackingGoodSoldiers)
            shootingCooldownTimer += Time.deltaTime;

	}
    bool CheckForGoodSoldiers()
    {
        bool goodGuysAround = false;
        if (!attackingGoodSoldiers)
        {
            foreach (Transform child in goodSoldiers)
            {
                if (Vector3.Distance(transform.position, child.position) < 50 && !attackingGoodSoldiers)
                {
                    attackingGoodSoldiers = true;
                    currentEnemy = child;
                    goodGuysAround = true;
                }
            }
        }else
        {
            if (currentEnemy)
                StopMovingAndFight(currentEnemy);
            else
                Debug.Log("should be fighting an enemy, but it is not assigned - EnemySoldierGoFight");
        }
        return goodGuysAround;
    }
    void StopMovingAndFight(Transform soldier)
    {
        
        Debug.Log("Stopmoving and fight called");
     //   transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
         if(shootingCooldownTimer > 5)
        {
            soldier.GetComponent<SoldierHealthManager>().TakeDamage(healthScript.GetDamageStrength());
            shootingCooldownTimer = 0;
        }

    }
}
