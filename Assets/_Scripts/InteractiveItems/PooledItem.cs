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
        Manager_Navigation.setTarget(null, transform.position);
    }

    public void RepoolObject()
    {
        myConveyor.addOffObject(gameObject);
        Reset();
    }

    protected virtual void Reset()
    {
        print("base class pooled item has nothing to reset");
    }

    // reset
    // set conveyor
    // repool object

    protected override void OnTriggerEnter(Collider other) // use for dropping items on the ground
    {
        if (other.tag.Equals("Despawn") && reactToDrop) {
            StopAllCoroutines();
            despawningCoroutine = StartCoroutine(DropCountDown());
            Manager_Navigation.setTarget(gameObject, transform.position);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Despawn") && reactToDrop) {
            Manager_Navigation.updateTargetLoc(gameObject, gameObject.transform.position);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Despawn") && reactToDrop) {
            StopCoroutine(despawningCoroutine);
            print("you remembered me!");
            Manager_Navigation.setTarget(null, transform.position);
        }
    }
}
