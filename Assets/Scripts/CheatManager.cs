using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public bool isCheating;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.C) && !isCheating)
        {
            isCheating = true;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.C) && isCheating)
        {
            isCheating = false;
        }
    }
}
