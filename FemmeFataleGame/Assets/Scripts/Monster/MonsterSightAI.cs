using System;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

//TODO: Clean up code
//TODO: Understand code
//TODO: Figure out why JSON file reading doesn't work at runtime
//TODO: Refactor to use ScriptableObject for config instead of JSON file???
//TODO: Refactor to use a state machine instead of switch statements???
//TODO: Consistent naming conventions (camelCase vs PascalCase, etc)
//TODO: Why doesn't reload config work

namespace FemmeFatale
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class MonsterAI : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform player;

        [SerializeField] private Transform[] patrolPoints;

        [Tooltip("Layers that block line of sight (walls, etc). Do NOT include the player or monster layers.")]
        [SerializeField]
        private LayerMask obstacleMask;

        [Header("Behaviour")]
        [Tooltip(
            "Should be larger than the NavMeshAgent's Stopping Distance, or the monster may never get close enough to catch.")]
        [SerializeField]
        private float catchDistance = 0.6f;

        [Tooltip("Extra slack when deciding the monster has arrived at a destination.")] [SerializeField]
        private float arrivalTolerance = 0.1f;

        [Tooltip("How far from the player's last position we'll look for a valid NavMesh point.")] [SerializeField]
        private float navMeshSampleDistance = 2f;

        [Tooltip("File inside Assets/StreamingAssets")] [SerializeField]
        private string configFileName = "MonsterConfig.json";

        [Header("Catch Events")] [Tooltip("Hook up methods in the Inspector to run when the player is caught.")]
        public UnityEvent onPlayerCaught;

        // Subscribe from code: monster.PlayerCaught += MyMethod;</summary>
        public event Action PlayerCaught;

        public MonsterState CurrentState { get; private set; } = MonsterState.Patrol;

        private NavMeshAgent agent;
        private MonsterConfig config = new MonsterConfig();
        private Vector2 facing = Vector2.right;
        private Vector2 lastKnownPlayerPosition;
        private int patrolIndex;
        private bool patrolDestinationSet;
        private float searchTimer;
        private bool hasCaughtPlayer;
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            LoadConfig();
        }

        private void Update()
        {
            if (hasCaughtPlayer || player == null) return;

            // Track facing from actual movement so the vision cone points where the monster is going.
            Vector2 velocity = agent.velocity;
            if (velocity.sqrMagnitude > 0.01f)
            {
                facing = velocity.normalized;
            }

            bool canSeePlayer = CanSeePlayer();

            switch (CurrentState)
            {
                case MonsterState.Patrol:
                    UpdatePatrol(canSeePlayer);
                    break;
                case MonsterState.Chase:
                    UpdateChase(canSeePlayer);
                    break;
                case MonsterState.Search:
                    UpdateSearch(canSeePlayer);
                    break;
            }
        }

        private void UpdatePatrol(bool canSeePlayer)
        {
            if (canSeePlayer)
            {
                SpotPlayer();
                return;
            }

            if (patrolPoints == null || patrolPoints.Length == 0) return;

            if (!patrolDestinationSet)
            {
                SetDestinationOnNavMesh(patrolPoints[patrolIndex].position);
                patrolDestinationSet = true;
            }
            else if (HasReachedDestination())
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                patrolDestinationSet = false;
            }
        }

        private void UpdateChase(bool canSeePlayer)
        {
            if (!canSeePlayer)
            {
                ChangeState(MonsterState.Search);
                return;
            }

            lastKnownPlayerPosition = player.position;
            SetDestinationOnNavMesh(lastKnownPlayerPosition);

            if (Vector2.Distance(transform.position, player.position) <= catchDistance)
            {
                CatchPlayer();
            }
        }

        private void UpdateSearch(bool canSeePlayer)
        {
            if (canSeePlayer)
            {
                SpotPlayer();
                return;
            }

            if (!HasReachedDestination()) return;
            
            searchTimer += Time.deltaTime;
            if (searchTimer >= config.searchDuration)
            {
                ChangeState(MonsterState.Patrol);
            }
        }

        private void SpotPlayer()
        {
            lastKnownPlayerPosition = player.position;
            ChangeState(MonsterState.Chase);
        }

        private void ChangeState(MonsterState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            searchTimer = 0f;
            
            switch (newState)
            {
                case MonsterState.Patrol:
                    agent.speed = config.patrolSpeed;
                    patrolDestinationSet = false;
                    break;

                case MonsterState.Chase:
                    agent.speed = config.chaseSpeed;
                    patrolDestinationSet = false;
                    break;

                case MonsterState.Search:
                    agent.speed = config.chaseSpeed;
                    SetDestinationOnNavMesh(lastKnownPlayerPosition);
                    break;
            }
        }
        

        private bool CanSeePlayer()
        {
            Vector2 origin = transform.position;
            Vector2 toPlayer = (Vector2)player.position - origin;
            float distance = toPlayer.magnitude;

            if (distance > config.visionRange) return false;

            // Cone check
            if (Vector2.Angle(facing, toPlayer) > config.visionAngle * 0.5f) return false;

            // Line-of-sight check: anything on the obstacle mask between us and the player blocks vision.
            RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, distance, obstacleMask);
            return hit.collider == null;
        }
        
        public void CatchPlayer()
        {
            if (hasCaughtPlayer) return;
            hasCaughtPlayer = true;

            agent.ResetPath();
            agent.velocity = Vector3.zero;

            onPlayerCaught?.Invoke();
            PlayerCaught?.Invoke();
        }

        public void ResetAfterCatch()
        {
            hasCaughtPlayer = false;
            ChangeState(MonsterState.Patrol);
            agent.speed = config.patrolSpeed;
            patrolDestinationSet = false;
        }
        
        private void SetDestinationOnNavMesh(Vector2 target)
        {
            // Snap to the nearest walkable point so an off-mesh target doesn't cause the path to fail.
            Vector3 desired = new Vector3(target.x, target.y, transform.position.z);
            if (NavMesh.SamplePosition(desired, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        private bool HasReachedDestination()
        {
            return !agent.pathPending &&
                   agent.remainingDistance <= agent.stoppingDistance + arrivalTolerance;
        }

        [ContextMenu("Reload Config")]
        public void LoadConfig()
        {
            string path = Path.Combine(Application.streamingAssetsPath, configFileName);

            try
            {
                if (File.Exists(path))
                {
                    config = JsonUtility.FromJson<MonsterConfig>(File.ReadAllText(path));
                }
                else
                {
                    Debug.LogWarning($"Monster config not found at {path}. Creating one with defaults.");
                    Directory.CreateDirectory(Application.streamingAssetsPath);
                    File.WriteAllText(path, JsonUtility.ToJson(config, true));
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load monster config, using defaults. {e.Message}");
                config = new MonsterConfig();
            }

            if (agent != null)
            {
                agent.speed = CurrentState == MonsterState.Patrol ? config.patrolSpeed : config.chaseSpeed;
            }
        }

        private void OnDrawGizmosSelected()
        {
            MonsterConfig c = config ?? new MonsterConfig();
            Vector3 pos = transform.position;
            Vector3 f = facing.sqrMagnitude > 0f ? (Vector3)facing : Vector3.right;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pos, c.visionRange);

            Vector3 left = Quaternion.Euler(0, 0, c.visionAngle * 0.5f) * f;
            Vector3 right = Quaternion.Euler(0, 0, -c.visionAngle * 0.5f) * f;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + left * c.visionRange);
            Gizmos.DrawLine(pos, pos + right * c.visionRange);

            if (patrolPoints == null) return;
            Gizmos.color = Color.cyan;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] == null) continue;
                Gizmos.DrawWireSphere(patrolPoints[i].position, 0.15f);
                Transform next = patrolPoints[(i + 1) % patrolPoints.Length];
                if (next != null) Gizmos.DrawLine(patrolPoints[i].position, next.position);
            }
        }
    }
}