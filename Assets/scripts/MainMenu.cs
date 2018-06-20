using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    bool isMainPressed = false;

    public void MainMenuJump()
    {
        isMainPressed = !isMainPressed;
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isMainPressed) SceneManager.LoadScene("mainMenu");
        else isMainPressed = false;
		
	}
}
