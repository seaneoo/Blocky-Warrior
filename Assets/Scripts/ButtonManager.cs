using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartButton(string Game)
    {
        SceneManager.LoadScene(Game);
    }

    public void CreditButton(string Credits)
    {
        SceneManager.LoadScene(Credits);
    }

    public void BackButton(string MainMenu)
    {
        SceneManager.LoadScene(MainMenu);
    } 

    public void TutorialButton(string Tutorial)
    {
        SceneManager.LoadScene(Tutorial);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}