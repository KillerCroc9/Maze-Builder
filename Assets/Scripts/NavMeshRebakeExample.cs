/*using Unity.AI.Navigation;
*/using UnityEngine;
using UnityEngine.AI;

public class NavMeshRebakeExample : MonoBehaviour
{
  

    // The NavMeshSurface component attached to the game object
/*    private NavMeshSurface navMeshSurface;
*/    public GameObject Player;

    void Start()
    {
        // Get the NavMeshAgent and NavMeshSurface components
 
/*        navMeshSurface = GetComponent<NavMeshSurface>();
*/       // Invoke("Rebake", 2f);
       // Invoke("PlayerActivator", 3f);

    }


    void Rebake()
    {
/*        navMeshSurface.BuildNavMesh();
*/    }
    void PlayerActivator()
    {

        Player.SetActive(true);
    }
}