using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter_Conveyor : Conveyor
{
    public override void addOffObject(GameObject obj)
    {
        base.addOffObject(obj);
        Box bx = obj.GetComponent<Box>();
        if(bx && bx.IsPacked) {
            print("packed and counted");
            // TODO: implement and increment counter
        }
    }
}
