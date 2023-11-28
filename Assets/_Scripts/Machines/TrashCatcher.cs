using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCatcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Trash item = other.GetComponent<Trash>();

        if (!item) return;

        item.InTrash();

    }

}
