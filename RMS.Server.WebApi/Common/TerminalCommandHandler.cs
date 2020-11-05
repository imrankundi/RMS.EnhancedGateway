using RMS.Server.DataTypes;
using System.Collections.Concurrent;

namespace RMS.Server.WebApi.Common
{
    public class TerminalCommandHandler
    {
        private static readonly TerminalCommandHandler instance = new TerminalCommandHandler();
        public static TerminalCommandHandler Instance => instance;

        private static readonly ConcurrentDictionary<string, TerminalCommand> commands =
           new ConcurrentDictionary<string, TerminalCommand>();
        private TerminalCommandHandler()
        {

        }
        public bool Add(TerminalCommand command)
        {
            if (command == null)
                return false;

            if (string.IsNullOrEmpty(command.TerminalId))
                return false;

            if (!commands.ContainsKey(command.TerminalId))
            {
                var result = commands.TryAdd(command.TerminalId, command);
                return result;
            }

            return false;
        }
        public bool Remove(string terminalId)
        {
            if (commands.ContainsKey(terminalId))
            {
                var result = commands.TryRemove(terminalId, out TerminalCommand command);
                return result;

            }
            return false;
        }

        public TerminalCommand Find(string terminalId)
        {
            var result = commands.TryGetValue(terminalId, out TerminalCommand command);
            return command;
        }
    }
}
