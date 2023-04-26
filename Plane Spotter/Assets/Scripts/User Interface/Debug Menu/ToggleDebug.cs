using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleDebug : MonoBehaviour
{
    public Button toggleMenu;
    private bool isVisible = false;
   
    public List<TMP_Text> debugTexts;
    public List<GameObject> debugObjects;
    public GetUserSettings getUserSettings;

    // Start is called before the first frame update
    void Start()
    {
        toggleMenu.onClick.AddListener(toggleMenuVisibility);
        foreach (TMP_Text text in debugTexts)
        {
            text.enabled = false;
        }
        foreach (GameObject obj in debugObjects)
        {
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        toggleMenu.gameObject.SetActive(getUserSettings.getDebugMenuState());
    }

    void toggleMenuVisibility(){
        if(isVisible){
            foreach (TMP_Text text in debugTexts)
            {
                text.enabled = false;
            }
            foreach (GameObject obj in debugObjects)
            {
                obj.SetActive(false);
            }
        }
        else{
            foreach (TMP_Text text in debugTexts)
            {
                text.enabled = true;
            }
            foreach (GameObject obj in debugObjects)
            {
                obj.SetActive(true);
            }
        }
        isVisible = !isVisible;
    }

   
}
