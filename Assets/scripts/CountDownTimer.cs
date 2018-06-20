using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {

    float endGameTimer = 120;
    float minutes;
    float seconds;
    Text TimeText;
    int zero = 0;

    private void Start()
    {
        TimeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        endGameTimer -= Time.deltaTime;

        seconds = Mathf.FloorToInt(endGameTimer % 60);
        minutes = Mathf.Floor(endGameTimer / 60);

        if (seconds < 10)
        {
            TimeText.text = minutes + ":0" + seconds;
        }
        else
        {
            TimeText.text = minutes + ":" + seconds;
        }
        



	}
}
