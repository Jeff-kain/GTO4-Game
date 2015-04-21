using UnityEngine;
using System.Collections;

public class ButtonFunctions : MonoBehaviour {

    public GameObject StartScreen;
    public GameObject EndScreen;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Quit()
    {
        Application.Quit();
    }

    public void EnableGame()
    {
        Gamemanager.instance.Enabled = true;
        StartScreen.SetActive(false);
    }

    public void ReturnToMenu()
    {
        StartScreen.SetActive(true);
        EndScreen.SetActive(false);
    }
}
