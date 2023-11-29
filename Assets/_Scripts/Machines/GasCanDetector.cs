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
            gasCan = other.gameObject.GetComponent<GasCan>();
            gasCan.AddedToMachine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gas")
        { 
            GasCan otherGasCan = other.gameObject.GetComponent<GasCan>();
            if (otherGasCan != gasCan)
                return;
            gasCan.RemoveFromMachine();
        }
        
    }
}
