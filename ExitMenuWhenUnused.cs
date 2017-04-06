using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Strongly considering moving this functionality over to the canvas object to help deal with accidental hovering over 
 * the menu. Also, create a small time delay before menu appears, like a second or so.
 */
public class ExitMenuWhenUnused : MonoBehaviour, IPointerExitHandler
{
    Vector3 initialRotation;
	// Use this for initialization
	void Start () {
        initialRotation = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.localEulerAngles = initialRotation;
    }
}
