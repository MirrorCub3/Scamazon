using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private bool machineOn;
    
    [Header("Objects")]
    [SerializeField] private GameObject objSpawnLocation;
    [SerializeField] protected List<GameObject> objectPool;


    protected List<GameObject> objectPool_offList;
    private List<GameObject> objectsToMove;
    //private Vector3 spawnLocation;
    private int ObjectAmount;
    
    [Header("Current Settings [READ ONLY]")]
    [SerializeField] private float curMaxSpawnFreq;
    [Tooltip("Yes, this should be negative :)")]
    [SerializeField] private float curMaxSpeed;

    [Header("Mode Settings")]

    [Tooltip("On = slow conveyor speed\nOff = fast conveyor speed")]
    [SerializeField] private bool slowDown;

    [Space(10)]

    [Tooltip("Rate of spawn for fast mode")] [Range(0.15f, 3f)]
    [SerializeField] private float fastSpawnFreq = 0.75f; // Test value (WIP)
    [Tooltip("Speed of movement in fast mode")]
    [Range(-8f, 8f)]
    [SerializeField] private float fastSpeed = -3f; // Test Value

    [Space(10)]

    [Tooltip("Rate of spawn for slow mode")] [Range(0.15f, 3f)]
    [SerializeField] private float slowSpawnFreq = 1.5f; // Test value
    [Tooltip("Speed of movement in slow mode")] [Range(-8f, 8f)]
    [SerializeField] private float slowSpeed = -1.5f; // Test Value
    
    private bool running;
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

        speedUpConveyor();

        ObjectAmount = objectPool.Count;
        objectsToMove = new List<GameObject>(ObjectAmount);
        objectPool_offList = new List<GameObject>(objectPool.Count);
        //spawnLocation = objSpawnLocation.transform.position;

        foreach (GameObject o in objectPool) {
            objectPool_offList.Add(o);
            try {
                o.GetComponent<PooledItem>().SetConveyor(this);
            }
             catch {
                Debug.LogError(o.name + " in " + gameObject.transform.parent.name + "'s object pool does not have a PooledItem script (or subclass script).");
            }
                
        }
    }

    private void Update()
    {
        // Things are always "moving" but sometimes the speed is 0
        moveItems();

        // TESTING PURPOSES
        if (slowDown)
            slowDownConveyor();
        else
            speedUpConveyor();

        // If its off and things are moving, gradually bring it to a stop
        if(!machineOn && speed != 0)
            speed = Mathf.SmoothDamp(speed, 0, ref velocitySP, smoothTime);
        
        // If its on and its not at max speed, start spawning stuff if it's not already, gradually speed up the movement and spawnrate
        if (machineOn && speed != curMaxSpeed) {
            if (!running) {
                timeBetweenSpawn = 2f;
                StartCoroutine(spawnItem());
            }
            speed = Mathf.SmoothDamp(speed, curMaxSpeed, ref velocitySP, smoothTime);
            timeBetweenSpawn = Mathf.SmoothDamp(timeBetweenSpawn, curMaxSpawnFreq, ref velocityTM, smoothTime);
        }

    }

    // Set conveyor to slow speeds
    public void slowDownConveyor()
    {
        slowDown = true;
        curMaxSpeed = -slowSpeed;
        curMaxSpawnFreq = slowSpawnFreq;
    }

    // set conveyor to fast speeds
    public void speedUpConveyor()
    {
        curMaxSpeed = -fastSpeed;
        curMaxSpawnFreq = fastSpawnFreq;
    }

    // If obj is in the objectPool, disable it and add it to the offList
    public virtual void addOffObject(GameObject obj)
    {
        if (objectPool.Contains(obj) && !objectPool_offList.Contains(obj)) {
            obj.SetActive(false);
            objectPool_offList.Add(obj);
        }
    }

    // Move everything in contact with the conveyor
    private void OnCollisionEnter(Collision collision)
    {
        if(!objectsToMove.Contains(collision.gameObject))
            objectsToMove.Add(collision.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!objectsToMove.Contains(collision.gameObject))
            objectsToMove.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        objectsToMove.Remove(collision.gameObject);
    }

    // Sets machineOn boolean in a more reader friendly way
    public void startConveyor()
    {
        machineOn = true;
    }
    public void stopConveyor()
    {
        machineOn = false;
    }

    // Moves and activates objects based on the available options every few seconds
    private IEnumerator spawnItem()
    {
        while (machineOn) {
            if (objectPool_offList.Count > 0) {
                int index = Random.Range(0, objectPool_offList.Count);
                objectPool_offList[index].transform.position = objSpawnLocation.transform.position;
                objectPool_offList[index].transform.rotation = new Quaternion(0, 0, 0, 0);
                objectPool_offList[index].SetActive(true);
                objectPool_offList.RemoveAt(index);
            }
            running = true;
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        running = false;
    }

    // Moves all items in contact
    private void moveItems()
    {
        foreach (GameObject o in objectsToMove)
            o.transform.position = o.transform.position + new Vector3(0, 0, speed * Time.deltaTime);
    }
}
