using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown screenAppearance;

    public void setDebugMenuState(bool isEnabled)
    {
        PlayerPrefs.SetString("DebugMenuState", isEnabled.ToString());
    }

    public void setScreenAppearance()
    {
        if(screenAppearance.value == 0)
        {
            PlayerPrefs.SetString("ScreenAppearance", "Light");
        }
        else if(screenAppearance.value == 1)
        {
            PlayerPrefs.SetString("ScreenAppearance", "Dark");
        }
    }
}
