using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public GameObject airportPopup;

    bool airportPopUpVisible = false;
    //bool airplanePopUpVisible = false;

    private void Start()
    {
        airportPopup.SetActive(false);
    }

    public void toggleAirportPopUPVisibility()
    {
        switch (airportPopUpVisible)
        {
            case false: airportPopUpVisible = true; break;
            case true: airportPopUpVisible = false; break;
        }
        airportPopup.SetActive(airportPopUpVisible);
    }

}
