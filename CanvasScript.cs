using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour {
    Transform helpPanel;
    Transform mainPanel;
	// Use this for initialization
	void Start () {
        helpPanel = transform.Find("HelpPanel");
        mainPanel = transform.Find("MainPanel");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void StartGameButton()
    {
        SceneManager.LoadScene(0);
    }
    public void ShowHelpPanel()
    {
        mainPanel.gameObject.SetActive(false); //Can only access help panel from main panel, currently
        helpPanel.gameObject.SetActive(true);
    }
    public void ReturnToMainMenu() //Switch back to the main menu panel
    {
        foreach(Transform child in transform)
        {
            if (child.name != "MainPanel")
                child.gameObject.SetActive(false);
            else
                child.gameObject.SetActive(true);
        }
    }
}
