using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Canvas titleCanvas;
    public Canvas stageCanvas;
    
    public void ToggleCanvas()
    {
        titleCanvas.enabled = !titleCanvas.enabled;
        stageCanvas.enabled = !stageCanvas.enabled;
    }
}
