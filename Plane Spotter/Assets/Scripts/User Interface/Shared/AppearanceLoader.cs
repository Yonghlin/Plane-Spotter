using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearanceLoader : MonoBehaviour
{
    public List<GameObject> MenuItems;
    public GetUserSettings userSettings;
    int i;
    private void Update()
    {
        if (userSettings.getScreenAppearance() == "Light")
        {

            for (i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].GetComponent<Image>().color = Color.white;
            }

        }
        else if (userSettings.getScreenAppearance() == "Dark")
        {
            for (i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].GetComponent<Image>().color = Color.black;
            }

        }
    }

}
