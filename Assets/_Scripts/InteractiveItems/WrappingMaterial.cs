using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WrappingMaterial : MonoBehaviour
{
    [SerializeField]
    private Material wrappingMaterial;
    [SerializeField]
    private TrashPool trashPool;

    private void Awake()
    {
        if (wrappingMaterial == null)
            wrappingMaterial = GetComponent<MeshRenderer>().material;
    }

    public Material GetWrappingMat()
    { 
        return wrappingMaterial; 
    }

    public void SpawnTrash()
    {
        if(trashPool)
            trashPool.SpawnTrash();
    }
}
