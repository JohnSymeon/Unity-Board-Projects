using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject Play_CPU;
    public static int MONTE_NUMBER;
    public static bool PVP_mode;

    //use to select player vs player game mode
    public void OnPlayPVP()
    {
        PVP_mode = true;
        MainMenu.SetActive(false);
        SceneManager.LoadScene(1);
    }
    
    //use to enter difficulties selection menu
    public void OnPlayCPU()
    {
        MainMenu.SetActive(false);
        Play_CPU.SetActive(true);
    }
    //use to return to the main menu
    public void OnReturn()
    {
        MainMenu.SetActive(true);
        Play_CPU.SetActive(false);
    }
    //use to select a monte carlo number to change the difficulty of the game
    public void OnSelectDifficulty(int number)
    {
        MONTE_NUMBER = number;
        SceneManager.LoadScene(1);
    }

    
}
