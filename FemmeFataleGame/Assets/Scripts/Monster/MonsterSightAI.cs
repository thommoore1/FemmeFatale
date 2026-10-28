using System;
using System.IO;
using System.Collections;
using FemmeFatale.Save;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FemmeFatale
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class MonsterAI : SaveableTransform
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
        private float catchDistance = 2f;

        [Tooltip("Extra slack when deciding the monster has arrived at a destination.")] [SerializeField]
        private float arrivalTolerance = 0.1f;

        [Tooltip("How far from the player's last position we'll look for a valid NavMesh point.")] [SerializeField]
        private float navMeshSampleDistance = 2f;
        
        [Header("Catch Events")] [Tooltip("Hook up methods in the Inspector to run when the player is caught.")]
        public UnityEvent onPlayerCaught;

        // [Header("Catch Events That Need Parameters")]
        // [Tooltip("Hook up methods with specific parameters to run when the player is caught.")]
        // public UnityEvent<string, float> onPlayerCaught2;
        
        [Header("Monster Config")] [Tooltip("Monster stats scriptable object")]
        [SerializeField]
        private MonsterConfig config;

        // Subscribe from code: monster.PlayerCaught += MyMethod;</summary>
        //public event Action PlayerCaught;

        public MonsterState CurrentState { get; private set; } = MonsterState.Patrol;

        private NavMeshAgent agent;
        private Vector2 facing = Vector2.right;
        private Vector2 lastKnownPlayerPosition;
        private bool patrolDestinationSet;
        private float searchTimer;
        private bool hasCaughtPlayer;
        public bool isFrozen = false;
        
        [SerializeField]
        private int patrolIndex;

        public int PatrolIndex
        {
            get => patrolIndex;
            set
            {
                if (patrolPoints == null || patrolPoints.Length == 0)
                {
                    patrolIndex = 0;
                    return;
                }

                patrolIndex = Mathf.Clamp(value, 0, patrolPoints.Length - 1);
                patrolDestinationSet = false;
            }
        }

        private MonsterAffection affection; 
        
        private void Awake()
        {
            affection = GetComponent<MonsterAffection>();
            agent = GetComponent<NavMeshAgent>();
            
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            
            if(CurrentState == MonsterState.Patrol)
                agent.speed = affection.PatrolModifier(config.PatrolSpeed);
            else if(CurrentState == MonsterState.Chase || CurrentState == MonsterState.Search)
                agent.speed = affection.ChaseModifier(config.ChaseSpeed);
            
        }
        
        private void OnEnable()
        {
            // Subscribing to the scene loaded event
            SaveManager.OnAfterLoad += OnAfterLoad;
        }

        private void OnDisable()
        {
            SaveManager.OnAfterLoad -= OnAfterLoad;
        }
        
        private void OnAfterLoad()
        {
            Debug.Log("SHould happen");
            Debug.Log(isFrozen);

            if (isFrozen)
            {
                Debug.Log("yifadsilbhfasonuifaw");
                StartCoroutine(WaitForFrozen());
            }
        }
        
        IEnumerator WaitForFrozen()
        {
            Debug.Log("Monster is frozen, waiting for 3 seconds...");
            agent.isStopped = true;
            yield return new WaitForSeconds(3.0f);
            isFrozen = false;
            agent.isStopped = false;
        }

        private void Update()
        {
            if (isFrozen || hasCaughtPlayer || player == null) return;
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
            if (searchTimer >= config.SearchDuration) // Add search modifier if needed: affection.SearchModifier(config.SearchDuration)
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
                    agent.speed = affection.PatrolModifier(config.PatrolSpeed);
                    patrolDestinationSet = false;
                    break;

                case MonsterState.Chase:
                    agent.speed = affection.ChaseModifier(config.ChaseSpeed);
                    patrolDestinationSet = false;
                    break;

                case MonsterState.Search:
                    agent.speed = affection.ChaseModifier(config.ChaseSpeed);
                    SetDestinationOnNavMesh(lastKnownPlayerPosition);
                    break;
            }
        }
        

        private bool CanSeePlayer()
        {
            Vector2 origin = transform.position;
            Vector2 toPlayer = (Vector2)player.position - origin;
            float distance = toPlayer.magnitude;

            if (distance > affection.RangeModifier(config.VisionRange)) return false;

            // Cone check
            if (Vector2.Angle(facing, toPlayer) > affection.VisionAngleModifier(config.VisionAngle)) return false;

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
            Debug.Log(config.monsterName);

            isFrozen = true;
            
            //onPlayerCaught2?.Invoke(config.monsterName, affection.Affection);
            onPlayerCaught?.Invoke();
            //PlayerCaught?.Invoke();
        }

        public void ResetAfterCatch()
        {
            hasCaughtPlayer = false;
            ChangeState(MonsterState.Patrol);
            agent.speed = affection.PatrolModifier(config.PatrolSpeed);
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

        private void OnDrawGizmosSelected()
        {
            
            Vector3 pos = transform.position;
            Vector3 f = facing.sqrMagnitude > 0f ? (Vector3)facing : Vector3.right;
            
            float range = affection.RangeModifier(config.VisionRange);
            float angle = affection.VisionAngleModifier(config.VisionAngle);
            
            //Debug.Log($"{config.VisionAngle}{angle}{config.RangeModi}{}");

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pos, range);

            Vector3 left = Quaternion.Euler(0, 0, angle * 0.5f) * f;
            Vector3 right = Quaternion.Euler(0, 0, -angle * 0.5f) * f;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + left * range);
            Gizmos.DrawLine(pos, pos + right * range);

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