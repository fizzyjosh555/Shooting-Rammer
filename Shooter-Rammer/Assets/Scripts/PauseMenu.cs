using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;
    private bool paused = false;
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PauseUI.SetActive(false);
        player.GetComponent<PlayerTankController>().paused = paused;
    }


    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            paused = !paused;

        }

        if (paused)
        {
            player.GetComponent<PlayerTankController>().paused = paused;
            PauseUI.SetActive(true);
            Time.timeScale = 0f;
        }

        if (!paused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        paused = false;
        player.GetComponent<PlayerTankController>().paused = paused;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
