using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TurnOffMesh : MonoBehaviour
{
    private MeshRenderer myRenderer;
    private void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = false;
    }
}
