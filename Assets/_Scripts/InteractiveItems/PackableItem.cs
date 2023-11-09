using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackableItem : Item
{
    [Header("Pack Variables")]
    [SerializeField]
    private Wrapper myWrapper;
    public bool IsWrapped { get; private set;}

    // DO NOT CALL AWAKE IT WILL OVERLOAD BASE CLASS
    private void Start()
    {
         IsWrapped = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag.Equals("Wrapping") && !IsWrapped) 
        {
            IsWrapped = true;
            myWrapper.Wrap(other.GetComponent<WrappingMaterial>().GetWrappingMat());
        }
    }

    private void Reset()
    {
        IsWrapped = false;
        myWrapper.Unwrap();
    }
}