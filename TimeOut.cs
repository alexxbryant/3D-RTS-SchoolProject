using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeOut : MonoBehaviour {
    float lifespanTimer;
    public float lifespan = 10;
    bool removedSprite = false;
    SimpleBarracks barracksScript;
    public Sprite placeholderSprite;
	// Use this for initialization
	void Start () {
        lifespanTimer = 0;
        barracksScript = transform.root.Find("CubeBarracks").GetComponent<SimpleBarracks>();
	}
	
	// Update is called once per frame
	void Update () {
        lifespanTimer += Time.deltaTime;
        if(lifespanTimer >= lifespan && !removedSprite)
        {
            Debug.Log("RevertingSprite");
            revertSprite();
            removedSprite = true;
        }
	}
    void revertSprite()
    {
        
        barracksScript.RemoveSoldierFromQueue();
        // transform.SetAsLastSibling();
        transform.SetSiblingIndex(transform.parent.childCount - 1);
        transform.GetComponent<Image>().sprite = placeholderSprite;
        GetComponent<TimeOut>().enabled = false;
    }
}
