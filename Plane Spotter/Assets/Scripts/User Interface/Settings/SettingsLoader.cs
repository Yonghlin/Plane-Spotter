using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsLoader : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown dropdown;
    public GetUserSettings userSettings;

    // Start is called before the first frame update
    private void Update()
    {
        toggle.isOn = userSettings.getDebugMenuState();

        switch (userSettings.getScreenAppearance())
        {
            case "Light": dropdown.value = 0; break;
            case "Dark" : dropdown.value = 1; break;
        }
    }

}
