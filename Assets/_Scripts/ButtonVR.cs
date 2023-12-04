using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private UnityEvent onPress;
    [SerializeField] private UnityEvent onRelease;

    bool isPressed;
    GameObject presser;
    [SerializeField] private VotingSystem votingSystem;

    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.01f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.025f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    private void GoodOption()
    {
        if (votingSystem.votingReady == true)
        {
            Debug.Log("PLAYER PICKED GOOD OPTION");

            votingSystem.PlayerPickedGoodOption();
        }
    }

    private void BadOption()
    {
        if (votingSystem.votingReady == true)
        {
            Debug.Log("PLAYER PICKED BAD OPTION");

            votingSystem.PlayerPickedBadOption();
        }
    }

}
