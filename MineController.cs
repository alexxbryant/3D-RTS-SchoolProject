using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MineController : MonoBehaviour {
    public int rateOfIncome;
    public int maxIncome;
    public int costToClaim;
    public float secondsBetweenGoldPayout;
    Transform refToPanel;
    public Transform castlePanel;
    bool claimed;
    float timer;
    
   // GameObject mainPanel;

	// Use this for initialization
	void Start () {
        rateOfIncome = 1; //Starting rate of accumulation
        maxIncome = 12; //Limit the upgrading you can do to the mine
        claimed = false; //Mines must be claimed before use
                         //   mainPanel = transform.GetChild(0).GetChild(0).gameObject; //!!!Heirarchy dependent for now!!!
                         //    mainPanel.SetActive(false);
        refToPanel = transform.GetChild(0).GetChild(0);
        refToPanel.gameObject.SetActive(false);
        refToPanel.Find("Panel_Unclaimed").Find("Text").GetComponent<Text>().text = costToClaim + " gold to claim.";
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer >= secondsBetweenGoldPayout && claimed)
        {
            timer = 0;
            GlobalVariables.goldSaved += rateOfIncome;
            PayoutToCastle();
        }
        if (GlobalVariables.endOfRound)
        {
        }
	}
    void OnMouseOver()
    {
        Debug.Log("Detected mouse over on object");
        refToPanel.gameObject.SetActive(true);
    }
    public void Claim()
    {
        claimed = true;
        Debug.Log(gameObject.name + " has been claimed!");
        this.transform.SetParent(GameObject.Find("PlayerMines").transform);
        GlobalVariables.goldSaved -= costToClaim;
        castlePanel.GetComponent<CastlePanelController>().UpdateGoldText();
        refToPanel.Find("Panel_Claimed").Find("Text").GetComponent<Text>().text = "Cost: " + rateOfIncome*2 + " gold";
    }
    public void IncreaseIncome()
    {
        int costToIncrease = rateOfIncome * 2;
        if (rateOfIncome < maxIncome)
            rateOfIncome++;
        else
            Debug.Log("Max Income Reached");
        GlobalVariables.goldSaved -= costToIncrease;
        castlePanel.GetComponent<CastlePanelController>().UpdateGoldText();
        refToPanel.Find("Panel_Claimed").Find("Text").GetComponent<Text>().text = "Cost: " + costToIncrease + " gold";
    }
    void PayoutToCastle()
    {
        castlePanel.GetComponent<CastlePanelController>().UpdateGoldText();
    }
}
