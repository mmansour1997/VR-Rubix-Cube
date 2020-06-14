using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepScore : MonoBehaviour
{

    GUIStyle style = new GUIStyle();
    public GameObject display;
    void OnGUI()
    {
        GUI.Box(new Rect(20, 70, 200, 300), "Time Score\n--------------------------------------------\n" + display.GetComponent<Display>().AllScoresString);
    }

}
