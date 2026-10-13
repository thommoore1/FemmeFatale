namespace FemmeFatale.Commands
{
    public class SelectMoveRightCommand : IPlayerCommand
    {
        public void Execute(PlayerController player)
        {
            player.SelectMoveRight();
        }
    }
}