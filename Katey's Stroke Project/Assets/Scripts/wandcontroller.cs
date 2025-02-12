using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class wandcontroller : MonoBehaviour
{
    public SteamVR_Action_Boolean m_Trigger = null;
    public SteamVR_Action_Boolean m_Grip = null;
    public SteamVR_Action_Boolean m_Dpad = null;
    private SteamVR_Behaviour_Pose m_Pose = null;

    public bool trigger_press = false;
    private bool trigger_pressed = false;
    private bool trigger_released = false;
    public bool grip_press = false;
    private bool grip_pressed = false;
    private bool grip_released = false;
    public bool dpad_press = false;
    private bool dpad_pressed = false;

    public Text scoreUI;         // UI Text on handset
    public Text healthUI;       // UI Text on handset
    public static int score;    // Score which will have the same value on all instances of wandcontroller   
    public static int health;   // Health which will have the same value on all instances of wandcontroller
    public float strength = 5f; // This controls how far you can throw things
    public float projectilePower;       // Power that projectile will be shot at
    public Transform projectileModel;   // Projectile prefab to shoot out of handset

    // GameObjects being touched or being pickedup
    public GameObject touched;
    public GameObject pickedup;
    //public bool pickedupiskinematic = false;
    private RigidbodyConstraints rbconstraints = RigidbodyConstraints.None;
    private CollisionDetectionMode pickedupcmode;

    // List of last positions for throwing
    private List<Vector3> positions = new List<Vector3>();

    private bool stuck = false;

    //Variables for climbing
    public bool gripped = false;
    public bool released = false;

    private Vector3 ObjVelocity = new Vector3(0, 0, 0);

    //public SphereCollider sColliderOculus;
    //public SphereCollider sColliderWMR;
    private SphereCollider sCollider;

    private string devicename;
    // public GameObject wmrleftglove;

    // Start is called before the first frame update
    void Start()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();

        sCollider = this.GetComponent<SphereCollider>();

        var vr = SteamVR.instance;
        if (vr != null)
            devicename = vr.hmd_TrackingSystemName;

        // TrackedObj = GetComponent<SteamVR_TrackedObject>();
        score = 0;
        health = 100;

        Debug.Log(devicename);
        if (devicename != "oculus")
        {
            if (this.name == "Controller (left)")
            {
                GameObject leftglove = GameObject.Find("vr_glove_left");
                leftglove.transform.parent = this.transform;
            }
            if (this.name == "Controller (right)")
            {
                GameObject rightglove = GameObject.Find("vr_glove_right");
                rightglove.transform.parent = this.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for buttons and triggers being pressed or released
        trigger_press = m_Trigger.GetState(m_Pose.inputSource);
        trigger_pressed = m_Trigger.GetStateDown(m_Pose.inputSource);
        trigger_released = m_Trigger.GetStateUp(m_Pose.inputSource);
        grip_press = m_Grip.GetState(m_Pose.inputSource);
        grip_pressed = m_Grip.GetStateDown(m_Pose.inputSource);
        grip_released = m_Grip.GetStateUp(m_Pose.inputSource);
        dpad_press = m_Dpad.GetState(m_Pose.inputSource);
        dpad_pressed = m_Dpad.GetStateDown(m_Pose.inputSource);

        // Get the sphere collider from the current controller
        // SphereCollider sCollider = GetComponent<SphereCollider>();

        // If the controller is in an object and you press trigger then pick it up by setting it to be a child of the controller
        // Also set the object to be frozen or kinematic so that it isnt effected by bumping into things and cant leave your control
        if ((trigger_pressed || trigger_press) && touched != null && pickedup == null && touched.gameObject.tag != "grip")
        {
            pickedup = touched;

            Rigidbody pickedup_rb = pickedup.GetComponent<Rigidbody>();

            if (pickedup_rb != null)
            {
                //pickedupiskinematic = pickedup.GetComponent<Rigidbody>().isKinematic;
                rbconstraints = pickedup_rb.constraints;
                pickedupcmode = pickedup_rb.collisionDetectionMode;
                pickedup_rb.constraints = RigidbodyConstraints.FreezeAll;
                pickedup_rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

                //pickedup.GetComponent<Rigidbody>().isKinematic = true;

                if (pickedup.transform.parent != null && pickedup.transform.parent.gameObject.tag == "GameController")
                {
                    rbconstraints = pickedup.transform.parent.gameObject.GetComponent<wandcontroller>().rbconstraints;
                    //pickedupiskinematic = pickedup.transform.parent.gameObject.GetComponent<wandcontroller>().pickedupiskinematic;
                }
            }
            else
            {
                //pickedupiskinematic = false;
                rbconstraints = RigidbodyConstraints.None;
                pickedupcmode = CollisionDetectionMode.Discrete;
            }

            pickedup.transform.parent = this.transform;
        }

        // If the parent is no longer the current controller, set the pickedup value to null as the object has been passed to the other controller
        if (pickedup != null && this.transform != pickedup.transform.parent)
            pickedup = null;

        // If you release the trigger and you have picked something up then apply a force to it based on the last 10 positions of the controller
        if (trigger_released && pickedup != null && !stuck)
        {
            // Drop the object you are holding by stopping it from being a child of the controller
            pickedup.transform.parent = null;

            Rigidbody pickedup_rb = pickedup.GetComponent<Rigidbody>();

            // If the object you have picked up has a rigidbody then set Kinematic to false so gravity can take effect
            if (pickedup_rb != null)
            {
                //if (!pickedupiskinematic)
                //    pickedup.GetComponent<Rigidbody>().isKinematic = false;

                pickedup_rb.constraints = rbconstraints;
                pickedup_rb.collisionDetectionMode = pickedupcmode;

                // Add a force to the object you are releasing
                pickedup_rb.AddForce(ObjVelocity * strength, ForceMode.Impulse);
            }


            pickedup = null;
        }
        //Debug.Log(touched.transform.name);
        //if (touched.transform.name == "")
        {
             //movechair.power = 1;
        }

        // If you press grip and you are not holding something then stop the controller being a trigger so that you can push things
        if (trigger_press && pickedup == null)
            sCollider.isTrigger = false;
        // If you stop pressing grip then set the collider on the controller to be a trigger so you can pick things up
        if (trigger_released)
            sCollider.isTrigger = true;

        // If you are holding something and you press grip you can stick it to your controller
        //if (grip_pressed && pickedup != null)
        //    stuck = !stuck;

        // Spawn projectiles from the handsets if you press the dpad
        if (dpad_pressed && projectileModel != null)
        {
            // Spawn the projectile at the position and rotation of the handsets
            Transform rocket = Instantiate(projectileModel,
                new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            // Rotate the projectile 90 degrees on X before firing it
            rocket.transform.Rotate(new Vector3(1, 0, 0), 90);
            // Project/fire the projectile
            rocket.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 1) * projectilePower);
        }
        //Climbing code
        released = false;
        if (touched != null && touched.gameObject.tag == "grip")
        {
            if (trigger_press)
                gripped = true;
            else
            {
                if (trigger_released)
                    released = true;
            }
        }
        if (trigger_released)
            gripped = false;

        // Update the score UI if it has been assigned
        if (scoreUI != null)
            scoreUI.text = "Score " + score.ToString();
        else if (healthUI != null)
            healthUI.text = "Health " + health.ToString();

        //score+=1;
    }

    private void FixedUpdate()
    {
        // If you have picked something up then store the last 10 positions of the controller into a list
        // if (pickedup != null)
        //    {
        positions.Add(this.transform.position);

        if (positions.Count > 10)
        {
            positions.RemoveAt(0);
        }
        //  }

        if (positions.Count > 0)
        {
            float timetaken = Time.deltaTime * positions.Count;
            ObjVelocity = (positions[positions.Count - 1] - positions[0]) / timetaken;
        }
        else
        {
            ObjVelocity = new Vector3(0, 0, 0);
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Touched object");
        //  Debug.Log(collider.transform.root.gameObject.name);
        // If the controller enters an object and it is not static then store a reference to it in the variable pickup
        if (collider.gameObject.tag != "GameController" && collider.gameObject.tag != "static" && collider.transform.root.gameObject.tag != "static" && !collider.gameObject.isStatic && !collider.transform.root.gameObject.isStatic)
        {
            if (collider.transform.root.gameObject.tag == "GameController")
                if (this.gameObject.name == "Controller (left)")
                    touched = collider.transform.root.gameObject.transform.Find("Controller (right)").GetComponent<wandcontroller>().pickedup;
                else
                    touched = collider.transform.root.gameObject.transform.Find("Controller (left)").GetComponent<wandcontroller>().pickedup;
            else
            {
                touched = collider.transform.root.gameObject;
                Debug.Log(touched.name);
            }

        }


    }

    // If the controller leaves an object set the reference to null
    private void OnTriggerExit(Collider collider)
    {
        touched = null;
    }

    // Check for punching objects
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Punched object");
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {

            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            // Add a force to the object you are hitting
            rb.AddForce(ObjVelocity * strength, ForceMode.Impulse);
        }
    }



}