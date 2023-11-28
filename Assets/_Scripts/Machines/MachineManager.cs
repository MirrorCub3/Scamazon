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
        public BaseMachine initialMachine;
        public BaseMachine swapMachine;
        public MachineType type;
        public float moveDuration;
        public bool hasMovedUp;
        public bool hasBeenSwapped;
        public BaseMachine activeMachine;
    }

    [SerializeField]
    private List<MachineInfo> machines;

    private Dictionary<MachineType, MachineInfo> machinesDict = new Dictionary<MachineType, MachineInfo>();

    private bool machinesHaveMovedUp = false;

    [Header("Testing Variables")]
    public bool testing;
    public float moveUpTime;
    public float executeTime;
    public float swapMachineTime;

    private void Awake()
    {
        foreach (MachineInfo machine in machines)
        {
            try
            {
                /*machine.activeMachine =  Instantiate(machine.machine, machine.DownPosition);
                machinesDict.Add(machine.type, machine);
                machine.activeMachine.SetUpMachine(machine.type, this);*/
                
                //Set active machine to the initial machine
                machine.activeMachine = machine.initialMachine;

                //Set initial Positions of all machines to down position
                machine.activeMachine.gameObject.transform.position = machine.DownPosition.position;
                if (machine.swapMachine)
                machine.swapMachine.transform.position = machine.DownPosition.position;
                
                //Set up ActiveMachine
                machine.activeMachine.SetUpMachine(machine.type, this);
                //Add to dictionary so it can be referenced by other scripts
                machinesDict.Add(machine.type, machine);
            }

            catch
            {
                Debug.LogError($"Machine type, {machine.type} ,has already been added. " +
                    $"You can only have one machine type at the same time");
            }
        }


        if (!testing) return;

        StartCoroutine(IMoveMachines(true));
        StartCoroutine(IExecuteMachines());
        //StartCoroutine(IMachineSwap());
        //StartCoroutine(IExecuteMachine(MachineType.Power, swapMachineTime + 5));
    }

    public void MachineSwap(MachineType type, float time = 0)
    {
        MachineInfo info = machinesDict[type];
        info.activeMachine.SwapMachine(time);
        info.hasBeenSwapped = true;
    }

    public void MoveMachineSwap(MachineType type)
    { 
        MachineInfo info = machinesDict[type];
        Transform target = info.hasMovedUp ? info.DownPosition : info.UpPosition;
        info.activeMachine.MoveSwapMachine(target, info.moveDuration);
        info.activeMachine = info.hasBeenSwapped ? info.initialMachine : info.swapMachine;
        info.activeMachine.gameObject.SetActive(true);


        //info.activeMachine = Instantiate(info.activeMachine, target);
        
        info.activeMachine.SetUpMachine(type, this);
        info.activeMachine.MoveMachine(info.UpPosition, info.moveDuration);
        info.hasBeenSwapped = true;
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
            Debug.LogWarning($"Machine, {type}, have moved down. Can't move it down again");
            return;
        }

        InternalMoveMachine(type, machinesDict[type].DownPosition, false);
    }

    public void ExecuteAllMachines()
    {
        foreach (MachineInfo info in machinesDict.Values)
            if (info.activeMachine != null)
                info.activeMachine.ExecuteMachine();
    }

    public void ExecuteMachine(MachineType type, float executeDelay = 0f)
    {
        StartCoroutine(IExecuteMachine(type, executeDelay));
    }

    public void MoveMachineUp(MachineType type)
    {
        if (machinesDict[type].hasMovedUp)
        {
            Debug.LogWarning($"Machine, {type}, have moved up. Can't move it up again");
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
        info.activeMachine.MoveMachine(target, info.moveDuration);
        info.hasMovedUp = WasMovedUp;
    }

    private IEnumerator IMoveMachines(bool moveUp)
    {
        yield return new WaitForSeconds(moveUpTime);
        if (moveUp)
            MoveMachinesUp();
        else 
            MoveMachinesDown();
    }

    //Testing Purposes Methods
    private IEnumerator IExecuteMachines()
    { 
        yield return new WaitForSeconds(executeTime);
        ExecuteAllMachines();       
    }

    private IEnumerator IExecuteMachine(MachineType type, float executeDelay = 0f)
    { 
        yield return new WaitForSeconds(executeDelay);
        machinesDict[type].activeMachine.ExecuteMachine();
    }


    private IEnumerator IMachineSwap()
    {
        yield return new WaitForSeconds(swapMachineTime);
        MoveMachineSwap(MachineType.Power);
    }
}


public enum MachineType
{ 
    Packaging,
    Power,
    WasteManagement,
    ItemConveyor,
    BoxConveyor
}