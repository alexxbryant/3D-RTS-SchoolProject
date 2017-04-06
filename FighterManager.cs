using UnityEngine;
using System.Collections;

public class FighterManager : MonoBehaviour {
    public float damageAmount;
    public float health;
    public Transform targetEnemyBase;
    public int goldCostToMake;


    Animator animator;
    Transform arena;
    Transform targetEnemyFighter;
    Vector3 destination;
    Rigidbody rigidbodyy;
    bool moving, fighting; //Animation related
    bool ultimateDestinationReached, readyToMovePastArena, inArena; //Transform position related
    bool fightingMovingEnemy;
    float xdiff, ydiff, zdiff;
    float timer;
    private int numberOfEnemyUnits;
    int runSpeed;

    CastleClick enemyScriptCastleClick;
    CastleClick parentScriptCastleClick;
    FighterManager enemyScriptFighterManager;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        arena = GameObject.Find("Arena").transform;
        targetEnemyBase = GameObject.Find("Cube_B").transform;
        if(GlobalVariables.usingArenaThisRound)
            destination = new Vector3(arena.position.x, arena.position.y, arena.position.z);
        else
        {
            destination = new Vector3(targetEnemyBase.position.x, targetEnemyBase.position.y, targetEnemyBase.position.z);
        }
        rigidbodyy = transform.GetComponent<Rigidbody>();

        damageAmount = 10;
        health = 30;
        numberOfEnemyUnits = 0;
        runSpeed = 10;

        inArena = false;
        readyToMovePastArena = false;
        ultimateDestinationReached = false;
        moving = false;
        fightingMovingEnemy = true;

        goldCostToMake = 5;

        if (animator)
        {
            animator.SetBool("moving", true);
            animator.SetBool("fighting", false);
        }

        ResetBasicConstraints();
    }
	
	// Update is called once per frame
    void Update()
    {

    }
	void FixedUpdate () {
	    if(!ultimateDestinationReached && moving)
        {
            MoveTo(destination);
        }
        if (fightingMovingEnemy)
        {
            destination = new Vector3(targetEnemyBase.position.x, targetEnemyBase.position.y, targetEnemyBase.position.z);
        }
	}
    //Trigger Functions
    void OnTriggerEnter(Collider col)
    {
        if(GlobalVariables.usingArenaThisRound && col.gameObject.tag == "Arena")
        {
            inArena = true;
             Debug.Log("Upon entering arena: num enemies is " + numberOfEnemyUnits);
        }else if(col.gameObject.tag == "SummoningZone" && !GlobalVariables.readyToStartRound)
        {/* for summoning zone, not relevant to level1
            rigidbodyy.constraints = RigidbodyConstraints.FreezePosition;
            moving = false;
            fighting = false;
            if (animator)
            {
                 animator.SetBool("moving", false);
                 animator.SetBool("fighting", false);
            }
            */
        }
    }
    void OnTriggerStay(Collider col)
    {
        Debug.Log("Trigger Stay activated");
        if (GlobalVariables.usingArenaThisRound && col.gameObject.tag == "Arena")
        {
            if (targetEnemyBase.childCount == 0)
            { //only move towards enemy base if you defeat enemy fighters
               // Debug.Log("No one for " + transform.name + " to fight");
                fighting = false;
                if (animator)
                {
                    animator.SetBool("fighting", false);
                    animator.SetBool("moving", true);
                }
                
                destination = targetEnemyBase.transform.position;
                transform.LookAt(destination);
                ResetBasicConstraints();
            }
            else if(inArena || ultimateDestinationReached)
            {
               // Debug.Log("Someone one for " + transform.name + " to fight!!!!");
                fighting = true;
                animator.SetBool("fighting", true);
                //Find nearest enemy
                TargetNearestEnemy();
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (GlobalVariables.usingArenaThisRound && col.gameObject.tag == "Arena")
        {
            inArena = false;
            moving = true;
            fighting = false;
            if (animator)
            {
                animator.SetBool("moving", true);
                animator.SetBool("fighting", false);
            }
            
        }

    }
    //Collision Functions
    void OnCollisionEnter(Collision collision)
    {
 //       Debug.Log(transform.name + " collided with " + collision.collider.name);
        
        if (collision.collider.tag == "Fighter" && !ultimateDestinationReached)
        {
            if (inArena)
            {
                Debug.Log("in the arena, met a fighter, should fight");
                moving = false;
                fighting = true;
                if (animator)
                {
                    animator.SetBool("moving", false);
                    animator.SetBool("fighting", true);
                }
                
                rigidbodyy.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                Debug.Log("not in the arena, met a fighter, pushing past");
                //So fighter units can push past one another
                
            }
        }
        else if (collision.collider.tag == "Base")
        {
            if (!collision.gameObject.Equals(transform.parent))
            {
                if (!fightingMovingEnemy)
                {
                    rigidbodyy.constraints = RigidbodyConstraints.FreezeAll;
                    ultimateDestinationReached = true;
                }

            }
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if(collision.collider.tag == "Base")
        {
            if (!collision.gameObject.Equals(transform.parent))
            {
                ultimateDestinationReached = true;
                moving = false;
                fighting = true;
                if (animator)
                {
                    animator.SetBool("moving", false);
                    animator.SetBool("fighting", true);
                }
                timer += Time.deltaTime;
                if(timer > 2.5f)
                {
                    enemyScriptCastleClick = collision.collider.GetComponent<CastleClick>();
                    enemyScriptCastleClick.TakeDamage(damageAmount);
                    Debug.Log("damaged castle by: " + damageAmount);
                    timer = 0;
                }
            }

        }else if(collision.collider.tag == "Fighter")
        {
            if (!isSibling(collision.collider.gameObject))
            {
                moving = false;
                fighting = true;
                if (animator)
                {
                    animator.SetBool("moving", false);
                    animator.SetBool("fighting", true);
                }

                rigidbodyy.constraints = RigidbodyConstraints.FreezeAll;
                timer += Time.deltaTime;
                if (timer > 2.5f && (inArena || ultimateDestinationReached))
                {
                    enemyScriptFighterManager = collision.collider.gameObject.GetComponent<FighterManager>();
                    enemyScriptFighterManager.takeDamage(damageAmount);
                    Debug.Log("damaged other fighter by " + damageAmount);
                    timer = 0;
                }
            }
            else
            {

                //rigidbodyy.AddForce(Vector3.forward * 10, ForceMode.Force);
                transform.LookAt(destination);
               rigidbodyy.AddTorque(Vector3.forward * 2, ForceMode.Impulse);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Fighter")
        {
            ResetBasicConstraints();
        }
        else if(collision.collider.tag == "Base")
        {
            fighting = false;
            if (animator)
            {
                 animator.SetBool("fighting", false);
            }

        }
    }
    void OnDestroy()
    {
        GlobalVariables.casualties++;
        Debug.Log(gameObject.name + " has been slain");
    }
    void MoveTo(Vector3 destination)
    {
        xdiff = transform.position.x - destination.x;
        ydiff = transform.position.y - destination.y;
        zdiff = transform.position.z - destination.z;
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime*runSpeed);
    }
    public void SetEnemyBase(Transform e)
    {
        targetEnemyBase = e;
        //If you have an enemy, parent is already assigned.
        parentScriptCastleClick = transform.parent.GetComponent<CastleClick>();
    }
    public void setNumEnemyUnits(int num)
    {
        numberOfEnemyUnits = num;
        Debug.Log("From Fighter: tracking number of enemy units:" + numberOfEnemyUnits);
    }
    bool isSibling(GameObject obj)
    {
        if (obj.transform.parent.Equals(transform.parent))
        {
            return true;
        }
        return false;
    }
    public void takeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            // transform.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    void Delay(int seconds)
    {
        float clock = 0;
        while(clock < seconds)
        {
            clock += Time.deltaTime;
        }
    }
    public void SetDestination(Vector3 newDest)
    {
       // Debug.Log("destination set to " + newDest);
        destination = newDest;
        transform.LookAt(destination);
    }
    public int GetGoldCost()
    {
        return goldCostToMake;
    }
    public void BeginFightPhase()
    {
        GlobalVariables.readyToStartRound = true;
        destination = new Vector3(arena.position.x, arena.position.y, arena.position.z);
        transform.LookAt(destination);
        moving = true;
        if (animator)
        {
            animator.SetBool("moving", true);
        }

        ResetBasicConstraints();

    }
    bool IsEnemyFighter(Collider col)
    {
        if (transform.name != col.name)
            return true;
        return false;
    }
    void TargetNearestEnemy()
    {
        float minXDistance, minYDistance, minDistance, tempX, tempY, tempD;
        Transform enemyFighterTargeted;
        minXDistance = Mathf.Abs(targetEnemyBase.GetChild(0).position.x - transform.position.x);
        minYDistance = Mathf.Abs(targetEnemyBase.GetChild(0).position.y - transform.position.y);
        minDistance = Mathf.Sqrt((minXDistance * minXDistance) + (minYDistance * minYDistance));
        enemyFighterTargeted = targetEnemyBase.GetChild(0);
        foreach (Transform child in targetEnemyBase)
        {
            tempX = Mathf.Abs(child.position.x - transform.position.x);
            tempY = Mathf.Abs(child.position.y - transform.position.y);
            tempD = Mathf.Sqrt((tempX * tempX) + (tempY * tempY));
            if (tempD < minDistance)
            {
                minDistance = tempD;
                enemyFighterTargeted = child;
            }
        }
        destination = enemyFighterTargeted.position;
        transform.LookAt(destination);
    }
    void ResetBasicConstraints()
    {
        rigidbodyy.constraints = RigidbodyConstraints.None;
        rigidbodyy.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

}

