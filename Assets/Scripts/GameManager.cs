using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Public Variables

    public GameObject winOject;
    public GameObject loseObject;
    public GameObject[] healthObject;

    public new AudioClip[] audio;

    public bool rotating;

    [System.NonSerialized]
    public bool started;

    #endregion

    #region Private Variables

    AudioSource source;

    #endregion

    private void Start()
    {
        rotating = false;
        started = false;
        StartCoroutine(StartGame());

        source = GetComponent<AudioSource>();
        source.clip = audio[2];
        source.loop = true;
        source.Play();

        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2.0f);
        started = true;
    }

    public void PlayerDeath()
    {
        loseObject.SetActive(true);
        foreach (GameObject go in healthObject) go.SetActive(false);

        source.clip = audio[1];
        source.loop = false;
        source.volume = 0.15f;
        source.Play();

        Time.timeScale = 0;
        StartCoroutine(GoToMain());
    }

    public void EnemyDeath()
    {
        winOject.SetActive(true);
        foreach (GameObject go in healthObject) go.SetActive(false);

        source.clip = audio[0];
        source.loop = false;
        source.volume = 0.15f;
        source.Play();

        Time.timeScale = 0;
        StartCoroutine(GoToCredits());
    }

    IEnumerator GoToMain()
    {
        yield return StartCoroutine(WaitForRealSeconds(3));
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator GoToCredits()
    {
        yield return StartCoroutine(WaitForRealSeconds(3));
        SceneManager.LoadScene("Credits");
    }

    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}
