using System;

namespace Euler.Patterns.Bridge.Drivers
{
    public class SqlServerConnectionDriver : IConnectionDriver
    {
        public string ExecuteQuery(string connection, string query)
        {
            Console.WriteLine("Running Query Against Sql Server");
            return $"Returning query result via {nameof(SqlServerConnectionDriver)}";
        }

        public void Dispose()
        {
            // free up some connection resources
        }
    }
}