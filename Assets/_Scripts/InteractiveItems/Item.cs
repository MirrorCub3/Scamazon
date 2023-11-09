using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class Item : MonoBehaviour
{
    [Header("World Interaction")]
    [SerializeField, Tooltip("Hold item from where you grabbed it")]
    private bool useDynamicAttach;

    [Header("Dropped Response")]

    [SerializeField, Tooltip("Should item react when dropped")]
    private bool reactToDrop;

    [SerializeField, ConditionalField("reactToDrop")]
    private GameObject poofPrefab;
    private GameObject poofParticle;
    private ParticleSystem poofParticleSystem;

    [SerializeField, ConditionalField("reactToDrop"), Tooltip("The amount of time before the item triggers on drop response")]
    private float dropTime = 5f;

    [SerializeField, ConditionalField("reactToDrop"), Tooltip("Should the item reappear somewhere")]
    private bool respawnOnDrop; // should the item respawn once dropped

    [SerializeField, ConditionalField("respawnOnDrop"), Tooltip("Where the item respawns to")]
    private Transform respawnPoint; // where to respawn if dropped

    private XRGrabInteractable myGrabInteractable;
    private Coroutine despawningCoroutine;

    private void Awake()
    {
        myGrabInteractable = GetComponent<XRGrabInteractable>();
        myGrabInteractable.useDynamicAttach = useDynamicAttach;

        if (reactToDrop && poofPrefab) // set up the poof particle
        {
            poofParticle = Instantiate(poofPrefab, transform.position, Quaternion.Euler(90, 0, 0));
            poofParticleSystem = poofParticle.GetComponent<ParticleSystem>();
            poofParticleSystem.Stop();
        }
    }

    protected virtual void OnDropped() // called when item has been on floor long enough
    {
        print("oh no, ive been forgotten on the floor D:");
    }

    protected void PlayPoof()
    {
        poofParticle.transform.position = transform.position;
        poofParticleSystem.Play();
    }

    private IEnumerator DropCountDown() // counts down for the drop response
    {
        yield return new WaitForSeconds(dropTime);

        if (poofParticle)
            PlayPoof();

        if (respawnOnDrop)
            transform.position = respawnPoint.position;

        OnDropped();
    }

    protected virtual void OnTriggerEnter(Collider other) // use for dropping items on the ground
    {
        if (other.tag.Equals("Despawn") && reactToDrop)
        {
            StopAllCoroutines();
            despawningCoroutine = StartCoroutine(DropCountDown());
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Despawn") && reactToDrop)
        {
            StopCoroutine(despawningCoroutine);
            print("you remembered me!");
        }
    }
}
