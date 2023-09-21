using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject Play_CPU;
    public GameObject MODEs;
    public static int MONTE_NUMBER;
    public static bool PVP_mode;
    public static bool MODE_Tetris;
    public static bool MODE_Roids;

    public void OnTetrisToggle()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        if(MODE_Tetris)
            MODE_Tetris=false;
        else
            MODE_Tetris = true;

    }

    public void OnRoidsToggle()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        if(MODE_Roids)
            MODE_Roids=false;
        else
            MODE_Roids = true;
    }

    public void Start()
    {
        MONTE_NUMBER =0;
        PVP_mode = false;
        MODE_Tetris = false;
        MODE_Roids = false;
        FindObjectOfType<AudioManager>().Stop("gameplay_theme");
        FindObjectOfType<AudioManager>().Stop("lost");
        FindObjectOfType<AudioManager>().Stop("won");
        FindObjectOfType<AudioManager>().Play("menu_theme");
    }

    public void OnPickMODES()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        MainMenu.SetActive(false);
        MODEs.SetActive(true);
    }

    public void OnPlayPVP()
    {
        PVP_mode = true;
        MainMenu.SetActive(false);
        FindObjectOfType<AudioManager>().Play("UI_button");
        FindObjectOfType<AudioManager>().Stop("menu_theme");
        FindObjectOfType<AudioManager>().Play("gameplay_theme");
        SceneManager.LoadScene(1);
    }

    public void OnPlayCPU()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        MainMenu.SetActive(false);
        Play_CPU.SetActive(true);
    }

    public void OnReturn()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        MainMenu.SetActive(true);
        MODEs.SetActive(false);
        Play_CPU.SetActive(false);

    }

    public void OnSelectDifficulty(int number)
    {
        MONTE_NUMBER = number;
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Play("UI_button");
        FindObjectOfType<AudioManager>().Stop("menu_theme");
        FindObjectOfType<AudioManager>().Play("gameplay_theme");
    }
}
