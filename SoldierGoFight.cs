using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGoFight : MonoBehaviour {
    public Transform lastTrainTrack;
    public Transform tracks;
    public Transform enemySoldiers;
    public float soldierRunSpeed;
    float soldierDamage;
    static float gunFireTimer;
    static bool attacking;
    static bool moving;
    static bool attackingEnemySoldiers;
    static bool targetsNearby;
    SoldierHealthManager healthScript;
    AudioSource[] sounds;
    // Use this for initialization
    void Start () {
        attacking = true;
        attackingEnemySoldiers = false;
        moving = true;
        targetsNearby = false;
        gunFireTimer = 0;
        if (soldierRunSpeed == 0)
            soldierRunSpeed = 10;
        if (!tracks)
        {
            tracks = GameObject.Find("Tracks").transform;
        }
        if (!enemySoldiers)
        {
            enemySoldiers = GameObject.Find("TrainCart").transform.Find("Soldiers").transform;
        }
        healthScript = transform.GetComponent<SoldierHealthManager>();
        soldierDamage = healthScript.GetDamageStrength();
        sounds = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        //Keep solider upright
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        if (attacking && moving)
        {
            GetLastTrack();
            Vector3 goTo = new Vector3(lastTrainTrack.position.x, transform.position.y, lastTrainTrack.position.z);
            transform.LookAt(goTo);
            transform.position = Vector3.MoveTowards(transform.position, goTo, Time.deltaTime * soldierRunSpeed);
            AttackObjectsWithinZone();
        }
        gunFireTimer += Time.deltaTime;
	}

    public void GetLastTrack()
    {
        //       foreach(Transform child in tracks)
        //       {
        //           lastTrainTrack = child;
        //        }
        lastTrainTrack = tracks.GetChild(tracks.childCount - 1);
    }
    void AttackObjectsWithinZone()
    {
        if (!attackingEnemySoldiers)
        {
            moving = true;
            foreach(Transform child in enemySoldiers)
            {
                if(Vector3.Distance(transform.position, child.position) < 50 && !attackingEnemySoldiers)
                {
                    attackingEnemySoldiers = true;
                    targetsNearby = true;
                    StopMovingAndFight(child);
                }
            }
        }


    }
    void StopMovingAndFight(Transform target)
    {
        if (!target)
        {
            moving = true;
            attackingEnemySoldiers = false; //If the target has died, stop attacking and search for more targets
        }else
        {
           
            moving = false; // stop moving
            //do damage
            target.GetComponent<SoldierHealthManager>().TakeDamage(soldierDamage);
            sounds[0].enabled = true;//play bullet sound burst
            Debug.Log("attacking soldiers");
        }

    }
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Track" && !attackingEnemySoldiers)
        {
            Debug.Log("Stopped For Track - soldiergofight");
            moving = false;
            transform.GetComponent<Animator>().SetBool("idle", true);
            transform.GetComponent<Animator>().SetBool("running", false);
            transform.GetComponent<AudioSource>().enabled = true;
            col.GetComponent<TrackHealthManager>().TakeDamage(soldierDamage);

        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Track")
        {
            if(gunFireTimer >= 5)
            {
                gunFireTimer = 0;
                col.GetComponent<TrackHealthManager>().TakeDamage(soldierDamage);
                Debug.Log("Attacking track");
            }
        }
    }

}
