using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CastlePanelController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform castleRef;
    public Text goldSavedText;
    BarracksController barracksScriptRef;
    // Use this for initialization
    void Start()
    {
        goldSavedText.text = "Gold: " + GlobalVariables.goldSaved;
        barracksScriptRef = GameObject.Find("barracks").GetComponent<BarracksController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalVariables.goldSaved <= 0)
        {
            transform.Find("insetPanel").Find("Button").GetComponent<Button>().interactable = false;
            goldSavedText.text = "You Broke";
        }else if(GlobalVariables.goldSaved < barracksScriptRef.costOfSoldiers)
        {
            transform.Find("insetPanel").Find("Button").GetComponent<Button>().interactable = false;
            transform.Find("insetPanel").Find("Button").Find("Text").GetComponent<Text>().text = "Not enough gold to train soldier.";
        }
        else
        {
            transform.Find("insetPanel").Find("Button").GetComponent<Button>().interactable = true;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
     //   Debug.Log("pointer entered panel");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.gameObject.SetActive(false);
    }
    public void UpdateGoldText()
    {
        goldSavedText.text = "Gold: " + GlobalVariables.goldSaved;
    }

}

