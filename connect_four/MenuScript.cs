/*
This script controlls the main menu button functions.
*/
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
    public static bool MODE_Tetris;

    public void OnTetrisToggle()
    {
        if(MODE_Tetris)
            MODE_Tetris=false;
        else
            MODE_Tetris = true;

    }
    public void Start()
    {
        FindObjectOfType<AudioManager>().Stop("gameplay_theme");
        FindObjectOfType<AudioManager>().Play("menu_theme");
    }

    //use to select player vs player game mode
    public void OnPlayPVP()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        FindObjectOfType<AudioManager>().Stop("menu_theme");
        FindObjectOfType<AudioManager>().Play("gameplay_theme");
        PVP_mode = true;
        MainMenu.SetActive(false);
        SceneManager.LoadScene(1);
    }
    
    //use to enter difficulties selection menu
    public void OnPlayCPU()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        MainMenu.SetActive(false);
        Play_CPU.SetActive(true);
    }
    //use to return to the main menu
    public void OnReturn()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        MainMenu.SetActive(true);
        Play_CPU.SetActive(false);
    }
    //use to select a monte carlo number to change the difficulty of the game
    public void OnSelectDifficulty(int number)
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        FindObjectOfType<AudioManager>().Stop("menu_theme");
        FindObjectOfType<AudioManager>().Play("gameplay_theme");
        MONTE_NUMBER = number;
        SceneManager.LoadScene(1);
    }

    
}
