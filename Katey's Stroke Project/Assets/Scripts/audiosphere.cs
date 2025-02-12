using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiosphere : MonoBehaviour
{
    public AudioSource infosound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GameController")
        {
            infosound.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
