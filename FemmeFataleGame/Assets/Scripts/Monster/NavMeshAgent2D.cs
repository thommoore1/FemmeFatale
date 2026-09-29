using UnityEngine;
using UnityEngine.AI; // Required for NavMesh components

namespace FemmeFatale
{
    public class NavMeshAgent2D : MonoBehaviour
    {
        [SerializeField] private Transform targetDestination;
        private NavMeshAgent agent;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            // CRITICAL FOR 2D: Prevents the agent from tilting/flipping in 3D space
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        void Update()
        {
            if (targetDestination != null)
            {
                // Directs the agent to move toward the target position
                agent.SetDestination(targetDestination.position);
            }
        }
    }

}