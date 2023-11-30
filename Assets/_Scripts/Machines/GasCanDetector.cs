using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCanDetector : MonoBehaviour
{
    private GasCan gasCan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gas")
        {
            Debug.Log("IT IS INSIDE THE MACHINE");
            gasCan = other.gameObject.GetComponent<GasCan>();
            gasCan.AddedToMachine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gas")
        {
            Debug.Log("IT IS OUTSIDE THE MACHINE");
            GasCan otherGasCan = other.gameObject.GetComponent<GasCan>();
            if (otherGasCan != gasCan)
                return;
            gasCan.RemoveFromMachine();
        }
        
    }
}
