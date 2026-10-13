namespace FemmeFatale.Commands
{
    public class UseCommand : IPlayerCommand
    {
        public void Execute(PlayerController player)
        {
            player.Use();
        }
    }
}