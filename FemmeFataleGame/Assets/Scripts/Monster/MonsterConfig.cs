using UnityEngine;

namespace FemmeFatale
{
    [CreateAssetMenu(fileName = "MonsterConfig", menuName = "Scriptable Objects/MonsterConfig")]
    public class MonsterConfig : ScriptableObject
    {
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float chaseSpeed = 3.5f;
        [SerializeField] private float visionRange = 6f;
        [SerializeField] private float visionAngle = 90f;
        [SerializeField] private float searchDuration = 3f;

        public float PatrolSpeed => patrolSpeed;
        public float ChaseSpeed => chaseSpeed;
        public float VisionRange => visionRange;
        public float VisionAngle => visionAngle;
        public float SearchDuration => searchDuration;
    }
}   
