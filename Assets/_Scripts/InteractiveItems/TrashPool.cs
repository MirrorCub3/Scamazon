using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashPool : MonoBehaviour
{
    [Header("Object Pool")]
    [SerializeField]
    private List<GameObject> trashPool = new List<GameObject>();
    private Queue<GameObject> trashQueue = new Queue<GameObject>();

    [Header("Spawning")]
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField, Range(1, 10)]
    private int spawnCountLowerRange, spawnCountUpperRange;
    [SerializeField]
    private float spawnForce = 2f;
    [SerializeField]
    private float spawnRadius = .5f;
    [SerializeField]
    private float verticalModifier = 3f;
    private Dictionary<GameObject, Rigidbody> rbDict = new Dictionary<GameObject, Rigidbody>();

    [Header("Visuals")]
    [SerializeField]
    private Dictionary<GameObject, MeshRenderer> meshDict = new Dictionary<GameObject, MeshRenderer>();
    void Start()
    {
        foreach(GameObject trash in trashPool)
        {
            trash.transform.position = spawnPoint.position;
            trash.SetActive(false);
            trashQueue.Enqueue(trash);
            rbDict.Add(trash, trash.GetComponent<Rigidbody>());
            
            //MeshRenderer meshR = trash.GetComponent<MeshRenderer>();
            //meshR.material = mat;
            //meshDict.Add(trash, meshR);
        }
    }

    public void SpawnTrash()
    {
        int spawnCount = UnityEngine.Random.Range(spawnCountLowerRange, spawnCountUpperRange);
        if (trashQueue.Count < spawnCount)
            RefillTrashPool(spawnCount - trashQueue.Count);

        for(int i = 0; i < spawnCount; i++)
        {
            GameObject trash = trashQueue.Dequeue();
            trash.SetActive(true);
            if (rbDict.TryGetValue(trash, out Rigidbody rb))
            {
                rb.AddExplosionForce(spawnForce, spawnPoint.position, spawnRadius, verticalModifier);
            }
        }
    }

    private void RefillTrashPool(int neededItems)
    {
        for(int i = 0;i < neededItems;i++)
        {
            RequeueTrash(trashPool[i]);
        }
    }

    public void RequeueTrash(GameObject trash)
    {
        trashQueue.Enqueue(trash);
        trash.SetActive(false);
        trash.transform.position = spawnPoint.position;
    }
}
