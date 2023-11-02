using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [Serializable]
    public class MachineInfo
    {
        public Transform UpPosition;
        public Transform DownPosition;
        public BaseMachine machine;
        public MachineType type;
        public float moveDuration;
        public bool hasMovedUp;
        public bool hasBeenSwapped;
        public BaseMachine swapMachine;
    }

    [SerializeField]
    private List<MachineInfo> machines;

    private Dictionary<MachineType, MachineInfo> machinesDict;

    private bool machinesHaveMovedUp = false;

    [Header("Testing Variables")]
    public bool testing;
    public float time;

    private void Awake()
    {
        foreach (MachineInfo machine in machines)
        {
            try
            {
                BaseMachine machineTemp =  Instantiate(machine.machine, machine.DownPosition);
                machinesDict.Add(machine.type, machine);
                machineTemp.SetUpMachine(machine.type, this);
            }

            catch
            {
                Debug.LogError($"Machine type, {machine.type} ,has already been added." +
                    $"you can only have one machine type at the same time");
            }
        }

        StartCoroutine(IMoveMachines(true));
    }

    public void MachineSwap(MachineType type)
    {
        MachineInfo info = machinesDict[type];
        info.machine.SwapMachine();
    }

    public void MoveMachineSwap(MachineType type, BaseMachine machine)
    { 
        MachineInfo info = machinesDict[type];
        BaseMachine new_machine = info.hasBeenSwapped ? info.machine : info.swapMachine;
        Transform target = info.hasMovedUp ? info.DownPosition : info.UpPosition;
        new_machine = Instantiate(new_machine, target);
        new_machine.SetUpMachine(type, this);
        new_machine.MoveSwapMachine(info.UpPosition, info.moveDuration);
        Destroy(machine);
    }

    public void MoveMachinesUp()
    {
        if (machinesHaveMovedUp)
        {
            Debug.LogWarning("Machines have moved up. Can't move them up again");
            return;
        }

        foreach (MachineType type in machinesDict.Keys)
            InternalMoveMachine(type, machinesDict[type].UpPosition, true);

        machinesHaveMovedUp = true;
    }

    public void MoveMachineDown(MachineType type)
    {
        if (!machinesDict[type].hasMovedUp)
        {
            Debug.LogWarning($"Machine, {type}, have moved down. Can't move them down again");
            return;
        }

        InternalMoveMachine(type, machinesDict[type].DownPosition, false);
    }

    public void MoveMachineUp(MachineType type)
    {
        if (machinesDict[type].hasMovedUp)
        {
            Debug.LogWarning($"Machine, {type}, have moved up. Can't move them up again");
            return;
        }

        InternalMoveMachine(type, machinesDict[type].UpPosition, true);

    }

    public void MoveMachinesDown()
    {
        if (!machinesHaveMovedUp)
        {
            Debug.LogWarning("Machines have moved down. Can't move them down again");
            return;
        }

        foreach (MachineType type in machinesDict.Keys)
            InternalMoveMachine(type, machinesDict[type].DownPosition, false);

        machinesHaveMovedUp = false;
    }

    public void InternalMoveMachine(MachineType type, Transform target, bool WasMovedUp)
    {
        MachineInfo info = machinesDict[type];
        info.machine.MoveMachine(target, info.moveDuration);
        info.hasMovedUp = WasMovedUp;
    }

    private IEnumerator IMoveMachines(bool moveUp)
    {
        yield return new WaitForSeconds(time);
        if (moveUp)
            MoveMachinesUp();
        else 
            MoveMachinesDown();
    }
}


public enum MachineType
{ 
    Packaging,
    Shipping,
    Power,
    WasteManagement,
    OtimizePackingSize
}