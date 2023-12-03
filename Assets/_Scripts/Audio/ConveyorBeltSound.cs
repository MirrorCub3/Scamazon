using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltSound : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;

    [SerializeField] [Range(0f,5f)]
    private float para;

    public bool on;
    [SerializeField] [Range(0f,10f)]
    private float speed;

    // Start is called before the first frame update
    public void switchClicked(bool _on)
    {
        on = _on;
    }
    void Start()
    {
        on = false;
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        instance.setParameterByName("ConveyorStart",para);
        para = Mathf.Lerp(para,(on ? 1f:0f)*5f,speed * Time.deltaTime * (para < 1f ? 3f: 1f));
    }
}
