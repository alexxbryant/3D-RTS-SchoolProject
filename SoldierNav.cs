using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierNav : MonoBehaviour {
    Transform Destination;
    public Transform enemySoldiers;
    public float shootingCooldown = 2;

    private NavMeshAgent navAgent;
    SoldierHealthManager healthScript;
    Transform target;
    SoldierHealthManager targetHealth;
    bool attackTarget;
    bool constantlyCheckTargetPos;
    float enemyChecktimer;
    float shootTimer;
    // Use this for initialization
    void Start () {
      //  transform.position = new Vector3(10, 0, 10);
        navAgent = this.GetComponent<NavMeshAgent>();
        if (!Destination)
            Destination = GameObject.Find("Train").transform;

     //   Destination = GameObject.Find("Scaler").transform.Find("Train");
        //navAgent.destination = Destination.position;
        healthScript = GetComponent<SoldierHealthManager>();

        attackTarget = false;
        constantlyCheckTargetPos = true;
        enemyChecktimer = 0;
        shootTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(constantlyCheckTargetPos && Destination)
            navAgent.destination = Destination.position;
        enemyChecktimer += Time.deltaTime;
        shootTimer += Time.deltaTime;
        if(tag == "GoodSoldier" && enemyChecktimer >= 1)
        {
            CheckForEnemies();
            enemyChecktimer = 0;
        }
        if (attackTarget && shootTimer >= shootingCooldown)
        {
            Attack(target, targetHealth);
            shootTimer = 0;
        }
    }
    void CheckForEnemies()
    {
        Debug.Log("Calling CheckForEnemies from: " + name );

        if(enemySoldiers.childCount> 0)
        {
            targetHealth = enemySoldiers.GetChild(0).GetComponent<SoldierHealthManager>();
            foreach (Transform child in enemySoldiers)
            {
                if(Vector3.Distance(child.position, transform.position) < (GetComponent<MeshRenderer>().bounds.size.x *10))
                {
                     targetHealth = child.GetComponent<SoldierHealthManager>();
                    //Stop
                    navAgent.Stop();
                    child.GetComponent<NavMeshAgent>().Stop();
                    //Fight
                    target = child;
                    attackTarget = true;

                }
            }
        }



    }
    void Attack(Transform target, SoldierHealthManager targetHealth)
    {
        if (targetHealth)
        {
        targetHealth.TakeDamage(healthScript.damage - targetHealth.armor);
        healthScript.TakeDamage(targetHealth.damage - healthScript.armor);
        }

        shootTimer = 0;
    }
    public void SetDestination(Transform spot)
    {
        // navAgent.SetDestination(new Vector3(1,1,1));
        //this.GetComponent<NavMeshAgent>().SetDestination(spot);
        Destination = spot;
    }
    public void SetRefToEnemies(Transform targets)
    {
        enemySoldiers = targets;
    }
    public void TurnConstantCheckTo(bool status)
    {
        constantlyCheckTargetPos = status;
    }
}
