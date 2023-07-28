using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject Play_CPU;
    public static int MONTE_NUMBER;

    public void OnPlayCPU()
    {
        MainMenu.SetActive(false);
        Play_CPU.SetActive(true);
    }

    public void OnReturn()
    {
        MainMenu.SetActive(true);
        Play_CPU.SetActive(false);
    }

    public void OnSelectDifficulty(int number)
    {
        MONTE_NUMBER = number;
        SceneManager.LoadScene(1);
    }
}
