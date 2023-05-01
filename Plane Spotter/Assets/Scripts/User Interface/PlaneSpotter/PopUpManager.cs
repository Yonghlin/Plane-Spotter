using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class PopUpManager : MonoBehaviour
{
    //Constants for loading full view information scenes
  

    //Labels for Airport Popup
    public GameObject airportPopup;
    public TMP_Text airportName;
    public TMP_Text airportCode;
    public TMP_Text airportElevation;
    public TMP_Text airportLongitude;
    public TMP_Text airportLatitude;
    public Toggle showTrajectoryLines;


    //Labels for Airplane Popup
    public GameObject airplanePopup;
    public TMP_Text airplaneName;
    public TMP_Text airplaneCode;
    public TMP_Text airplaneElevation;
    public TMP_Text airplaneLongitude;
    public TMP_Text airlaneLatitude;
    public TMP_Text destinationCity;
    public TMP_Text destinationName;
    

    private void Start()
    {
        airportPopup.SetActive(false);
        airplanePopup.SetActive(false);
    }

    public void enableAirportPopup(GameObject airport)
    { 
        airportName.text = airport.GetComponent<Airport>().getName();
        airportCode.text = airport.GetComponent<Airport>().getCode();
        airportElevation.text = airport.GetComponent<Airport>().getElevation();
        airportLongitude.text = airport.GetComponent<Airport>().getLongitude();
        airportLatitude.text = airport.GetComponent<Airport>().getLatitude();
        showTrajectoryLines.isOn = airport.GetComponent<Airport>().isShowingTrajectoryLine();
        saveCurrentAirport(airport.GetComponent<Airport>());

        airportPopup.SetActive(true);
    }

    public void disableAirportPopup()
    {
        airportPopup.SetActive(false);
    }

    public void enableAirplanePopup(GameObject airplane)
    {
        airplaneName.text = airplane.GetComponent<Airplane>().name;
        airplaneCode.text = airplane.GetComponent<Airplane>().Code;
        airplaneElevation.text = airplane.GetComponent<Airplane>().Elevation.ToString();
        airplaneLongitude.text = airplane.GetComponent<Airplane>().Longitude.ToString();
        airlaneLatitude.text = airplane.GetComponent<Airplane>().Latitude.ToString();
        destinationCity.text  = airplane.GetComponent<Airplane>().DestinationCity;
        destinationName.text = airplane.GetComponent<Airplane>().DestinationName;
        saveCurrentAirplane(airplane.GetComponent<Airplane>());

        airplanePopup.SetActive(true);
    }

    public void disableAirplanePopup()
    {
        airplanePopup.SetActive(false);
    }

    private void saveCurrentAirport(Airport data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("Airport", json);
    }

    private void saveCurrentAirplane(Airplane data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("Airplane", json);
    }
}
