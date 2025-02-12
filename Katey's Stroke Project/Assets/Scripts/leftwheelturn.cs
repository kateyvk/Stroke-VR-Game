using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftwheelturn : MonoBehaviour
{
    public float rotation = 0;
    public float pwr = 0;
    public GameObject chair;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotation += pwr;
        this.transform.localEulerAngles = new Vector3(rotation, chair.transform.localEulerAngles.y, chair.transform.localEulerAngles.z);
        this.transform.position = chair.transform.position + chair.transform.right * -0.35f + chair.transform.forward * -0.25f + chair.transform.up * -0.3f;
    }

    private void OnTriggerEnter(Collider other)
    //When the wand hits the right wheel the chair should move turn left
    {
        if (other.gameObject.tag == "GameController")
        {
            pwr = 4;
            movechair.leftwheel = 1; //sending a determined value to movechair script 
        }
    }
    private void OnTriggerExit(Collider other)
    //When the wheel doesn't detect a collison with the left wheel the chair doesn't move
    {
        if (other.gameObject.tag == "GameController")
        {
            pwr = 0;
            movechair.leftwheel = 0; //sending a determined value to movechair script
        }
    }

}
