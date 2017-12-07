namespace Euler.Patterns.Bridge.Drivers
{
    public interface IDriverManager
    {
        void Use(IConnectionDriver driver);
        IConnection GetConnection(string connectionAddress);
    }
}