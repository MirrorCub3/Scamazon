using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledItem : Item
{
    [Header("Pooled Object Variables")]
    [SerializeField]
    protected Conveyor myConveyor;

    public void SetConveyor(Conveyor conveyor)
    {
        myConveyor = conveyor;
    }

    protected override void OnDropped()
    {
        Reset();
        RepoolObject();
    }

    public void RepoolObject()
    {
        Reset();
        myConveyor.addOffObject(gameObject);
    }

    protected virtual void Reset()
    {
        print("base class pooled item has nothing to reset");
    }

    // reset
    // set conveyor
    // repool object
}
