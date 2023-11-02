using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPool;
    [SerializeField] private GameObject objSpawnLocation;
    private const int ObjectAmount = 25;
    [SerializeField] private bool machineOn;
    private bool running;

    private List<GameObject> objectPool_offList;
    private List<GameObject> objectsToMove;
    private Vector3 spawnLocation;

    private const float maxTimeBetweenSpawn = 1f;
    private const float maxSpeed = -0.01f;

    private float timeBetweenSpawn;
    private float speed;
    private float velocitySP = 0f;
    private float velocityTM = 0f;
    private float smoothTime = 1f;

    void Start()
    {
        machineOn = false;
        running = false;
        speed = 0;
        objectsToMove = new List<GameObject>(ObjectAmount);
        objectPool_offList = new List<GameObject>(objectPool.Count);
        spawnLocation = objSpawnLocation.transform.position;

        foreach (GameObject o in objectPool)
            objectPool_offList.Add(o);
        
        StartCoroutine(spawnItem());
    }

    private void Update()
    {
        moveItems();
        if(!machineOn && speed != 0)
            speed = Mathf.SmoothDamp(speed, 0, ref velocitySP, smoothTime);
        if (machineOn && speed != maxSpeed) {
            if (!running) {
                timeBetweenSpawn = 2f;
                StartCoroutine(spawnItem());
            }
            speed = Mathf.SmoothDamp(speed, maxSpeed, ref velocitySP, smoothTime);
            timeBetweenSpawn = Mathf.SmoothDamp(timeBetweenSpawn, maxTimeBetweenSpawn, ref velocityTM, smoothTime);
        }

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
            running = true;
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        running = false;
    }

    private void moveItems()
    {
        foreach (GameObject o in objectsToMove)
            o.transform.position = o.transform.position + new Vector3(0, 0, speed);
    }
}
