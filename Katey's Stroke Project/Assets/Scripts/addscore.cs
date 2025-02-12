using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class addscore : MonoBehaviour
{
    public static float score = 0;
    public TextMesh scoreui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        scoreui.text = "Score " + score.ToString("0.00");
        if (score == 3)
        {
            SceneManager.LoadScene("winnerscreen");
        }

    }
        
   

        
    
}
