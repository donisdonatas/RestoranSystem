using RestoranSystem.Model;
using RestoranSystem.Struct;
using RestoranSystem.Utilities;

namespace RestoranSystem.Services
{
    public class CustomerReceptionServices
    {
        public void InitializeReception()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Klientų priėmimas / staliuko rezervavimas");
            Console.WriteLine("Ar norite priimti klientus?");
            Console.WriteLine("[1] Taip. Staliuko rezervavimas.");
            Console.WriteLine("[2] Ne. Staliuko atlaisvinimas.");

            int MenuChoice = InputValidation.ValidateInput(2);
            OpenMenuByChoice(MenuChoice);
        }

        protected void OpenMenuByChoice(int choice)
        {
            switch(choice)
            {
                case 1:
                    StartTableReservation();
                    break;
                case 2:
                    CancelReservation();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error");
                    break;
            }
            SystemMenu Menu = new SystemMenu();
            Menu.BackToMainMenu();
        }

        protected void StartTableReservation()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Įveskite žmonių skaičių:");
            int NumberOfCustomers = InputValidation.ValidateInput();
            List<AvailableTable> AvailableTables = GetAvailableTable();
            AvailableTable Table = ChooseTable(NumberOfCustomers, AvailableTables);
            CreateTableReservation(Table, NumberOfCustomers);
        }

        protected void CreateTableReservation(AvailableTable table, int customers)
        {
            if (table.TableID != 0)
            {
                string SQLString = $"UPDATE Tables SET isReserved=1, OccupiedSeats={customers} WHERE TableID={table.TableID};";
                SQLiteServices.UpdateSQLTable(SQLString);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Rezervuoto staliuko numeris: {table.TableID} ({table.Seats} vietos).");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nėra laisvo staliuko, tokiam žmonių kiekiui.");
            }
        }

        protected List<AvailableTable> GetAvailableTable()
        {
            List<AvailableTable> AvailableTables = SQLiteServices.GetAvailableTableList();
            return AvailableTables;
        }

        protected AvailableTable ChooseTable(int numberOfCustomers, List<AvailableTable> availableTables)
        {
            IEnumerable<AvailableTable> TablesForOrder = availableTables
                                                            .Where(availableSeats => availableSeats.Seats - numberOfCustomers >= 0)
                                                            .OrderBy(availableSeats => availableSeats.Seats - numberOfCustomers >= 0);
            return TablesForOrder.FirstOrDefault();
        }

        protected void CancelReservation()
        {
            List<ReservedTable> ReservedTables = SQLiteServices.GetReservedTables();
            if(ReservedTables.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pasirinkite iš sąrašo, kurį staliuką norite atrezervuoti:");
                int KeyboardKey = 0;
                foreach (ReservedTable table in ReservedTables)
                {
                    ++KeyboardKey;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"[{KeyboardKey}] Staliuko numeris: {table.TableID}");
                }
                InputValidation.ValidateInput(KeyboardKey);
                string SQLString = $"UPDATE Tables SET isReserved=0, OccupiedSeats=0 WHERE TableID={ReservedTables[KeyboardKey - 1].TableID};";
                SQLiteServices.UpdateSQLTable(SQLString);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Nėra galimų atlaisvinti staliukų.");
            }
        }
    }
}
