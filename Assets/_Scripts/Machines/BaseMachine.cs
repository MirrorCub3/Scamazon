using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMachine : MonoBehaviour
{
    [SerializeField]
    private ObjectMover objectMover;

    protected MachineManager machineManager;
    protected MachineType machineType;

    public abstract void ExecuteMachine();

    public virtual void SetUpMachine(MachineType type, MachineManager machineManager)
    { 
        machineType = type;
        this.machineManager = machineManager;
    }

    public virtual void MoveSwapMachine(Transform target, float moveDuration)
    {
        MoveMachine(target, moveDuration);
        StartCoroutine(WaitToDestroy(moveDuration));
    }

    public virtual void SwapMachine(float delaySwap = 0f)
    {
        StartCoroutine(WaitForChange(delaySwap));
    }

    public void MoveMachine(Transform target, float duration)
    { 
        objectMover.MoveObject(transform, target, duration);
    }

    private IEnumerator WaitForChange(float duration)
    { 
        yield return new WaitForSeconds(duration);
        ExecuteMachine();
    }

    private IEnumerator WaitToDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this);
    }
}
