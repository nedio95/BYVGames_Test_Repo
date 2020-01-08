using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
    public void ButtonClick(int num)
    {
        switch (num)
        {
            case 0:
                Application.Quit();
                break;
            case 1:
                SceneManager.LoadScene(num, LoadSceneMode.Single);
                break;
            case 2:
                SceneManager.LoadScene(num, LoadSceneMode.Single);
                break;
            default:
                break;
        }
    }	
}
