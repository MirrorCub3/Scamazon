using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PooledItem PI = collision.gameObject.GetComponent<PooledItem>();
        if (PI)
            PI.RepoolObject();
    }
}
