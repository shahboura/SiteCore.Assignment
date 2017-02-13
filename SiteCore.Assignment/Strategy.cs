using System;

namespace SiteCore.Assignment
{
    public class Strategy
    {
        private readonly Func<string, bool> _predicate;
        private readonly Action<string> _command;
        public string Input { get; set; }

        public Strategy(Func<string, bool> predicate, Action<string> command)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _predicate = predicate;
            _command = command;
        }

        public bool IsCurrent(string input)
        {
            var isCurrent = _predicate(input);
            if (!isCurrent)
            {
                return false;
            }

            Input = input;
            return true;
        }

        public void PerformCommand()
        {
            _command(Input);
        }
    }
}
