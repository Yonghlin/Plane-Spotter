using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppearanceLoader : MonoBehaviour
{
    public List<GameObject> MenuItems;
    public List<TMP_Text> MenuText;

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

            for(i = 0; i < MenuText.Count; i++)
            {
                MenuText[i].color = Color.black;
            }

        }
        else if (userSettings.getScreenAppearance() == "Dark")
        {
            for (i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].GetComponent<Image>().color = Color.black;
            }
            for (i = 0; i < MenuText.Count; i++)
            {
                MenuText[i].color = Color.white;
            }

        }
    }

}
