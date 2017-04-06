/**
 * Author: Alexandra Bryant
 * Last Updated: 3/30/17
 * Description: SimpleBarracks handles fighting units training, spawining, and upgrading. It interacts with it's GUI to display
 * relevant information & options to the player.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
/***
 * Running Thoughts here:
 * Should upgrades only affect new units? I think so: currently only changing the prefab for future units
 * Need to organize & label functions based on functionality and which objects they affect.
 * HOLOLENS:    * When the user gazes at the menu it should get bigger and rise up so they can easily read & select,
 *                  and when they look away it should shrink back so it doesn't obscure the game.
 *              * Maybe status panel is always visible, and still gets larger when the user looks at it, but the queue and
 *                  the selection panel "unfold" when you look at the status panel or barracks (and would therefore not need to 
 *                  shrink/enlargen).
 *              * Since Microsoft recommends keeping things about 1m away, maybe we could (also upon gaze) optimize the distance
 *                  and positioning beyond raising it up or making it bigger, potentially moving it farther out and making it bigger
 *                  to make it more comfortable for the user to read and select
 *                  
 *              * It would be cool to do level selection by having the initial gui prompt the user to "bloom" and having a 
 *                  swipeable carausel menu with images of levels that the user could air tap on in order to select it.
 * Bugs:
 * Solders spawn in a weird spot, never at the spawn spot even when instantiated in world space (except for when given hardcoded
 * positions - probably means something is up with the heirarchy involved in the spawn spot calculation)
 */
public class SimpleBarracks : MonoBehaviour {
    //public 
    public GameObject goodSoldierPrefab;
    public GameObject minerPrefab;
    
    public Transform allGoodSoldiers;
    public Transform allMiners;
    public Transform soldiersDestination;
    public Transform enemySoldiers;
    public Transform playerMines;
    public Transform referenceSoldier;

    public Sprite soldierSprite;
    public Sprite placeholderSprite;

    public float timeToMakeSoldier = 7; //instantiated here so that it is mutable
    public float soldierCost = 3;
    public float minerCost = 3;

    //private
    static Transform barracksQueuePanel;
    Transform spawnSpot;
    Transform barracksStatusPanel;
    Transform barracksSelectionPanel; //Unit selection panel, considering changing name
    Transform rotationPanel;
    Transform nextSpriteInQueue;
    Transform lastSpriteAdded;


    float queueTimer;
    float startX;
    int currentLevelInQueue;
    int currentPosInLevel;
    bool queueFull;
    bool queueEmpty;
    bool queueTimerOn;
    bool rotating;

    SoldierHealthManager prefabHealthScript;
    void Start () {
        prefabHealthScript = goodSoldierPrefab.GetComponent<SoldierHealthManager>();
        spawnSpot = transform.Find("SpawnSpot");
        barracksQueuePanel = transform.Find("Canvas").Find("RotationPanel").Find("QueuePanel");
        currentLevelInQueue = currentPosInLevel = 0;
        nextSpriteInQueue = barracksQueuePanel.GetChild(currentLevelInQueue).GetChild(currentPosInLevel);

        barracksStatusPanel = barracksQueuePanel.parent.parent.Find("BarracksStatusPanel");
        barracksSelectionPanel = barracksQueuePanel.parent.Find("UnitSelectionPanel");
        rotationPanel = barracksQueuePanel.parent;
        queueFull = false;
        queueEmpty = true;
        queueTimerOn = false;
        rotating = false;
        queueTimer = 0;
        startX = 0;


        UpdateSoldierCostText();
        UpdateUpgradeAttackTexts();
        UpdateUpgradeArmorTexts();
    }
	
	// Update is called once per frame
	void Update () {
        if (queueTimerOn)
        {
            queueTimer += Time.deltaTime;
            if (queueTimer >= timeToMakeSoldier)
            {
                RemoveSoldierFromQueue();
                queueTimer = 0;
            }
        }
  /*      if (rotating)
        {
            rotationPanel.Rotate(100 * Time.deltaTime, 0, 0);
            if (rotationPanel.localEulerAngles.x >= 359)
            {
                rotating = false;
                Debug.Log("rotation: " + rotationPanel.localEulerAngles.x);
            }
        } */

    }
    void OnMouseDown() //Legacy, on the chopping block
    {
        Debug.Log("mouse down over barracks");
        //SpawnSoldier();
        RotatePanel();
    }
    void SpawnSoldier()
    {
        Debug.Log("spawnSoldier called");
        GameObject newSoldier = Instantiate(goodSoldierPrefab, allGoodSoldiers);
        SoldierNav newSoldierNav = newSoldier.GetComponent<SoldierNav>();
        // newSoldier.transform.localPosition = Vector3.zero; //Buggy - For some reason it doesn't put the newSoldier object at 0,0,0 away from it's parent
        // newSoldier.transform.localPosition = referenceSoldier.localPosition;
        newSoldier.transform.position = new Vector3(52.33591f, 11.79359f, 169.7765f);
        newSoldier.GetComponent<NavMeshAgent>().Warp(referenceSoldier.position);
        newSoldierNav.SetDestination(soldiersDestination);
        newSoldierNav.SetRefToEnemies(enemySoldiers);
    }
    void SpawnMiner()
    {
        Debug.Log("spawn Miner called");
        GameObject newMiner = Instantiate(minerPrefab, allMiners);
        MinerNav newMinerNav = newMiner.GetComponent<MinerNav>();
        // newSoldier.transform.localPosition = Vector3.zero; //Buggy - For some reason it doesn't put the newSoldier object at 0,0,0 away from it's parent
        // newSoldier.transform.localPosition = referenceSoldier.localPosition;
        //newMiner.transform.position = new Vector3(52.33591f, 11.79359f, 169.7765f);
       // newMiner.GetComponent<NavMeshAgent>().Warp(referenceSoldier.position);
        newMiner.GetComponent<NavMeshAgent>().Warp(new Vector3(171.1f, 14.2f, 72.7f));

        //newMinerNav.SetDestination(soldiersDestination);
        //newMinerNav.SetRefToEnemies(enemySoldiers);
    }
    void RotatePanel()
    {
        rotating = true;
        Debug.Log("rotation: " + rotationPanel.localEulerAngles.x);
        //  startX = rotationPanel.rotation.eu
        //  Debug.Log("Coroutine started");
        //  bool doneRotating = false;
        //  while(!doneRotating)
        //   {
        //      rotationPanel.Rotate(10 * Time.deltaTime, 0, 0);
        //      if (rotationPanel.rotation.x == 0)
        //           doneRotating = true;
        //   }
    }
    public void UpdateSoldierCostText() //Helper function for training soldier
    {
        if (barracksSelectionPanel.Find("SelectionButtons").gameObject.activeInHierarchy == true)
        {
            barracksSelectionPanel.Find("SelectionButtons").Find("SelectSoldierButton").Find("CostText").GetComponent<Text>().text = soldierCost + " gold";
            Debug.Log("updated from selectionpanel");
        }
        if (barracksSelectionPanel.Find("SoldierPanel").gameObject.activeInHierarchy == true)
        {
            barracksSelectionPanel.Find("SoldierPanel").Find("TrainSoldierButton").Find("CostText").GetComponent<Text>().text = soldierCost + " gold";
            Debug.Log("updated from soldierpanel");
        }
    }
    public void UpdateUpgradeAttackTexts() //Helper function for upgrading attack
    {
        if (barracksSelectionPanel.Find("SoldierPanel").gameObject.activeInHierarchy == true)
        {
            //Update cost text
            barracksSelectionPanel.Find("SoldierPanel").Find("UpgradeAttackButton").Find("CostText").GetComponent<Text>().text = (int)(prefabHealthScript.damage* prefabHealthScript.upgradeAttackFactor) + " gold";
            //Update status text
            barracksSelectionPanel.Find("SoldierPanel").Find("AttackStatusText").GetComponent<Text>().text = "Attack: " + (int)(prefabHealthScript.damage);
        }
        //Upgrades should only affect new units?
    }
    public void UpdateUpgradeArmorTexts() //Helper function for upgrading armor
    {
        if (barracksSelectionPanel.Find("SoldierPanel").gameObject.activeInHierarchy == true)
        {
            //Update cost text
            barracksSelectionPanel.Find("SoldierPanel").Find("UpgradeArmorButton").Find("CostText").GetComponent<Text>().text = (int)(prefabHealthScript.armor * prefabHealthScript.upgradeArmorFactor) + " gold";
            //Update status text
            barracksSelectionPanel.Find("SoldierPanel").Find("ArmorStatusText").GetComponent<Text>().text = "Armor: " + (int)(prefabHealthScript.armor);
        }
    }
    public void UpgradeAttackOnPrefab() //Controller function for upgrading attack, is called by button
    {
        float cost = (int)(prefabHealthScript.damage * prefabHealthScript.upgradeAttackFactor);
        if (playerMines.GetComponent<PlayerMinesController>().GetSavedGold() >= cost)
        {
            prefabHealthScript.UpgradeDamage();
            UpdateUpgradeAttackTexts();
            playerMines.GetComponent<PlayerMinesController>().SpendGold(cost);
        }
        else
        {
            //Display a message that there is not enough gold to upgrade attack
        }

    }
    public void UpgradeArmorOnPrefab() //Controller function for upgrading armor, is called by button
    {
        float cost = (int)(prefabHealthScript.armor * prefabHealthScript.upgradeArmorFactor);
        if (playerMines.GetComponent<PlayerMinesController>().GetSavedGold() >= cost)
        {
            prefabHealthScript.UpgradeArmor();
            UpdateUpgradeArmorTexts();
            playerMines.GetComponent<PlayerMinesController>().SpendGold(cost);
        }
        else
        {
            //Display a message that there is not enough gold to upgrade attack
        }
    }
    void AddSoldierToQueueEnd()
    {
        if (queueEmpty)
        {
            //  barracksCanvasPanel.GetChild(0).GetChild(0).GetComponent<TimeOut>().enabled = true;
            queueTimerOn = true;
        }
        queueEmpty = false;
        if (!queueFull)
        {
            if (nextSpriteInQueue)
                lastSpriteAdded = nextSpriteInQueue;
            nextSpriteInQueue.GetComponent<Image>().sprite = soldierSprite;
           // nextSpriteInQueue.GetComponent<TimeOut>().enabled = true;
            currentPosInLevel++;
            if(currentPosInLevel == barracksQueuePanel.GetChild(currentLevelInQueue).childCount)
            {
                currentPosInLevel = 0;
                currentLevelInQueue++;
            }
            if(currentLevelInQueue == barracksQueuePanel.childCount)
            {
                queueFull = true;
            }
            else
            {
                nextSpriteInQueue = barracksQueuePanel.GetChild(currentLevelInQueue).GetChild(currentPosInLevel);
            }
        }

    }
    public void TurnOnSoldierPanel()
    {
        barracksSelectionPanel.Find("SoldierPanel").gameObject.SetActive(true);
        barracksSelectionPanel.Find("SelectionButtons").gameObject.SetActive(false);
    }
    public void TurnOnMinerPanel()
    {
        barracksSelectionPanel.Find("MinerPanel").gameObject.SetActive(true);
        barracksSelectionPanel.Find("SelectionButtons").gameObject.SetActive(false);
    }
    public void TurnOnUnitSelectionPanel()
    {
        foreach(Transform child in barracksSelectionPanel)
        {
            child.gameObject.SetActive(false);
        }
        barracksSelectionPanel.Find("SelectionButtons").gameObject.SetActive(true);
    }
    public void CheckIfEnoughGoldForPurchase(string unitName)
    {
        if (unitName == "soldier")
        {
            if (playerMines.GetComponent<PlayerMinesController>().GetSavedGold() > soldierCost)
            {
                if (!queueFull)
                {
                    AddSoldierToQueueEnd(); //generalize this function for miners
                    playerMines.GetComponent<PlayerMinesController>().SpendGold(soldierCost);
                    //Add in an iron cost
                }

            }
            else
            {
                //Send not enough gold message
            }
        }else if(unitName == "miner")
        {
            if(playerMines.GetComponent<PlayerMinesController>().GetSavedGold() > minerCost)
            {
                //Add unit to  queue end
                playerMines.GetComponent<PlayerMinesController>().SpendGold(minerCost);
                //Add in an iron cost
                SpawnMiner();
            }

        }
    }
    public void RemoveSoldierFromQueue()
    {
        
        Debug.Log("removeSoldierFrom queue called");
        if(currentLevelInQueue >=0 && currentPosInLevel >= 0)
        {
            nextSpriteInQueue = lastSpriteAdded;
            lastSpriteAdded.GetComponent<Image>().sprite = placeholderSprite;
            currentPosInLevel--;

            
            if (currentPosInLevel < 0)
            {
                currentLevelInQueue--;
                Debug.Log("current level is: " + currentLevelInQueue);
                Debug.Log("current pos is: " + currentPosInLevel);
                if(currentLevelInQueue < 0)
                {
                    currentLevelInQueue = 0;
                    currentPosInLevel = 0;
                    queueEmpty = true;
                }else
                {
                    currentPosInLevel = barracksQueuePanel.GetChild(currentLevelInQueue).childCount - 1;
                    Debug.Log("current pos should now be: " + currentPosInLevel);
                }
            }
            if (!queueEmpty)
                SpawnSoldier();

            

            //  lastSpriteAdded = barracksCanvasPanel.GetChild().GetChild(currentPosInLevel);
            int newLastPos, newLastLevel;
            if(nextSpriteInQueue.GetSiblingIndex() == 0)
            {
                if(nextSpriteInQueue.parent.GetSiblingIndex() > 0)
                {
                    newLastPos = nextSpriteInQueue.parent.childCount - 1;
                    newLastLevel = nextSpriteInQueue.parent.GetSiblingIndex()-1;
                }else
                {
                    lastSpriteAdded = nextSpriteInQueue;
                }

            }else
            {
                lastSpriteAdded = nextSpriteInQueue.parent.GetChild(nextSpriteInQueue.GetSiblingIndex() - 1);
            }
            Debug.Log("lastSpriteAdded pos: " + lastSpriteAdded.parent.childCount);

        }else
        {
            queueTimerOn = false;
            queueTimer = 0;
        }


    }
   
    void OnDestroy()
    {   //When the barracks is destroyed the game must already be over, so reset the prefab that this script
        //changes and already has a reference to.
        prefabHealthScript.ResetSoldierPrefabHealthStats();
    }
}
