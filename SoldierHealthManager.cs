using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHealthManager : MonoBehaviour {
    public  float health = 100;
    public  float damage = 10;
    public float armor = 5;
    public float upgradeAttackFactor = 1.3f;
    public float upgradeArmorFactor = 1.5f;

    public Transform healthBar;
    public Transform healthbarSegments;
    bool dead = false;
    bool shouldRemoveSegment = false;
    float deathTimer;
    int numActiveSegments;
    //  AudioSource[] sounds;
    //   bool deathAnimaitonHappend = false;

    // Use this for initialization
    void Start () {
      //  sounds = GetComponents<AudioSource>();
        deathTimer = 0;
        if(!healthBar)
            healthBar = transform.Find("HealthBar");
        if(!healthbarSegments)
            healthbarSegments = healthBar.Find("Segments");
        numActiveSegments = healthbarSegments.childCount;

    }
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            deathTimer += Time.deltaTime;
            
        }
	}
    void FixedUpdate()
    {

        if (dead)
        {
            if (deathTimer > 5)
            {
                Debug.Log("die called on " + transform.name);
        //        if (!deathAnimaitonHappend)
       //         {
                    healthBar.gameObject.SetActive(false);
         //           sounds[0].enabled = false;
       //             sounds[1].enabled = true;
        //            sounds[1].Play();
        //            transform.GetComponent<Animator>().SetBool("die", true);
        //            deathAnimaitonHappend = true;
        //        }

                if (deathTimer > 10)
                    Destroy(transform.gameObject);
            }
        }

    }
    public void TakeDamage(float amt)
    {
        shouldRemoveSegment = true;
      //  Debug.Log("TakeDamage called");
      //  Debug.Log(transform.name + " took " + amt + " damage");
        health -= amt;
        if(numActiveSegments > 1)
        {
            healthbarSegments.GetChild(numActiveSegments - 1).gameObject.SetActive(false);
          //  Debug.Log("Should lower health bar number: " + (numActiveSegments-1));
            numActiveSegments--;
        }else
        {
            Die();
        }

    }

    void Die()
    {
   //     Debug.Log("die called on " + transform.name);
        dead = true;
   //     sounds[0].enabled = false;
   //     sounds[1].enabled = true;
   //     sounds[1].Play();
   //     transform.GetComponent<Animator>().SetBool("die", true);
   //     if (deathTimer > 5)
    //        Destroy(transform.gameObject);
    }

    public float GetDamageStrength()
    {
        return damage;
    }
    public float GetArmorStrength()
    {
        return armor;
    }

    //Upgrades
    public void UpgradeArmor()
    {
        armor *= upgradeArmorFactor;
    }
    public void UpgradeDamage()
    {
        damage *= upgradeAttackFactor;
    }
    public void ResetSoldierPrefabHealthStats()
    {
        health = 100;
        damage = 10;
        armor = 5;
    }
}
