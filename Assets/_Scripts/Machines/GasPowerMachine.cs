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

    [SerializeField]
    private XRLever lever;

    private bool wasFueled;

    public static event Action MachineWasTurnedOff;

    public static GasPowerMachine gasPowerMachine;

    [SerializeField]
    private Rigidbody doorRB;

    [SerializeField]
    private GasCan gasCan; 

    private void Awake()
    {
        gasPowerMachine = this;
        StopMachine();
        TurnOnRBConstraints(true);
        gasCan.gameObject.SetActive(false);
    }

    private void TurnOnRBConstraints(bool turnOn)
    {
        if (turnOn)
            doorRB.constraints = RigidbodyConstraints.FreezeAll;
        else
            doorRB.constraints = RigidbodyConstraints.None;
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
        if (!wasFueled) return;
        print("STARTING");
        ToggleVisualEffect(true);
    }
    public void StopMachine()
    {
        print("STOPPING");
        ToggleVisualEffect(false);
        MachineWasTurnedOff?.Invoke();
    }

    public override void MoveSwapMachine(Transform target, float moveDuration)
    {
        StopMachine();
        TurnOnRBConstraints(true);
        gasCan.gameObject.SetActive(false);
        base.MoveSwapMachine(target, moveDuration);
    }

    public override void SwapMachine(float delaySwap = 0)
    {
        StopMachine();
        TurnOnRBConstraints(true);
        gasCan.gameObject.SetActive(false);
        base.SwapMachine(delaySwap);
    }


    public void ToggleVisualEffect(bool turnOn)
    {
        foreach (var effect in visualEffects)
        {
            if (turnOn)
            {
                print("Playing");
                effect.Play();
            
            } 
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
        TurnOnRBConstraints(false);
        gasCan.gameObject.SetActive(true);
    }
}
