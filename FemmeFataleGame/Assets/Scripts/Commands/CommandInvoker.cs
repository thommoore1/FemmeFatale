using System.Collections.Generic;

namespace FemmeFatale.Commands
{
    public class CommandInvoker
    {
        private readonly Queue<IPlayerCommand> _queue = new Queue<IPlayerCommand>();
        private readonly List<IPlayerCommand> _history = new List<IPlayerCommand>();
        private readonly int _historyLimit;

        public CommandInvoker(int historyLimit = 200)
        {
            _historyLimit = historyLimit;
        }

        public IReadOnlyList<IPlayerCommand> History => _history;

        public void Enqueue(IPlayerCommand command)
        {
            _queue.Enqueue(command);
        }

        public void ProcessAll(PlayerController player)
        {
            while (_queue.Count > 0)
            {
                IPlayerCommand command = _queue.Dequeue();
                command.Execute(player);

                _history.Add(command);
                if (_history.Count > _historyLimit)
                {
                    _history.RemoveAt(0);
                }
            }
        }
    }
}
