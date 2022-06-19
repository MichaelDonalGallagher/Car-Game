using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControls : MonoBehaviour
{
    public static CarControls instance;
    public bool AccelerationOn = false;
    public bool TurnLeft = false;
    public bool TurnRight = false;
    public float maxSpeed = 5;
    public float accAmount = 1;
    public float turnSpeed = 50;
    private float currSpeed = 0;
    private bool rightNavComplete = false;
    private bool leftNavComplete = false;

    private void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (AccelerationOn)
        {
            currSpeed = Mathf.Clamp(currSpeed + accAmount * Time.deltaTime, 0, maxSpeed);
        }
        else if(currSpeed > 0)
        {
            currSpeed = Mathf.Clamp(currSpeed - accAmount * Time.deltaTime, 0, maxSpeed);
        }
        this.GetComponent<Rigidbody>().velocity = transform.forward * currSpeed;

        if (TurnLeft)
        {
            transform.Rotate(Vector3.up, -turnSpeed*Time.deltaTime);
        }
        else if (TurnRight)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }                
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RightNav")
        {
            rightNavComplete = true;
            Debug.Log("Right Nav Passed");
        }
        else if(other.tag == "LeftNav")
        {
            leftNavComplete = true;
            Debug.Log("Left Nav Passed");
        }
        else if(other.tag == "EndPoint" && leftNavComplete && rightNavComplete)
        {
            Debug.Log("Finish!");            
        }
    }
}
