using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class KidBehaviorScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private Transform[] kidPoints;
    [SerializeField] private float stoppingDistance = 1.0f;
    [SerializeField] private bool debugMode;
    private UnityEvent catchEvent;
    private bool moving;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.destination = RandomPoint();
        agent.stoppingDistance = stoppingDistance;
        catchEvent = new UnityEvent();
        moving = true;
    }

    void Update()
    {
        if (agent.enabled && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            moving = false;
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (debugMode) DebugBehavior();
        animator.SetBool("moving", moving);
    }

    private void DebugBehavior()
    {
        //Stops or continues pathing
        if (Input.GetKeyDown(KeyCode.I))
        {
            moving = !moving;
            agent.enabled = moving;
            if (moving) agent.destination = RandomPoint();
        }
        //Finds new destination point
        if (Input.GetKeyDown(KeyCode.P) && agent.enabled)
        {
            moving = true;
            agent.isStopped = false;
            agent.destination = RandomPoint();
        }
    }

    //Returns a random point of the available points, that is not the current one
    Vector3 RandomPoint()
    {
        int size = kidPoints.Length;
        int randomPosition;
        while (true)
        {
            randomPosition = UnityEngine.Random.Range(0, size);
            if ((kidPoints[randomPosition].position - transform.position).magnitude > stoppingDistance) break;
        }
        return kidPoints[randomPosition].position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            catchEvent.Invoke();
            gameObject.SetActive(false);
            Debug.Log("Catched!");
        }
    }
}
