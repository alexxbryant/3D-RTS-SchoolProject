using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This script is where all the mines the player has discovered is managed
//Enable comments with "passive income" as label to turn passive income feature back on
//Currently, income only arrives when the miner physically returns to the castle
public class PlayerMinesController : MonoBehaviour {
    public float frequencyToCollectFromMines = 3;
    public Transform barracksStatusPanel;
    float savedGold;
    float savedIron;
    float collectionTimer;
	// Use this for initialization
	void Start () {
        savedGold = 50;
        savedIron = 10;
        collectionTimer = 0;
        UpdateStatusPanelGold();
        UpdateStatusPanelIron();

    }
	
	// Update is called once per frame
	void Update () {
    //PASSIVE INCOME START
   //     collectionTimer += Time.deltaTime;
   //     if(collectionTimer >= frequencyToCollectFromMines)
    //    {
            collectionTimer = 0;
    //        CollectFromMines();
    //    }
    //PASSIVE INCOME END
	}
    public void CollectFromMiner(float amount, bool isGold) //Only options are gold or iron, change bool if that changes
    {
 //       Debug.Log("Collecting from miner");
        if (isGold)
        {
            Debug.Log("adding " + amount + " gold");
            savedGold += amount;
            UpdateStatusPanelGold();
        }
        else
        {
            Debug.Log("adding " + amount + " iron");

            savedIron += amount;
            UpdateStatusPanelIron();
        }
    }
    void CollectFromMines() //Collects the income from all the mines the player owns
    {
        foreach(Transform child in transform)
        {
            if (child.name == "GoldMine")
            {
                savedGold += child.GetComponent<SimpleMineController>().GetIncome();
                UpdateStatusPanelGold();

            }else if(child.name == "IronMine")
            {
                savedIron += child.GetComponent<SimpleMineController>().GetIncome();
                UpdateStatusPanelIron();
            }
        }
    }
    void UpdateStatusPanelGold()
    {
        barracksStatusPanel.Find("GoldStatus").GetComponent<Text>().text = "Gold: " + savedGold;
    }
    void UpdateStatusPanelIron()
    {
        barracksStatusPanel.Find("IronStatus").GetComponent<Text>().text = "Iron: " + savedIron;
    }
    public float GetSavedGold()
    {
        return savedGold;
    }
    public float GetSavedIron()
    {
        return savedIron;
    }
    public void SpendGold(float amount)
    {
        savedGold -= amount;
        UpdateStatusPanelGold();
    }
    public void SpendIron(float amount)
    {
        savedIron -= amount;
        UpdateStatusPanelIron();
    }


}
