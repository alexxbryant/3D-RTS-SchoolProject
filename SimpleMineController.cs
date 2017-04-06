using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMineController : MonoBehaviour {
    bool claimed;
    bool minerAssigned;
    float income;
    Transform canvas;
    Transform assignedMiner;

    // Use this for initialization
    void Start () {
        claimed = false;
        minerAssigned = false;
        canvas = transform.Find("Canvas");
        income = 5;
        UpdateCostToUpgradeText();
        if(transform.parent.name == "PlayerMines")
        {
            claimed = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseOver()
    {

    }
    void UpdateCostToUpgradeText()
    {
        canvas.Find("ClaimedPanel").Find("Text").GetComponent<Text>().text = "Cost: " + (1.5f * income);
    }
    public void Display()
    {
        transform.Find("Canvas").Find("UnclaimedPanel").gameObject.SetActive(true);
    }
    public void Claim()
    {
        claimed = true;
        transform.GetChild(0).gameObject.SetActive(true);
        canvas.Find("UnclaimedPanel").gameObject.SetActive(false);
        canvas.Find("ClaimedPanel").gameObject.SetActive(true);
        transform.SetParent(transform.parent.parent.Find("PlayerMines"));
    }
    public void Upgrade()
    {
        if(transform.name == "GoldMine")
        {
            if(transform.parent.GetComponent<PlayerMinesController>().GetSavedGold() >= income*1.5f) //Cost to upgrade income = 1.5 times income amount
            {
                income++;
                transform.parent.GetComponent<PlayerMinesController>().SpendGold(income * 1.5f);
                UpdateCostToUpgradeText();
            }
            else
            {
                //Display not enough gold message
                //canvas.Find("ClaimedPanel").Find("Text").GetComponent<Text>().text = "Not Enough Gold";
            }
        }else if(transform.name == "IronMine")
        {
            if (transform.parent.GetComponent<PlayerMinesController>().GetSavedIron() >= income * 1.5f) //Cost to upgrade income = 1.5 times income amount
            {
                income++;
                transform.parent.GetComponent<PlayerMinesController>().SpendIron(income * 1.5f);
                UpdateCostToUpgradeText();
            }
            else
            {
                //Display not enough gold message
                //canvas.Find("ClaimedPanel").Find("Text").GetComponent<Text>().text = "Not Enough Gold";
            }
        }

    }
    public float GetIncome()
    {
        return income;
    }
    public void TellMineWhichMiner(Transform miner)
    {
        assignedMiner = miner;
        minerAssigned = true;
    }
    public void MineCommandMinerToStart()
    {
        if (minerAssigned)
        {
            assignedMiner.GetComponent<MinerNav>().AssignToMine(transform);
        }else
        {
            Debug.Log("Error, attempting to get the miner to start mining a mine from SimpleMineController, but miner not assigned");
        }
    }
}
