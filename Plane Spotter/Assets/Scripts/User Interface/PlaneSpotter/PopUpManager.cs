using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject airportPopup;
    public TMP_Text airportName;
    public TMP_Text airportCode;
    public TMP_Text airportElevation;
    public TMP_Text airportLongitude;
    public TMP_Text airportLatitude;
    public Toggle showTrajectoryLines;

    bool airportPopUpVisible = false;
    //bool airplanePopUpVisible = false;

    private void Start()
    {
        airportPopup.SetActive(false);
    }

    public void enableAirportPopup(GameObject airport)
    { 
        airportName.text = airport.GetComponent<Airport>().getName();
        airportCode.text = airport.GetComponent<Airport>().getCode();
        airportElevation.text = airport.GetComponent<Airport>().getElevation();
        airportLongitude.text = airport.GetComponent<Airport>().getLongitude();
        airportLatitude.text = airport.GetComponent<Airport>().getLatitude();
        showTrajectoryLines.isOn = airport.GetComponent<Airport>().isShowingTrajectoryLine();

        airportPopup.SetActive(true);
    }

    public void disableAirportPopup()
    {
        airportPopup.SetActive(false);
    }

}
