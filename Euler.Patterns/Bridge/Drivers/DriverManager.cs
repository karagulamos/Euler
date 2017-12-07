// Author: Alexander Karagulamos
// Date:
// Comment:

namespace Euler.Patterns.Bridge.Drivers
{
    public class DriverManager : IDriverManager
    {
        // helper factory
        public static IDriverManager GetInstance() => new DriverManager();

        public void Use(IConnectionDriver driver) => Driver = driver;

        private IConnectionDriver Driver { get; set; }

        public IConnection GetConnection(string address)
        {
            return new Connection(Driver) { Address = address };
        }

        // This can be made internal and extracted into it's own file. Your mileage may
        // differ, but no client should have knowledge of this helper class as its 
        // implementation is subject to change -- including its name :D
        //
        // In other words, we must force clients to depend on the IConnection abstraction.
        private class Connection : IConnection
        {
            private readonly IConnectionDriver _driver;

            public Connection(IConnectionDriver driver)
            {
                _driver = driver;
            }

            public void ExecuteQuery(string query)
            {
                Result = _driver.ExecuteQuery(Address, query);
            }

            public string Address { private get; set; }
            public string Result { get; private set; }

            public void Dispose()
            {
                _driver.Dispose();
            }
        }
    }
}