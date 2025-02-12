using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class movechair : MonoBehaviour
{
    public float horsepower=1;
    public float maxturn;
    public float power = 0;
   // public static float rotation = 0;
    public float turnpower;
    private Rigidbody rb;
    public static float leftwheel = 0;
    public static float rightwheel = 0;
    public string horizontalinput = "Horizontal";
    public string verticalinput = "Vertical";
    public AudioSource badsound;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turnpower = 0;
    }

    // Update is called once per frame
    void Update()
    {
       // this.transform.localEulerAngles = new Vector3(0, 0, 0);
       if (leftwheel> 0 && rightwheel > 0)
        {
            power = horsepower;
        }

       else
        {
            power = 0;
            if (leftwheel > 0)
            {
                turnpower = maxturn;
            }
            else
            {
                turnpower = 0;       //Stops the wheels from continuslly turning
            }
            if (rightwheel > 0)
            {
                turnpower = -maxturn;
            }
        }
    }

    private void FixedUpdate()
    {
        
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y+turnpower, this.transform.eulerAngles.z); //Turning left and right
        rb.AddRelativeForce(Vector3.forward * power, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log("hit 2 " + other.gameObject.tag);

        if (other.gameObject.tag == "Boxes") //I tagged all the boxes and used this code so all boxes with the same tag will detect a collision and add points to the score
        {
            addscore.score += 1f;
        }
    }
    

    private void OnCollisionEnter(Collision collision)//All the cones are tagged so any object tagged "cones" that detects a hit will subtract from the score
    {

        if (collision.gameObject.tag == "Cones")
        {
            addscore.score -= 0.25f;
            badsound.Play();
        }
    }
}
