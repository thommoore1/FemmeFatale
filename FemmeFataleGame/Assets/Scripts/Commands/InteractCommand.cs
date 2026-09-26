namespace FemmeFatale.Commands
{
    public class InteractCommand : IPlayerCommand
    {
        public void Execute(PlayerController player)
        {
            player.Interact();
        }
    }
}
