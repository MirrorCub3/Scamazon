using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sip : MonoBehaviour
{
    //put this on the cup
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;
    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "sip")
        {
            instance.start();
        }
    }

}
