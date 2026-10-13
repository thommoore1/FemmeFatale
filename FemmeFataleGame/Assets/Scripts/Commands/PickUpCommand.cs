namespace FemmeFatale.Commands
{
    public class PickUpCommand : IPlayerCommand
    {
        public void Execute(PlayerController player)
        {
            player.PickUp();
        }
    }
}