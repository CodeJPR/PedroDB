namespace PedroEngine {
    public class Engine {

        #region Variables

        private readonly string path;
        private readonly List<User> users = new();


        #endregion

        #region Constructor

        public Engine(string path) {
            this.path = path;
        }

        #endregion

        #region Methods

        public void AddUser(string username, string password) {
            users.Add(new User(username, password));
        }

        public void AddDatabase(string name, string user) {

        }

        #endregion
    }
}