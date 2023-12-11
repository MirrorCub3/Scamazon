using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackableItem : PooledItem
{
    [Header("FMOD Stuff")]
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;

    [Header("Pack Variables")]
    [SerializeField]
    private Wrapper myWrapper;
    public bool IsWrapped { get; private set;}

    // DO NOT CALL AWAKE IT WILL OVERLOAD BASE CLASS
    private void Start()
    {
        IsWrapped = false;
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag.Equals("Wrapping") && !IsWrapped) 
        {
            instance.start();
            IsWrapped = true;
            WrappingMaterial wrappingMat = other.GetComponent<WrappingMaterial>();
            myWrapper.Wrap(wrappingMat.GetWrappingMat());
            wrappingMat.SpawnTrash();
        }
    }
    protected override void Reset()
    {
        IsWrapped = false;
        myWrapper.Unwrap();
    }
}
