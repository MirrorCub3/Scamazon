using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.VFX;

public class GasPowerMachine : BaseMachine
{
    [SerializeField]
    private List<VisualEffect> visualEffects;

    private XRLever lever;

    private bool wasFueled;

    public static event Action MachineWasTurnedOff;

    public static GasPowerMachine gasPowerMachine;

    private GasCan gasCan; 

    private void Awake()
    {
        gasPowerMachine = this;
        StopMachine();
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
        ToggleVisualEffect(true);
    }

    public override void MoveSwapMachine(Transform target, float moveDuration)
    {
        StopMachine();
        base.MoveSwapMachine(target, moveDuration);
    }

    public override void SwapMachine(float delaySwap = 0)
    {
        StopMachine();
        base.SwapMachine(delaySwap);
    }

    public void StopMachine()
    {
        ToggleVisualEffect(false);
        MachineWasTurnedOff?.Invoke();
    }


    public void ToggleVisualEffect(bool turnOn)
    {
        foreach (var effect in visualEffects)
        {
            if (turnOn) effect.Play();
            else effect.Stop();     
        }
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
        StartMachine();
    }
}
