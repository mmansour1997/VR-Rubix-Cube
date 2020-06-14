using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject PauseFlag;
    public bool unpause = false;
    public GameObject bigcube;

    private void Update()
    {
      
        if (SceneManager.GetActiveScene().name == "Rubix Cube")
        {
            mainmenu.SetActive(true);
        }
        else
        {
            mainmenu.SetActive(false);
        }
        
    }

    public void Pausef()
    {
        if (unpause)
        {
            PauseFlag.GetComponent<TimerScript>().PauseFlag = false;
            unpause = false;
            bigcube.GetComponent<InstCube>().canRotate = true;
            bigcube.GetComponent<InstCube>().canShuffle = true;

        }
        else
        {
            PauseFlag.GetComponent<TimerScript>().PauseFlag = true;
            unpause = true;
            bigcube.GetComponent<InstCube>().canRotate = false;
            bigcube.GetComponent<InstCube>().canShuffle = false;
        }
    }
}
