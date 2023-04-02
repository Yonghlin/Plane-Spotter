using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUserSettings : MonoBehaviour
{
    public bool getDebugMenuState()
    {
        return (PlayerPrefs.GetString("DebugMenuState", "False") == "True");
    }

    public string getScreenAppearance()
    {
        return PlayerPrefs.GetString("ScreenAppearance", "Light");
    }
}
