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

    public bool proceduralMesh = true;
    [SerializeField, ConditionalField("proceduralMesh")]
    private float wrappingThickness = 0.02f;

    [SerializeField]
    private Vector3 wrapOffset = Vector3.zero;

    [SerializeField, ConditionalField("proceduralMesh", true)]
    private Vector3 wrapScaling = Vector3.one * 1.2f;

    [SerializeField, ConditionalField("proceduralMesh", true)]
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
        // creating the wrapping and setting as a child
        wrapping = new GameObject("Wrapping");
        wrapping.transform.SetParent(transform, false);
        wrapping.transform.localPosition = wrapOffset;

        //adding mesh rendering components
        wrappingMeshFilter = wrapping.AddComponent<MeshFilter>();
        wrappingRenderer = wrapping.AddComponent<MeshRenderer>();

        //setting the mesh to be same as parent but different material
        wrappingMeshFilter.mesh = !proceduralMesh ? preferredMesh : myMeshFilter.mesh;

        if (proceduralMesh) // creating a mesh based on expanding current mesh
            ExpandMesh();
        else
            wrapping.transform.localScale = wrapScaling;
        //wrappingRenderer.material = plastic;

        Unwrap();
    }

    private void ExpandMesh()
    {
        Vector3[] vertices = wrappingMeshFilter.mesh.vertices;
        Vector3[] normals = wrappingMeshFilter.mesh.normals;

        for (var i = 0; i < vertices.Length; i++)
        {
            // normalizing the vector
            normals[i] = vertices[i];
            normals[i].Normalize();
            // caculating new vec location
            vertices[i] += normals[i] * wrappingThickness;
        }

        // assign the local vertices array into the vertices array of the Mesh.
        wrappingMeshFilter.mesh.vertices = vertices;
        wrappingMeshFilter.mesh.RecalculateNormals();
        wrappingMeshFilter.mesh.RecalculateBounds();
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
