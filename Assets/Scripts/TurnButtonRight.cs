using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButtonRight : MonoBehaviour
{
    public void OnPointerDown()
    {
        CarControls.instance.TurnRight = true;
    }

    public void OnPointerUp()
    {
        CarControls.instance.TurnRight = false;
    }
}
