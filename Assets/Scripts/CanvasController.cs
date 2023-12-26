using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject title;
    public GameObject stage;
    bool onCanvas;


    public void ToggleObjects()
    {
        title.SetActive(!title.activeSelf);
        stage.SetActive(!stage.activeSelf);

    }

    //------------------------------------------------------------Test Code
    public void ToggleCanvas1()
    {
        if (onCanvas)
        {
            title.SetActive(false);
            stage.SetActive(false);
            onCanvas = false;
        }
        else
        {
            title.SetActive(true);
            stage.SetActive(true);
            onCanvas = true;
        }
    }
    //------------------------------------------------------------Test Code
}
