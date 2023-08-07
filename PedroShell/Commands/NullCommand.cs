namespace PedroDB.Shell.Commands
{
    internal class NullCommand : ICommand {
        public string Name => "";

        public string Args { get ; set; }

        public void Execute(string args) {
            throw new NotImplementedException();
        }

        public void Execute() {
            throw new NotImplementedException();
        }

        public string GetHelp() {
            throw new NotImplementedException();
        }

        public bool IsValid(string args) {
            throw new NotImplementedException();
        }

        public bool IsValid() {
            throw new NotImplementedException();
        }
    }
}