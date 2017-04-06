using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour {
    public static Transform refToPanel;
	// Use this for initialization
	void Start () {
        refToPanel = transform.GetChild(0).GetChild(0);
        refToPanel.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseDown()
    {
        Debug.Log("MouseDown on object");
    }
    void OnMouseOver()
    {
        Debug.Log("Detected mouse over on object");
        refToPanel.gameObject.SetActive(true);
    }
    void OnMouseExit()
    {
        Debug.Log("mouse exited object area");


       // refToPanel.gameObject.SetActive(false);
    }
}
