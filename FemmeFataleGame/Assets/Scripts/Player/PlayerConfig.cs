using UnityEngine;


namespace FemmeFatale
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float moveSpeed = 6f;
        
        public float MoveSpeed => moveSpeed;
    }
}