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

    // Start is called before the first frame update
    void Start()
    {
        toggleMenu.onClick.AddListener(toggleMenuVisibility);
        longitude.enabled = false;
        latitude.enabled = false;
        altitude.enabled = false;
        timesRun.enabled = false;
    }

    void toggleMenuVisibility(){
        if(isVisible){
            longitude.enabled = false;
            latitude.enabled = false;
            altitude.enabled = false;
            timesRun.enabled = false;
        }else{
            longitude.enabled = true;
            latitude.enabled = true;
            altitude.enabled = true;
            timesRun.enabled = true;
        }
        isVisible = !isVisible;
    }

   
}
