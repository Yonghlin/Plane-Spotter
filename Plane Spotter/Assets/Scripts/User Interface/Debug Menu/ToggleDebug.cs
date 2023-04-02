using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleDebug : MonoBehaviour
{
    public Button toggleMenu;
    private bool isVisible = false;
    public TMP_Text longitude;
    public TMP_Text latitude;
    public TMP_Text altitude;
    public TMP_Text timesRun;
    public TMP_Text airportCount;
    public GetUserSettings getUserSettings;

    // Start is called before the first frame update
    void Start()
    {
        toggleMenu.onClick.AddListener(toggleMenuVisibility);
        longitude.enabled = false;
        latitude.enabled = false;
        altitude.enabled = false;
        timesRun.enabled = false;
        airportCount.enabled = false;
    }

    private void Update()
    {
        toggleMenu.gameObject.SetActive(getUserSettings.getDebugMenuState());
    }

    void toggleMenuVisibility(){
        if(isVisible){
            longitude.enabled = false;
            latitude.enabled = false;
            altitude.enabled = false;
            timesRun.enabled = false;
            airportCount.enabled = false;
        }else{
            longitude.enabled = true;
            latitude.enabled = true;
            altitude.enabled = true;
            timesRun.enabled = true;
            airportCount.enabled = true;
        }
        isVisible = !isVisible;
    }

   
}
