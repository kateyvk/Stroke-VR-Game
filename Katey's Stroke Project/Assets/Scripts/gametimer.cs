using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class gametimer : MonoBehaviour
{

    private float timer = 60.0f;   //Declare timer for a 60 second timer
    public TextMesh timerUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;  //Counting down 
      
        timerUI.text = "Time left: " + timer.ToString("0");  //Display timer value on screen

        if (timer <= 0)
        {
            timerUI.text = "YOU LOSE";
            SceneManager.LoadScene("loserscreen");
        }
    }
}
