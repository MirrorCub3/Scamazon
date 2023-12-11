using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : PooledItem
{
    [SerializeField] [Range(0f,1f)]
    private float para;
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;

    private bool soundOff;

    [Header("Box Variables")]

    [SerializeField]
    private float startScale = 1.5f;

    [SerializeField]
    private float endScale = 1.25f;

    [SerializeField]
    private GameObject openBox;

    [SerializeField]
    private GameObject closedBox;
    public bool IsPacked { get; private set; }

    public string itemName;

    private void Start()
    {
        Reset();
        transform.localScale = Vector3.one * startScale;
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
    }
    private void OnEnable()
    {
    StartCoroutine("SoundOff");
    }

    public void SetEndScale()
    {
        transform.localScale = Vector3.one * endScale;
    }

    private void Pack()
    {
        IsPacked = true;
        openBox.SetActive(false);
        closedBox.SetActive(true);
    }

    protected override void Reset()
    {
        IsPacked = false;
        openBox.SetActive(true);
        closedBox.SetActive(false);
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        PackableItem item = other.GetComponent<PackableItem>();
        if (item && item.IsWrapped) // only if the object is packable
        {
            itemName = item.name;
            Pack();
            item.RepoolObject();
        }
    }
        private void OnCollisionEnter(Collision other)
    {
        if (!soundOff) {
            para = other.relativeVelocity.magnitude / 7;
            para = para > 1f ? 1: para;
            instance.setParameterByName("Velocity",para);
            instance.setParameterByName("Pitch",Random.Range(0f,1.5f));
            instance.start();
        }
    }
    IEnumerator SoundOff()
    {
        soundOff = true;
        yield return new WaitForSeconds(0.3f);
        soundOff = false;
    }
}
