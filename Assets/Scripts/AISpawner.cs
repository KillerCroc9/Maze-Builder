using UnityEngine;
using System.Collections;

public class AISpawner : MonoBehaviour
{
    public GameObject AIPrefab; // The AI prefab to spawn
    private float spawnInterval = .1f; // The interval at which to spawn the AI, in seconds

    void Start()
    {
        // Start spawning the AI at the specified interval
        InvokeRepeating("SpawnAI", .5f, spawnInterval);
    }

    void SpawnAI()
    {
        // Instantiate the AI prefab at the specified position
        Instantiate(AIPrefab, this.transform.position, Quaternion.identity);
    }
}
