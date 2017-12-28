namespace Euler.Patterns.Bridge.Drivers
{
    public interface IDriverManager
    {
        void Use(IConnectionDriver driver);
        IConnection CreateConnection(string connectionAddress);
    }
}