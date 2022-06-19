using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButtonLeft : MonoBehaviour
{
    public void OnPointerDown()
    {
        CarControls.instance.TurnLeft = true;
    }

    public void OnPointerUp()
    {
        CarControls.instance.TurnLeft = false;
    }
}
