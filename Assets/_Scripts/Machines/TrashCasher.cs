using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCasher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Trash") return;

        Item item = other.GetComponent<Item>();

        if (item)
        { 
            //play poof but it is protected so I have to wait for approval
        }
        Destroy(other.gameObject);
    }

}
