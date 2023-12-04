using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Navigation : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject monitor;
    
    private int pIndex;
    private NavMeshAgent agent;
    private bool moving;
    private bool forward;
    
    private VotingSystem votingSystem;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pIndex = 0;
        moving = false;
        forward = true;
        Count_Manager.bossAppears += moveForward;

        votingSystem = monitor.GetComponent<VotingSystem>();
    }

    private void Update()
    {
        if (moving) {
            if (!agent.pathPending) {
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        if (pIndex >= 0 && pIndex < points.Length) {
                            move(points[pIndex].position);

                        } else {
                            moving = false;
                            if(forward) {
                                votingSystem.activateVoting = true;
                            }
                            forward = !forward;
                        }
                        xCrement();
                    }
                }
            }
        }

    }

    private void xCrement()
    {
        if(forward)
            pIndex++;
        else
            pIndex--;
    }


    private void move(Vector3 loc)
    {
        agent.SetDestination(loc);
    }

    private void moveForward()
    {
        pIndex = 0;
        moving = true;
        forward = true;
        gameObject.transform.rotation = points[0].rotation;
        gameObject.transform.position = points[0].position;
    }

    public void moveBackward()
    {
        pIndex = points.Length - 1;
        moving = true;
        forward = false;
        Count_Manager.resetCount();
    }

}
