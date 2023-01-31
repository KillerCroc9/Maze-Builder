using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    public Transform target; // Point B
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Set the destination to point B
        agent.destination = target.position;

    }

    void FixedUpdate()
    {
        // Check if the AI has reached its destination
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            // Do something when the AI reaches its destination
            // For example, you can destroy the AI object or stop it from moving
            // Destroy(gameObject);
        }
    }
}
