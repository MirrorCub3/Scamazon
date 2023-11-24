using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.VFX;

public class GasPowerMachine : BaseMachine
{
    [SerializeField]
    private VisualEffect visualEffect;

    private XRLever lever;

    private bool wasFueled;

    public static event Action MachineWasTurnedOff;

    public static GasPowerMachine gasPowerMachine;

    private GasCan gasCan; 

    private void Awake()
    {
        gasPowerMachine = this;
    }

    public void FuelMachine(bool fueled)
    {
        wasFueled = fueled;
        if (!wasFueled)
        { 
            TurnOffLever();
        }
    }

    public void StartMachine()
    { 
        visualEffect.Play();    
    }    

    public void StopMachine()
    {
        visualEffect.Stop();
        MachineWasTurnedOff?.Invoke();
    }

    public void TurnOffLever()
    {
        lever.SetValue(false);
    }

    public void TurnOnLever()
    {
        lever.SetValue(true);
    }

    public override void ExecuteMachine()
    {
        Debug.Log("Executing Power Machine. Yay!!");
    }
}
