using System;

namespace Euler.Patterns.Bridge.Drivers
{
    public interface IConnectionDriver : IDisposable
    {
        string ExecuteQuery(string connection, string query);
    }
}