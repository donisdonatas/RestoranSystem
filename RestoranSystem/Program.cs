using RestoranSystem.Model;
using RestoranSystem.Services;

namespace RestoranSystem
{
    internal class Program
    {
        static void Main()
        {
            DefaultDatabaseServices DefaultDatabase = new DefaultDatabaseServices();
            DefaultDatabase.CreateAllDatabaseTables();

            SystemMenu Menu = new SystemMenu();
            Menu.GetPrimaryMenu();
        }
    }
}