using UnityEngine;

namespace FemmeFatale.Commands
{
    public class MoveCommand : IPlayerCommand
    {
        private readonly Vector2 _direction;

        public MoveCommand(Vector2 direction)
        {
            _direction = direction;
        }

        public void Execute(PlayerController player)
        {
            player.SetMoveInput(_direction);
        }
    }
}
