using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class TimerScript : MonoBehaviour
{
    public Text timerText;
    private float secondsCount;
    private int minuteCount;
    private int hourCount;

    private string hourString;
    private string minuteString;
    private string secondString;

    public bool CompletedFlag = false;
    public bool PauseFlag = false;

    public List<string> Scores = new List<string>();
    public GameObject bigCube;
    // Start is called before the first frame update
    void Start()
    {
        timerText.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            timerText.text = "";
        }
        else
        {
            UpdateTimerUI();
        }

    }

    public void UpdateTimerUI()
    {
        if (!PauseFlag && bigCube.GetComponent<InstCube>().canShuffle)
        {
            //set timer UI
            secondsCount += Time.deltaTime;

            if (secondsCount < 10)
                secondString = "0" + (int)secondsCount;
            else
                secondString = "" + (int)secondsCount;

            if (minuteCount < 10)
                minuteString = "0" + minuteCount;
            else
                minuteString = "" + minuteCount;

            if (hourCount < 10)
                hourString = "0" + hourCount;
            else
                hourString = "" + hourCount;

            timerText.text = hourString + ":" + minuteString + ":" + secondString + "";
            if (secondsCount >= 60)
            {
                minuteCount++;
                secondsCount = 0;
            }
            else if (minuteCount >= 60)
            {
                hourCount++;
                minuteCount = 0;
            }
        }



    

}
}
