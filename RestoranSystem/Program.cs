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

            //SQLiteDBCheck checkDBTypes = new SQLiteDBCheck();
            //checkDBTypes.CheckTablesTableColumnTypes();
            //checkDBTypes.CheckMenuTableColumnTypes();

            //CustomerReception customer = new CustomerReception();
            //customer.ReservateTable();

            SystemMenu Menu = new SystemMenu();
            Menu.GetPrimaryMenu();


            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Good Bye!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}