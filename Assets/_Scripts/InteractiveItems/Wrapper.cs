using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Wrapper : MonoBehaviour
{
    //[Header("Materials")]
    //[SerializeField]
    //private Material plastic;

    //[SerializeField]
    //private Material paper;

    [Header("Wrapping")]
    [SerializeField]
    private float wrappingThickness = 1.5f;

    [SerializeField]
    private Mesh preferredMesh;

    private MeshFilter myMeshFilter;

    private MeshFilter wrappingMeshFilter;
    private MeshRenderer wrappingRenderer;

    private GameObject wrapping;

    private void Awake()
    {
        GenerateWrapping();
    }

    private void GenerateWrapping()
    {
        myMeshFilter = GetComponent<MeshFilter>();
        // creating hthe wrapping and setting as a child
        wrapping = new GameObject("Wrapping");
        wrapping.transform.SetParent(transform, false);
        wrapping.transform.localScale = Vector3.one * wrappingThickness; // setting the scale to be 2x the parent's

        //adding mesh rendering components
        wrappingMeshFilter = wrapping.AddComponent<MeshFilter>();
        wrappingRenderer = wrapping.AddComponent<MeshRenderer>();

        //setting the mesh to be same as parent but different material
        wrappingMeshFilter.mesh = preferredMesh != null ? preferredMesh : myMeshFilter.mesh;
        //wrappingRenderer.material = plastic;

        Unwrap();
    }

    public void Unwrap()
    {
        wrapping.SetActive(false);
    }

    public void Wrap(Material mat)
    {
        wrappingRenderer.material = mat;
        wrapping.SetActive(true);
    }

    //// tie this in with the events triggered by wrapping machine becoming active
    //public void SetPaperMat()
    //{
    //    wrappingRenderer.material = paper;
    //}

    //public void SetPlasticMat()
    //{
    //    wrappingRenderer.material = plastic;
    //}
}
