using System;

namespace Euler.Patterns.Bridge.Drivers
{
    public class OracleConnectionDriver : IConnectionDriver
    {
        public string ExecuteQuery(string connection, string query)
        {
            Console.WriteLine("Running Query Against Oracle DB");
            return $"Returning query result via {nameof(OracleConnectionDriver)}";
        }

        public void Dispose()
        {
            // free up some connection resources
        }
    }
}