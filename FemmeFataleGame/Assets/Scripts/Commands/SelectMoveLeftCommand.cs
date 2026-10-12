namespace FemmeFatale.Commands
{

    public class SelectMoveLeftCommand : IPlayerCommand
    {
        public void Execute(PlayerController player)
        {
            player.SelectMoveLeft();
        }
    }
}