using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExit : MonoBehaviour
{
    [SerializeField] private GameObject cv;
    private Conveyor conveyor;
    private void Start()
    {
        conveyor = cv.GetComponent<Conveyor>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        conveyor.addOffObject(collision.gameObject);
    }
}
