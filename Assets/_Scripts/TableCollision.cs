using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TableCollision : MonoBehaviour
{
    [HideInInspector]
    public bool coffeeOn;
    [HideInInspector]
    public bool penOn;

    [SerializeField] private GameObject coffee;
    [SerializeField] private GameObject pen;

    private void Start()
    {
        coffee = GameObject.Find("Coffee");
        pen = GameObject.Find("Pen");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == coffee)
        {
            coffeeOn = true;
            Debug.Log("coffee is on the table");
        }

        if (collision.gameObject == pen)
        {
            penOn = true;
            Debug.Log("pen is on the table");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (coffee.GetComponent<Rigidbody>().isKinematic == false)
        {
            if (collision.gameObject == coffee)
            {
                coffeeOn = false;
                Debug.Log("coffee is off the table");
            }
        }

        if (pen.GetComponent<Rigidbody>().isKinematic == false)
        {
            if (collision.gameObject == pen)
            {
                penOn = false;
                Debug.Log("pen is off the table");
            }
        }
    }
}
