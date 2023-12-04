using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GasCan : Item
{
    [SerializeField]
    private float timerLowerBound = 90f;
    [SerializeField]
    private float timerUpperBound = 150f;
    [SerializeField]
    private XRGrabInteractable interactable;
    
    
    private float timeToDeplete;

    private void Start()
    {
        ResetTimer();
        
    }

    public void ResetGasCan()
    {
        ResetTimer();
        RestartPosition();
        RemoveFromMachine();
    }

    private void ResetTimer()
    {
        timeToDeplete = Random.Range(timerLowerBound, timerUpperBound);
    }

    public void RemoveFromMachine()
    {
        Debug.Assert(GasPowerMachine.gasPowerMachine != null, "There is not a GasPowerMachine currently in used");
        GasPowerMachine.gasPowerMachine.FuelMachine(false);
        interactable.enabled = true;
    }

    public void AddedToMachine()
    {
        Debug.Assert(GasPowerMachine.gasPowerMachine != null, "There is not a GasPowerMachine currently in used");
        GasPowerMachine.gasPowerMachine.FuelMachine(true); 
        interactable.enabled = false;
    }

    public IEnumerator DepleteGasCan()
    { 
        yield return new WaitForSeconds(timeToDeplete);
        ResetGasCan();
    }

}
