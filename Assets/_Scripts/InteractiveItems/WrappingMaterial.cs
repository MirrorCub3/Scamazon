using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WrappingMaterial : MonoBehaviour
{
    [SerializeField]
    private Material wrappingMaterial;

    private void Awake()
    {
        if (wrappingMaterial == null)
            wrappingMaterial = GetComponent<MeshRenderer>().material;
    }

    public Material GetWrappingMat()
    { 
        return wrappingMaterial; 
    }
}
