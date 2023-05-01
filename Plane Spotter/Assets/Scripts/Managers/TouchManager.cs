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
            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            var objsHit = Physics.RaycastAll(ray);
            if (objsHit.Length >= 1)
            {
                if (objsHit[0].collider.tag == "Airport")
                {
                    popup.enableAirportPopup(objsHit[0].collider.gameObject);
                }else if (objsHit[0].collider.tag == "Airplane")
                {
                    popup.enableAirplanePopup(objsHit[0].collider.gameObject);
                }
            }
        }
    }
}
