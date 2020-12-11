using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class coincoutscript : MonoBehaviour
{
    public GameObject player;
    public int cointoatal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        if (player.GetComponent<PlayerTankController>().score >= cointoatal)
        {
            win();
            
        }
        else if (player.GetComponent<PlayerHealth>().currentHealth <= 0)
        {
            player.GetComponent<PlayerTankController>().youWin.text = "You Lose";
            Invoke("levelreset", 5.0f);
        }
        else if(player.GetComponent<PlayerTankController>().score <= 0)
        {
            player.GetComponent<PlayerTankController>().youWin.text = "Score: " + player.GetComponent<PlayerTankController>().score.ToString();
        }
        else
        {

        }
    }

	public void win()
	{
		SceneManager.LoadScene(2);
	}

    public void countup()
    {
        player.GetComponent<PlayerTankController>().score += 1;
        player.GetComponent<PlayerTankController>().youWin.text = "Score: " + player.GetComponent<PlayerTankController>().score.ToString();
    }

    public void levelreset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
