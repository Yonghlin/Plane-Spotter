using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePlaneLines : MonoBehaviour
{
    public Button buttonShowLines;
    private bool showingLines = false;

    public bool DisplayingLines()
    {
        return showingLines;
    }

    private void ToggleLines()
    {
        showingLines = !showingLines;
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonShowLines.onClick.AddListener(ToggleLines);
    }

}
