using UnityEngine;

namespace FemmeFatale
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStatsConfigLoader))]
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; private set; }
        public Vector2 CurrentMoveInput { get; private set; }
        public Vector2 FacingDirection { get; private set; } = Vector2.down;

        private PlayerStatsConfigLoader _configLoader;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Rigidbody.gravityScale = 0f;
            Rigidbody.freezeRotation = true;

            _configLoader = GetComponent<PlayerStatsConfigLoader>();
        }

        private void FixedUpdate()
        {
            PlayerStatsConfig config = _configLoader.Config;
            Rigidbody.MovePosition(Rigidbody.position + CurrentMoveInput * config.moveSpeed * Time.fixedDeltaTime);
        }
        
        public void SetMoveInput(Vector2 input)
        {
            CurrentMoveInput = input.sqrMagnitude > 1f ? input.normalized : input;

            if (CurrentMoveInput.sqrMagnitude > 0.01f)
            {
                FacingDirection = CurrentMoveInput.normalized;
            }
        }
        
        public void Interact()
        {
            Debug.Log($"{name} interacts, facing {FacingDirection}");
        }
    }
}