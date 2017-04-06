using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CastleClick : MonoBehaviour {
    public GameObject fightingUnit1;
    public GameObject usableMines;
 //   public GameObject mainMenu;
    public GameObject barracks;
    public GameObject castleMenu;
    public Transform enemyBase;
    public float health;
    static float x_offset, z_offset;
    float scaleOfWorld;

    GameObject cubeSpawn;
    Transform summoningZone;
    Vector3 spawnSpot;
    float x, y, z;
    int numberOfFriendlyUnits;
    int numberOfEnemyUnits;
    int fightingUnitCost; //!Currently hardcoded, will change when multiple units can be selected.
    bool fighting;


    FighterManager fighterScriptRef;
    CastleClick enemyCastleScript;
    CanvasController canvasControllerScript;

	// Use this for initialization
	void Start () {
        numberOfFriendlyUnits = 0;
        numberOfEnemyUnits = 0;
        health = 50;
        fightingUnitCost = 5;
        fighting = false;
        spawnSpot = new Vector3(transform.position.x - (transform.GetComponent<MeshRenderer>().bounds.extents.x * 1.5f), transform.position.y, transform.position.z-(transform.GetComponent<MeshRenderer>().bounds.extents.z*1.005f));
        scaleOfWorld = GameObject.Find("Scaler").transform.localScale.x;
        x_offset = 0;
        z_offset = -15f * scaleOfWorld;


        if (this.name == "Cube_A")
        {
            enemyBase = GameObject.Find("Cube_B").transform;
            usableMines = GameObject.Find("Mines_A");
            summoningZone = GameObject.Find("SummoningZone_A").transform;
        }
        else if (this.name == "Cube_B")
        {
            enemyBase = GameObject.Find("Cube_A").transform;
            usableMines = GameObject.Find("Mines_B");
            summoningZone = GameObject.Find("SummoningZone_B").transform;
        }

        enemyCastleScript = enemyBase.GetComponent<CastleClick>();
        canvasControllerScript = GameObject.FindObjectOfType<Canvas>().GetComponent<CanvasController>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (fighting)
        {
            if (CheckIfRoundOver())
            {
                fighting = false;

            }
        }
        if (GlobalVariables.endOfGame)
        {
  //          mainMenu.gameObject.SetActive(true);
  //          mainMenu.transform.GetChild(1).gameObject.SetActive(true);
        }
	}
    void OnMouseDown()
    {
     //   SpawnFighter();
        barracks.GetComponent<BarracksController>().SpawnSoldier();
    }
    void OnMouseOver()
    {
        castleMenu.transform.Find("Panel").gameObject.SetActive(true);
    }
    void SpawnFighter()
    {
        if (GlobalVariables.goldSaved > fightingUnitCost)
        {
            GlobalVariables.goldSaved -= fightingUnitCost;
            castleMenu.transform.Find("Panel").GetComponent<CastlePanelController>().UpdateGoldText();
            // usableMines.GetComponent<MineController>().Spend(fightingUnitCost);
            numberOfFriendlyUnits++;
            if(enemyCastleScript)
                 enemyCastleScript.setNumEnemyUnits(numberOfEnemyUnits + 1);
            
            if (transform.localPosition.x > 0)
            {
             //   x_offset *= -1;
            }
            cubeSpawn = (GameObject)Instantiate(fightingUnit1, spawnSpot, Quaternion.identity);

            fighterScriptRef = cubeSpawn.GetComponent<FighterManager>();

           // cubeSpawn.transform.localPosition = new Vector3(transform.localPosition.x + x_offset, transform.localPosition.y, transform.localPosition.z);
            cubeSpawn.name = "ThisCubeSpawn";
            cubeSpawn.transform.parent = transform;
    
            cubeSpawn.transform.localScale = new Vector3(.375f, .375f, .375f);
            fighterScriptRef.SetDestination(summoningZone.position);
            cubeSpawn.transform.LookAt(summoningZone);
            
         //   x_offset *= -1;
         // x_offset += transform.GetComponent<MeshRenderer>().bounds.extents.x*.2f;

            cubeSpawn.transform.position = new Vector3(cubeSpawn.transform.position.x + x_offset, cubeSpawn.transform.position.y , cubeSpawn.transform.position.z + z_offset);
            // z_offset += transform.GetComponent<MeshRenderer>().bounds.extents.z * .2f;
            if(transform.childCount % 5 != 0)
            {
                z_offset += cubeSpawn.GetComponent<MeshRenderer>().bounds.extents.x*1.7f;

            }
            else
            {
                z_offset = -15 * scaleOfWorld;
                x_offset -= cubeSpawn.GetComponent<MeshRenderer>().bounds.extents.x * 1.7f;
            }

            if (transform.name == "Cube_A")
            {
                fighterScriptRef.SetEnemyBase(GameObject.Find("Cube_B").transform);
                cubeSpawn.name = "Fighter_A";
            }
            else
            {
                fighterScriptRef.SetEnemyBase(GameObject.Find("Cube_A").transform);
                cubeSpawn.name = "Fighter_B";
            }
        }else
        {
            Debug.Log("Not enough gold to make fighting unit");
        }

    }
    void OnCollisonEnter(Collision col)
    {
        if(col.gameObject.tag == "Fighter")
        {
            //TakeDamage(smallDamage); // should let the fightingunit say what damage gets taken
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            transform.gameObject.SetActive(false);

        }
    }
    public void setNumEnemyUnits(int num)
    {
        numberOfEnemyUnits = num;
        Debug.Log("from castle" + gameObject.name + " : setting number of enemy units to : " + numberOfEnemyUnits);
        FighterManager tempScriptRef;
        foreach(Transform child in transform)
        {
            if(child.tag == "Fighter")
            {
                tempScriptRef = child.GetComponent<FighterManager>();
                tempScriptRef.setNumEnemyUnits(numberOfEnemyUnits);
            }

        }
    }
    public int GetNumEnemyUnits()
    {
        return numberOfEnemyUnits;
    }
    public Vector3 GetPositionOfSummonZone()
    {
        return new Vector3(summoningZone.position.x, summoningZone.position.y, summoningZone.position.z);
    }
    public void BeginFightPhase()
    {
     /*   FighterManager tempScriptRef; legacy from when castle spawned the fighters onclick
        fighting = true;
        foreach (Transform child in transform)
        {
            tempScriptRef = child.GetComponent<FighterManager>();
            tempScriptRef.BeginFightPhase();
        }*/
    }
    public bool CheckIfRoundOver()
    {        
        if(enemyBase.gameObject.activeSelf && (numberOfEnemyUnits > 0 || numberOfFriendlyUnits > 0)){
            return false;
        }
        GlobalVariables.endOfRound = true;
      //  canvasControllerScript.ChangeMessageTo("Game Over");
        return true;  
    }
    public void SpawnFighterViaMenu()
    {
    //    SpawnFighter();
        barracks.GetComponent<BarracksController>().SpawnSoldier();
    }
    void OnDestroy()
    {
        GlobalVariables.endOfGame = true;
        Debug.Log("Game Over! " + gameObject.name + " has been destroyed.");
       // mainMenu.SetActive(true);
       // mainMenu.transform.GetChild(1).gameObject.SetActive(true);
    }
    public float GetGoldSaved()
    {
        //   return usableMines.GetComponent<MineController>().GetWealthSaved();
         return GlobalVariables.goldSaved;
       // return 10;
    }
}
public static class GlobalVariables
{
    //   public static float health = 100f;
    public static bool usingArenaThisRound = false;
    public static bool readyToStartRound = false;
    public static bool endOfRound = false;
    public static bool endOfGame = false;
    public static int casualties = 0;
    public static int goldSaved = 10;


    public static bool trainMoving = true;
}