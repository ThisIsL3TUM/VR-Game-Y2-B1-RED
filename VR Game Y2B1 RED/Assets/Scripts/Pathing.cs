using UnityEngine;
using UnityEngine.AI;

public class NPCRouting : MonoBehaviour
{
    public Transform[] waypoints;        // All waypoints along the path
    public int stopAtWaypointIndex = 2;  // Index where NPC waits for player
    public float stopDistance = 1f;      // Distance threshold for reaching a waypoint
    public Transform player;             // Reference to the player object

    private NavMeshAgent agent;
    private int currentWaypoint = 0;
    private bool waitingForPlayer = false;
    private bool playerInteracted = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to NPCPathSingle_FacePlayer!");
            enabled = false;
            return;
        }

        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void Update()
    {
        if (agent.pathPending) return;

        // Check if NPC reached current waypoint
        if (!waitingForPlayer && agent.remainingDistance < stopDistance)
        {
            // Stop and wait for player
            if (currentWaypoint == stopAtWaypointIndex && !playerInteracted)
            {
                agent.isStopped = true;
                waitingForPlayer = true;
                Debug.Log("NPC waiting for player interaction...");
            }
            else
            {
                MoveToNextWaypoint();
            }
        }

        // NPC faces player while waiting
        if (waitingForPlayer && player != null)
        {
            Vector3 lookDirection = (player.position - transform.position);
            lookDirection.y = 0; // Keep rotation flat
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
            }
        }

        // Wait for player input
        if (waitingForPlayer && Input.GetKeyDown(KeyCode.O))
        {
            playerInteracted = true;
            waitingForPlayer = false;
            agent.isStopped = false;
            MoveToNextWaypoint();
            Debug.Log("Player interacted. NPC continues on path.");
        }

        // Stop or despawn at end
        if (playerInteracted && currentWaypoint >= waypoints.Length)
        {
            agent.isStopped = true;
            // Optional: Destroy(gameObject, 2f);
        }
    }

    void MoveToNextWaypoint()
    {
        currentWaypoint++;

        if (currentWaypoint < waypoints.Length)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
        else
        {
            Debug.Log("NPC reached end of path.");
        }
    }
}