using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {
    public Transform Scaler;
    public Camera mainCamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PlayGame()
    {
        Scaler.gameObject.SetActive(true);
        transform.gameObject.SetActive(false);
       // mainCamera.transform.rotation.SetEulerAngles(new Vector3(55, mainCamera.transform.rotation.y, mainCamera.transform.rotation.z));
        mainCamera.transform.Rotate(new Vector3(55, mainCamera.transform.rotation.y, mainCamera.transform.rotation.z));
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
