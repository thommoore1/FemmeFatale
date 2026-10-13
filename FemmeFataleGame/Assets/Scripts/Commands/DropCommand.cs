namespace FemmeFatale.Commands
{
    public class DropCommand : IPlayerCommand
    {
        public void Execute(PlayerController player)
        {
            player.Drop();
        }
    }
}