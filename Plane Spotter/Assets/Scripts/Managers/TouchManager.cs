using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TouchManager : MonoBehaviour
{
    private Camera arCamera;

    public PopUpManager popup;

    void Start()
    {
        arCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
                // Check if the touch is over a UI element
           /* if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }*/

            //            Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            var objsHit = Physics.RaycastAll(ray);
            if (objsHit.Length >= 1)
            {
                if (objsHit[0].collider.tag == "Airport")
                {
                    popup.enableAirportPopup(objsHit[0].collider.gameObject);
                }
            }

            /*if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is the airport
                if (hit.collider.tag == "Airport")
                {
                    // Do something with the airport
                    //hit.collider.gameObject.SetActive(false);
                    popup.enableAirportPopup(hit.collider.gameObject);
                    //Debug.Log("Airport touched!");
                }
            }*/
        }
    }
}
