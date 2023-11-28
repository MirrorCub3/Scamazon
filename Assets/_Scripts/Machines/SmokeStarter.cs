using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SmokeStarter : MonoBehaviour
{
    // this is for testin why are you using this
    [SerializeField]
    private VisualEffect smoke;

    private void Start()
    {
        smoke.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            smoke.Play();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            smoke.Stop();
        }
    }
}
