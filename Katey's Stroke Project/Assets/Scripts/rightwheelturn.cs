using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightwheelturn : MonoBehaviour
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
        // This code helps keep the wheel in line with the chair (the wheel is seperate so this makes sure they rotate the same way at the same time)
        rotation += pwr;
        this.transform.localEulerAngles = new Vector3(rotation, chair.transform.localEulerAngles.y, chair.transform.localEulerAngles.z); //Deals with roation of wheel
        this.transform.position = chair.transform.position + chair.transform.right * 0.35f + chair.transform.forward * -0.25f + chair.transform.up * -0.3f; //forces the wheels to not buckle and stay in place
        
    }

    private void OnTriggerEnter(Collider other)
        //When the wand hits the right wheel the chair should rotate and the wheel should spin
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "GameController")
        {
            //When the controller hits the wheel rotates and the chair turns 
            pwr = 4;
            movechair.rightwheel = 1;
        }
    }
    private void OnTriggerExit(Collider other)
        //
    {
        if (other.gameObject.tag == "GameController")
        {
            //If the controller isn't touching the wheel the wheel shouldn't rotate and chair shouldn't turn 
            pwr = 0;
            movechair.rightwheel = 0;
        }
    }

}
