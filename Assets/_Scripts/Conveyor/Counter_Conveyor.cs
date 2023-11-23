using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter_Conveyor : Conveyor
{
    public override void addOffObject(GameObject obj)
    {
        Box bx = obj.GetComponent<Box>();
        if(bx && bx.IsPacked && objectPool.Contains(obj) && !objectPool_offList.Contains(obj)) {
            Count_Manager.incrementCount();
        }
        base.addOffObject(obj);
    }
}
