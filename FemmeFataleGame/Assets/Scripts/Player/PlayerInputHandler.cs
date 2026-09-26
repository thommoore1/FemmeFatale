using FemmeFatale.Commands;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FemmeFatale
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInputHandler : MonoBehaviour, PlayerControls.IPlayerActions
    {
        private PlayerController _player;
        private PlayerControls _controls;
        private CommandInvoker _invoker;

        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            _invoker = new CommandInvoker();

            _controls = new PlayerControls();
            _controls.Player.SetCallbacks(this);
        }

        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();
        private void OnDestroy() => _controls.Dispose();

        private void Update()
        {
            _invoker.ProcessAll(_player);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _invoker.Enqueue(new MoveCommand(input));
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _invoker.Enqueue(new InteractCommand());
            }
        }
    }
}
