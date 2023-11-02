using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPool;
    [SerializeField] private GameObject objSpawnLocation;
    [SerializeField] private float timeBetweenSpawn = 1f; // Test number for now 
    private float speed = -0.01f; // Test number for now
    private const int ObjectAmount = 25;
    [SerializeField] private bool machineOn;

    private List<GameObject> objectPool_offList;
    private List<GameObject> objectsToMove;
    private Vector3 spawnLocation;

    void Start()
    {
        machineOn = false;
        objectsToMove = new List<GameObject>(ObjectAmount);
        objectPool_offList = new List<GameObject>(objectPool.Count);
        spawnLocation = objSpawnLocation.transform.position;
        foreach (GameObject o in objectPool)
            objectPool_offList.Add(o);
        
        startConveyor();
        StartCoroutine(spawnItem());
    }

    private void Update()
    {
        moveItems();
    }

    public void addOffObject(GameObject go)
    {
        if (objectPool.Contains(go)) {
            go.SetActive(false);
            objectPool_offList.Add(go);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        objectsToMove.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        objectsToMove.Remove(collision.gameObject);
    }

    public void startConveyor()
    {
        machineOn = true;
    }
    public void stopConveyor()
    {
        machineOn = false;
    }

    private IEnumerator spawnItem()
    {
        while (machineOn) {
            if (objectPool_offList.Count > 0) {
                int index = Random.Range(0, objectPool_offList.Count);
                objectPool_offList[index].transform.position = spawnLocation;
                objectPool_offList[index].SetActive(true);
                objectPool_offList.RemoveAt(index);
            }
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    private void moveItems()
    {
        foreach (GameObject o in objectsToMove)
            o.transform.position = o.transform.position + new Vector3(0, 0, speed);
    }
}
