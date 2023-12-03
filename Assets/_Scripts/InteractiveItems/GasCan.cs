using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCan : Item
{
    [SerializeField]
    private float timerLowerBound = 90f;
    [SerializeField]
    private float timerUpperBound = 150f;

    private float timeToDeplete;

    private void Start()
    {
        ResetTimer();
    }

    public void ResetGasCan()
    {
        ResetTimer();
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

}
