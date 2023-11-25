using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trash : Item
{
    [Header("Trash Variables")]
    [SerializeField]
    private MeshRenderer myMatRenderer;
    [SerializeField]
    private TrashPool myPool;
    private bool matSet;

    private void Start()
    {
        myMatRenderer = GetComponent<MeshRenderer>();
        matSet = false;
    }
    
    public void InTrash()
    {
        OnDropped();
    }

    public void SetMaterial(Material mat)
    {
        myMatRenderer.material = mat;
    }

    protected override void OnDropped()
    {
        PlayPoof();
        myPool.RequeueTrash(this.gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag.Equals("Wrapping") && !matSet)
        {
            myMatRenderer.material = other.GetComponent<WrappingMaterial>().GetWrappingMat();
            matSet = true;
        }
    }

    private void OnDisable()
    {
        matSet = false;
    }

}
