using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public GameObject scoreboard;
    bool active = false;

    public List<string> Scores = new List<string>();
    public string AllScoresString = "";
    public void display()
    {
        AllScoresString = "";
        for (int i = 0; i < Scores.Count; i++)
        {
            Debug.Log(Scores[i]);
            AllScoresString = AllScoresString + Scores[i] + "\n";
        }
        if (active)
        {
            scoreboard.SetActive(true);
            active = false;
        }
        else
        {
            scoreboard.SetActive(false);
            active = true;
        }

    }
}
