using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    Transform claimedPanelRef; //heirarchy dependent
    Transform unclaimedPanelRef; //heirarchy dependent
    HUDController hudController;
    Text incomeText;
    Button incomeUpgradeButton;

    float income; //Should get this from parent instead of defining locally
    float maxIncome;
    // Use this for initialization
    void Start () {
        unclaimedPanelRef = transform.GetChild(0);
        if(transform.childCount > 1)
        {
            claimedPanelRef = transform.GetChild(1);
            claimedPanelRef.gameObject.SetActive(false);


        }
        unclaimedPanelRef.gameObject.SetActive(true);

        hudController = GameObject.Find("Canvas-HUD").transform.GetChild(0).GetComponent<HUDController>();

            incomeText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
            incomeUpgradeButton = transform.GetChild(1).GetChild(1).GetComponent<Button>();

            income = 1; //default amount
            maxIncome = 12; //default max
       //     hudController.UpdateGoldAmt(); //put default starting gold amt onto the hud

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("pointer entered panel");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("pointer left panel");

        transform.gameObject.SetActive(false);
    }
    public void SwitchPanels()
    {
     //   unclaimedPanelRef.gameObject.SetActive(!gameObject.activeInHierarchy);
    //    claimedPanelRef.gameObject.SetActive(!gameObject.activeInHierarchy);
        //can never unclaim a mine
        unclaimedPanelRef.gameObject.SetActive(false);
        if(transform.childCount > 1)
            claimedPanelRef.gameObject.SetActive(true);
    }
    public void IncreaseIncome()
    {
        if(income < maxIncome)
        {
            income++;
            incomeText.text = "+" + income;
      //      hudController.UpdateGoldAmt();
        }
        //If income is now maxed out, disable button. 
        if (income >= maxIncome)
        {
            incomeUpgradeButton.interactable = false;
        }
    }
}
