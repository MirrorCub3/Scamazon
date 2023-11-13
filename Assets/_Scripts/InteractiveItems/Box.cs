using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : PooledItem
{
    [Header("Box Variables")]

    [SerializeField]
    private float startScale = 1.5f;

    [SerializeField]
    private GameObject openBox;

    [SerializeField]
    private GameObject closedBox;
    public bool IsPacked { get; private set; }

    private void Start()
    {
        Reset();
        transform.localScale = Vector3.one * startScale;
    }

    public void SetScale(float newScale = 1f)
    {
        transform.localScale = Vector3.one * newScale;
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

    private void OnTriggerEnter(Collider other)
    {
        PackableItem item = other.GetComponent<PackableItem>();
        if (item && item.IsWrapped) // only if the object is packable
        {
            Pack();
            item.RepoolObject();
        }
    }
}
