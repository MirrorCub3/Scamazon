using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExit : MonoBehaviour
{
    /*[SerializeField] private GameObject cv;
    private Conveyor conveyor;
    private void Start()
    {
        conveyor = cv.GetComponent<Conveyor>();
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        PooledItem PI = collision.gameObject.GetComponent<PooledItem>();
        if (PI)
            PI.RepoolObject();
        /*else
            PI = collision.gameObject.transform.parent.GetComponent<PooledItem>();
        if (PI)
            PI.RepoolObject();*/
    }
}
