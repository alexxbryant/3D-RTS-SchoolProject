using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
   // Text messageToPlayer;
	// Use this for initialization
	void Start () {
       // messageToPlayer = transform.GetChild(0).GetComponent<Text>(); //Dependent on the graphical heiarchy in Unity

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ToggleMessage()
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeInHierarchy);
    }
    public void ToggleThis()
    {
        transform.gameObject.SetActive(!transform.gameObject.activeInHierarchy);
    }
    public void ChangeMessageTo(string message)
    {
       // messageToPlayer.text = message;
    }
    public void TurnOnSendSoldiersPanel()
    {
        Transform panel;
        if(panel = transform.Find("Panel - Ready Check"))
        {
            panel.gameObject.SetActive(true);
        }
        
        //transform.GetChild(0).gameObject.SetActive(true);
    }
}
