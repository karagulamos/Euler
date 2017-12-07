using System;

namespace Euler.Patterns.Bridge.Drivers
{
    public interface IConnection : IDisposable
    {
        void ExecuteQuery(string query);
        string Address { set; }
        string Result { get; }
    }
}