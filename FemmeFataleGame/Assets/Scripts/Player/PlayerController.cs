using UnityEngine;

namespace FemmeFatale
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; private set; }
        public Vector2 CurrentMoveInput { get; private set; }
        public Vector2 FacingDirection { get; private set; } = Vector2.down;
        
        
        [SerializeField]
        private PlayerConfig _playerConfig;
        [SerializeField]
        private InventoryController _inventoryController;
        
        
        private float checkRadius = 0.5f; //TODO: Make this configurable
        [SerializeField]
        private LayerMask targetLayer;
        
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Rigidbody.gravityScale = 0f;
            Rigidbody.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            Rigidbody.MovePosition(Rigidbody.position + CurrentMoveInput * _playerConfig.MoveSpeed * Time.fixedDeltaTime);
        }
        
        public void SetMoveInput(Vector2 input)
        {
            CurrentMoveInput = input.sqrMagnitude > 1f ? input.normalized : input;

            if (CurrentMoveInput.sqrMagnitude > 0.01f)
            {
                FacingDirection = CurrentMoveInput.normalized;
            }
        }
        
        public void Use()
        {
            Debug.Log($"{name} interacts, facing {FacingDirection}");
        }

        public void PickUp()
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, checkRadius, targetLayer);

            if (hitCollider)
            {
                this.GetComponent<PlayerItemCollector>().AddToInventory(hitCollider.GetComponent<Item>());
            }
            
        }

        public void Drop()
        {
            _inventoryController.Drop(this.transform.position);
        }
        
        public void SelectMoveRight()
        {
            _inventoryController.SelectNextSlot();
        }
        
        public void SelectMoveLeft()
        {
            _inventoryController.SelectPreviousSlot();;
        }
    }
}