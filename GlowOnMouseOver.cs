using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowOnMouseOver : MonoBehaviour {
    GameObject haloObejct1, haloObject2; //Must be child
    
	// Use this for initialization
	void Start () {
        haloObejct1 = transform.FindChild("GoldGlow").gameObject;
        haloObject2 = transform.FindChild("GoldGlow2").gameObject;
        haloObejct1.SetActive(false);
        haloObject2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseOver()
    {
        Debug.Log("MOUSE OVER");
        float timer = 0;
        float limit = 1;

        haloObejct1.SetActive(true);
        while(timer < limit)
            timer += Time.deltaTime;
        haloObejct1.SetActive(false);
        haloObject2.SetActive(true);
        timer = 0;
        while (timer < limit)
            timer += Time.deltaTime;
        haloObejct1.SetActive(true);
        timer = 0;
        while (timer < limit)
            timer += Time.deltaTime;
        haloObejct1.SetActive(false);
        timer = 0;
        while (timer < limit)
            timer += Time.deltaTime;
        haloObject2.SetActive(false);
    }
}
