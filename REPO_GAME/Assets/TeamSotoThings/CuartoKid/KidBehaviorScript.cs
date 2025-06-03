using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
//This script handles the kid puzzle. It needs te kid prefab and the player positions and point where the kid is going to be moving around
public class KidBehaviorScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private Transform[] kidPoints;
    [SerializeField] private Transform[] playersTransform;
    [SerializeField] private float stoppingDistance = 1.0f;
    [SerializeField] private float playerSafeDistance = 2.0f;
    [SerializeField] private bool debugMode;
    [SerializeField] private UnityEvent catchEvent;
    private int lastIndex = -1;
    private bool moving;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.SetDestination(RandomPoint());
        agent.stoppingDistance = stoppingDistance;
        moving = true;

        if (kidPoints == null || kidPoints.Length == 0)
            Debug.LogError("Kid points not assigned!");
        if (playersTransform == null || playersTransform.Length == 0)
            Debug.LogError("Player transforms not assigned!");
    }

    void Update()
    {
        if (agent.enabled && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            moving = false;
            agent.isStopped = true;
            agent.enabled = false;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero; 
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        if (debugMode) DebugBehavior();
        else GameBehavior();
        animator.SetBool("moving", moving);
    }


    //Behavior for normal gameplay
    private void GameBehavior()
    {
        var playersObstructing = new List<Vector3>();

        foreach (var transform in playersTransform)
        {
            if ((transform.position - gameObject.transform.position).magnitude < playerSafeDistance)
            {
                moving = true;
                agent.enabled = true;
                agent.isStopped = false;
                playersObstructing.Add(transform.position);
            }
        }

        if (playersObstructing.Count > 0)
        {
            //Debug.Log("Players too close");
            agent.SetDestination(FarestPointFromPlayer(playersObstructing));
        }
        else if (!moving && Random.value > 0.99f)
        {
            //Debug.Log("Well... I guess it's time to move");
            moving = true;
            agent.enabled = true;
            agent.isStopped = false;
            agent.SetDestination(RandomPoint());
        }
    }


    //Behavior to test strategies that we may use in the game
    private void DebugBehavior()
    {
        //Stops or continues pathing
        if (Input.GetKeyDown(KeyCode.I))
        {
            moving = !moving;
            agent.enabled = moving;
            if (moving) agent.SetDestination(RandomPoint());
        }
        //Finds new destination point
        if (Input.GetKeyDown(KeyCode.P) && agent.enabled)
        {
            moving = true;
            agent.isStopped = false;
            agent.SetDestination(RandomPoint());
        }
    }

    //Returns a random point of the available points, that is not the current one
    //It needs the points to be inserted into the kidPoints parameter in the editor
    //This points need to be inside the navmesh range 
    UnityEngine.Vector3 RandomPoint()
    {
        int size = kidPoints.Length;
        int newIndex;
        do
        {
            newIndex = Random.Range(0, kidPoints.Length);
        } while (newIndex == lastIndex ||
                (kidPoints[newIndex].position - transform.position).magnitude <= stoppingDistance);
        lastIndex = newIndex;
        return kidPoints[newIndex].position;
    }

    //Returns the farest point of the available points
    //It needs the points to be inserted into the kidPoints parameter in the editor
    //This points need to be inside the navmesh range 
    Vector3 FarestPointFromPlayer(List<Vector3> playersPositions)
    {
        foreach(var transform in kidPoints)
        {
            bool isFar = true;
            foreach(var p in playersPositions)
            {
                if((transform.position - p).magnitude < playerSafeDistance) isFar = false;
            }
            if (isFar)
            {
                //Debug.Log("Found another spot bucko");
                return transform.position;
            }
        }
        return kidPoints[Random.Range(0, kidPoints.Length)].position;
    }


    //If it detects the player collision, it dissapears
    //I used a unity event to make the objective logic, but if we need other solution, its okay to do so
    //The kid gameobject does need to have a rigidbody and a collider with no trigger
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            catchEvent.Invoke();
            gameObject.SetActive(false);
            //Debug.Log("Catched!");
        }
    }
}
