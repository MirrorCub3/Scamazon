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
        ToggleInteracionLayer(true);
    }

    public void ResetGasCan()
    {
        ToggleInteractableComponent(false);
        ResetTimer();
        RestartPosition();
        RemoveFromMachine();
        ToggleInteractableComponent(true);
    }

    private void ResetTimer()
    {
        timeToDeplete = Random.Range(timerLowerBound, timerUpperBound);
    }

    public void RemoveFromMachine()
    {
        Debug.Assert(GasPowerMachine.gasPowerMachine != null, "There is not a GasPowerMachine currently in used");
        GasPowerMachine.gasPowerMachine.FuelMachine(false);
    }

    public void AddedToMachine()
    {
        Debug.Assert(GasPowerMachine.gasPowerMachine != null, "There is not a GasPowerMachine currently in used");
        GasPowerMachine.gasPowerMachine.FuelMachine(true); 
    }

    public IEnumerator DepleteGasCan()
    { 
        yield return new WaitForSeconds(timeToDeplete);
        ResetGasCan();
    }

    public void ToggleInteracionLayer(bool on)
    {
        if (on)
            gameObject.layer = 0;
        else
            gameObject.layer = 1;
    }

    public void ToggleInteractableComponent(bool toggle)
    {
        interactable.enabled = toggle;
    }

}
