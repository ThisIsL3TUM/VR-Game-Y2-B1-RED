using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;

public class NPCRouting : MonoBehaviour
{
    public Transform[] waypoints;        // All waypoints along the path
    public int stopAtWaypointIndex = 2;  // Index where NPC waits for player
    public float stopDistance = 1f;      // Distance threshold for reaching a waypoint
    public Transform player;             // Reference to the player object
    public InputActionReference customButton; // For VR input (trigger, button, etc.)
    public float respawnDelay = 3f;      // Time before NPC respawns

    private NavMeshAgent agent;
    private int currentWaypoint = 0;
    private bool waitingForPlayer = false;
    private bool playerInteracted = false;
    private bool pressed = false;
    private Vector3 spawnPosition;       // Original spawn point

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to NPCRouting!");
            enabled = false;
            return;
        }

        // Remember where the NPC spawned
        spawnPosition = transform.position;

        // Start moving to the first waypoint
        agent.SetDestination(waypoints[currentWaypoint].position);

        // Subscribe to VR button press
        customButton.action.started += Drop;
    }

    void Update()
    {
        if (agent.pathPending) return;

        // Move between waypoints
        if (!waitingForPlayer && agent.remainingDistance < stopDistance)
        {
            if (currentWaypoint == stopAtWaypointIndex && !playerInteracted)
            {
                // Stop and wait for player
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
            lookDirection.y = 0;
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
            }
        }

        // Check for VR interaction
        if (waitingForPlayer && pressed)
        {
            playerInteracted = true;
            waitingForPlayer = false;
            pressed = false;
            agent.isStopped = false;
            MoveToNextWaypoint();
            Debug.Log("Player interacted. NPC continues on path.");
        }
    }

    void Drop(InputAction.CallbackContext context)
    {
        pressed = true;
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
            Debug.Log("NPC reached end of path. Despawning...");
            StartCoroutine(RespawnNPC());
        }
    }

    IEnumerator RespawnNPC()
    {
        // Disable NPC while despawning
        agent.isStopped = true;
        yield return new WaitForSeconds(respawnDelay);

        // Reset NPC state and teleport back to start
        transform.position = spawnPosition;
        currentWaypoint = 0;
        playerInteracted = false;
        waitingForPlayer = false;
        pressed = false;
        agent.isStopped = false;

        // Restart path
        agent.SetDestination(waypoints[currentWaypoint].position);

        Debug.Log("NPC respawned at start.");
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent input leak
        customButton.action.started -= Drop;
    }
}
