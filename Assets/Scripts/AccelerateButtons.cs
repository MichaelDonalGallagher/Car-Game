using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateButtons : MonoBehaviour
{
    public void OnPointerDown()
    {
        CarControls.instance.AccelerationOn = true;
    }

    public void OnPointerUp()
    {
        CarControls.instance.AccelerationOn = false;
    }
}
