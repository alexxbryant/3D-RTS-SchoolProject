using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIRotate : MonoBehaviour, IPointerEnterHandler//, IPointerExitHandler
{
    bool rotateRestOfMenu;
    Transform partToRotate;
  //  Vector3 initialRotation;
	// Use this for initialization
	void Start () {
        rotateRestOfMenu = false;
        partToRotate = transform.parent.Find("RotationPanel");
      //  initialRotation = partToRotate.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        if (rotateRestOfMenu)
        {
            partToRotate.Rotate(350 * Time.deltaTime, 0, 0);
            if (partToRotate.localEulerAngles.x < 5 || partToRotate.localEulerAngles.x > 355)
            {
                rotateRestOfMenu = false;
            }

        }
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
//        Debug.Log("pointer over");
        if(partToRotate.GetChild(0).gameObject.activeInHierarchy == false)
        {
            rotateRestOfMenu = true;
            foreach(Transform child in partToRotate)
            {
                child.gameObject.SetActive(true);
            }
    //        Debug.Log("Rotation starting angle" + partToRotate.localEulerAngles);
        }

    }

    void OnMouseOver()
    {

    }
}
