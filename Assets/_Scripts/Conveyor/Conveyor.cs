using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPool;
    [SerializeField] private GameObject objSpawnLocation;
    [SerializeField] private float timeBetweenSpawn = 1f; // Test number for now 
    private float speed = -0.01f; // Test number for now
    private const int ObjectAmount = 25;
    private List<GameObject> objectsToMove;
    private Vector3 spawnLocation;

    void Start()
    {
        objectsToMove = new List<GameObject>(ObjectAmount);
        spawnLocation = objSpawnLocation.transform.position;
    }

    
    void Update()
    {
        //spawnItem();
        moveItems();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("added");
        objectsToMove.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        objectsToMove.Remove(collision.gameObject);
    }

    public void startConveyor()
    {

    }
    public void stopConveyor()
    {

    }

    private IEnumerator spawnItem()
    {
        
        //Random rand = new Random();
        //int index = rand.Next(0, objectPool.Length);
        // move then activate objectPool[index]
        yield return new WaitForSeconds(timeBetweenSpawn);
    }

    private void moveItems()
    {
        foreach (GameObject o in objectsToMove)
            o.transform.position = o.transform.position + new Vector3(0, 0, speed);
    }
}
