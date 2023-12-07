using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Navigation : MonoBehaviour
{
    [SerializeField] private bool playIntro = true;
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject monitor;
    
    private int pIndex;
    private NavMeshAgent agent;
    private bool moving;
    private bool forward;
    
    private VotingSystem votingSystem;

    //FMOD stuff:


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pIndex = 0;
        moving = false;
        forward = true;
        Count_Manager.bossAppears += moveForward;

        votingSystem = monitor.GetComponent<VotingSystem>();

        if (playIntro)
            StartCoroutine(startIntro());
    }

    private void Update()
    {
        if (moving) {
            if (!agent.pathPending) {
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {  // checking if boss is at the next location

                        if (pIndex >= 0 && pIndex < points.Length) {  // move to next point if not done
                            move(points[pIndex].position);

                        } else {
                            moving = false;
                            if (playIntro) {  // play intro if necessary
                                StartCoroutine(playingIntro());
                            }else if(forward) {  // start the voting if arrived to player
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
        if(!playIntro)
            Count_Manager.resetCount();
    }


    private IEnumerator startIntro()
    {
        yield return new WaitForSeconds(3f); // Wait for machines to rise
        moveForward();
    }

    private IEnumerator playingIntro()
    {
        // PLAY INTRO VOICE LINE
        print("the tutorial voices are being played");  // KANOA, remove this print statement when the voice line is working
        yield return new WaitForSeconds(36f); // change to be how long the boss should stay
        moveBackward();
        playIntro = false;
    }

}
