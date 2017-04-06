using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinerNav : MonoBehaviour {
    Transform Destination;
    Transform castle;
    Transform hills;
    private NavMeshAgent navAgent;
    SoldierHealthManager healthScript;
   // Transform MineTarget;
    Transform lastHillVisited;
    Transform currentZone;
    Transform oreSphere;
    Transform minerCanvas;


    string destinationName;
    public float miningTimer;
   // public float searchingTimer;
    int indexOfMineSearch;
    public bool miningTimerOn;
    //public bool searchingTimerOn;
    bool reachedDestination;
    bool miningGold;
    public bool isAssignedToAMine = false;
    public Transform assignedMine;

    SimpleMineController assignedMineScript;
    PlayerMinesController minesManagerScript;
    // Use this for initialization
    void Start()
    {
        castle = GameObject.Find("Scaler").transform.Find("Village").Find("Castle");
        hills = GameObject.Find("Scaler").transform.Find("TerrainObjects").Find("Hills");
        minesManagerScript = GameObject.Find("Scaler").transform.Find("Mines").Find("PlayerMines").GetComponent<PlayerMinesController>();
        minerCanvas = transform.Find("Canvas");
        //  transform.position = new Vector3(10, 0, 10);
        navAgent = this.GetComponent<NavMeshAgent>();
        if (isAssignedToAMine)
        {
            AssignToMine(assignedMine);
        }else
        {
           
            minerCanvas.Find("ExplorePanel").gameObject.SetActive(true); //Await input
            if (hills.childCount > 0)
            {
                //Nothing needed?
            }else
            {
                Debug.Log("Error: miner nav cannot go to any hills because there are none listed");
            }
         /*   SetMineZone(hills.GetChild(0));
            if(hills.GetChild(0).childCount > 0)
            {
                SearchForNextMine();
            }*/

        }

        //   Destination = GameObject.Find("Scaler").transform.Find("Train");
        //navAgent.destination = Destination.position;
        healthScript = GetComponent<SoldierHealthManager>();
        miningTimer = 0;
        indexOfMineSearch = 0; 
        miningTimerOn = false;
        reachedDestination = false;
   }

    // Update is called once per frame
    void Update()
    {
        if (miningTimerOn)
        {
            miningTimer += Time.deltaTime;
            if(miningTimer >= 5)
            {
                miningTimer = 0;
                miningTimerOn = false;
                navAgent.Resume();
            }
        }
        /*if (searchingTimerOn)
        {
            searchingTimer += Time.deltaTime;
            if(searchingTimer >=5)
            {
                searchingTimer = 0;
                searchingTimerOn = false;
                nav
            }
        }
        */
    }
   
    public void AssignToMine(Transform mine)
    {
        assignedMine = mine;
        isAssignedToAMine = true;
        assignedMineScript = assignedMine.GetComponent<SimpleMineController>();
        if (mine.name == "IronMine")
        {
            oreSphere = transform.Find("Cart").Find("IronOreSphere");
            miningGold = false;
        }
        else if(mine.name == "GoldMine")
        {
            oreSphere = transform.Find("Cart").Find("GoldOreSphere");
            miningGold = true;
        }
        SetDestination(mine, mine.name);
    }
    public void SetMineZone(int zone)
    {
        currentZone = hills.GetChild(zone);
        indexOfMineSearch = 0;
        SearchForNextMine();
    }
    public void SearchForNextMine()
    {
        Debug.Log("searching for next mine! current zone is " + currentZone.name);
        if(currentZone.childCount > indexOfMineSearch)
        {
            indexOfMineSearch++;
            navAgent.SetDestination(currentZone.GetChild(indexOfMineSearch).position);
            destinationName = currentZone.GetChild(indexOfMineSearch).name;
        }
    }
    void OnTriggerEnter(Collider col)
    {
      //  Debug.Log(transform.name + " miner triggerEnter");
      //  Debug.Log(transform.name + ":  dest: " + destinationName + "/ " + col.name);
     //   Debug.Log(transform.name + " col: " + col.name + " colParentP: " + col.transform.parent.parent.name + "***************");
        if (col.name == destinationName)
        {
       //     Debug.Log(transform.name + ": miner reached destination: " + destinationName + "/ " + col.name);
            reachedDestination = true;
            navAgent.Stop();
            miningTimerOn = true;
            if (isAssignedToAMine)
            {
                if(destinationName == castle.name)
                {
                    KeepMiningAssignedMine(); //Changes the destination back to the mine
                }else
                {
                    
                    MinerReturnToCastle(); //Changes destination back to castle
                }
            }
            if (col.transform.parent.parent.name == "Hills")
            {
         //       Debug.Log(transform.name + " ran into a hillLLLLLLLLLLLLLLLLLLL^^^^^^^^^^^^^^^^^^^^");
                lastHillVisited = col.transform;
                if (col.name == destinationName)
                {
                    //Stop and wait while "mining"
                    navAgent.Stop();
                    miningTimerOn = true;
                    CheckSurroundingsForMine(col.transform);
                }
            }
            else
            {
              //  Debug.Log(transform.name + "NOT HILLSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
            }

          
            
        }

    }
    void CheckSurroundingsForMine(Transform hill)
    {
        Debug.Log("checking surroundings for mine inside " + hill.name);
        if(hill.tag == "HasMine")
        {
            Transform mineToStartWith = null;
            float radius;
            MeshRenderer hillMesh = hill.GetComponent<MeshRenderer>();
            if (hillMesh.bounds.extents.x > hillMesh.bounds.extents.y)
            {
                if(hillMesh.bounds.extents.x > hillMesh.bounds.extents.z)
                {
                    radius = hillMesh.bounds.extents.x;
                }else
                {
                    radius = hillMesh.bounds.extents.z;
                }
            }else if(hillMesh.bounds.extents.y > hillMesh.bounds.extents.z)
            {
                radius = hillMesh.bounds.extents.y;
            }else
            {
                radius = hillMesh.bounds.extents.z;
            }
            Collider[] hits = Physics.OverlapSphere(hill.position, radius);

            foreach(Collider item in hits)
            {
                if(item.gameObject.name == "GoldMine" || item.gameObject.name == "IronMine")
                {
                    item.transform.SetParent(transform.parent.parent.Find("PlayerMines"));
                    mineToStartWith = item.transform;
                }
            }
            if(mineToStartWith != null)
            {
                Debug.Log(transform.name + "found a mine! assigning to " + mineToStartWith.name + "MMMMMMMMMMMMMMMMMMMMMMMM");
                mineToStartWith.GetComponent<SimpleMineController>().TellMineWhichMiner(transform);
                mineToStartWith.GetComponent<SimpleMineController>().Display();

                //AssignToMine(mineToStartWith);
            }else
            {
                Debug.Log("Did not find a mine to start with on this pass of check");
            }
            if(hits.Length == 0)
            {
                SearchForNextMine(); //somewhat redundant since we know we should have a mine
            
            }
            hill.gameObject.SetActive(false);
        }else
        {
            Debug.Log(transform.name + "no mines, searching again");
            SearchForNextMine();
        }
        

    }
    void OnTriggerExit(Collider col)
    {
        if(col.name == "IronMine" || col.name == "GoldMine") //Miner has reached the mine and is heading back
        {
            oreSphere.GetComponent<MeshRenderer>().enabled = true; //Show the ore
        }
        if(col.name == castle.name) //Miner has delivered ore to the castle and is heading to the mine
        {
            //           Debug.Log("calling from " + transform.name);
            if (isAssignedToAMine)
            {
                oreSphere.GetComponent<MeshRenderer>().enabled = false; //Hide the ore
                //Increase the castle's gold saved by the amount of income from that mine
            
                minesManagerScript.CollectFromMiner(assignedMineScript.GetIncome(), miningGold); //miningGold is a bool that tells the function if the income is gold or iron

            }

        }
    }
    public void MinerReturnToCastle()
    {
        SetDestination(castle, "Castle");
    }
    public void KeepMiningAssignedMine()
    {
        SetDestination(assignedMine, assignedMine.name);
    }
    public void SetDestination(Transform dest, string destName)
    {
        // navAgent.SetDestination(new Vector3(1,1,1));
        //this.GetComponent<NavMeshAgent>().SetDestination(spot);
        reachedDestination = false;
        Destination = dest;
        destinationName = destName;
        navAgent.SetDestination(dest.position);
    }
    public bool IsReadyForNextDestination()
    {
        if (lastHillVisited.name == destinationName)
            return true;
        return false;
    }



}
