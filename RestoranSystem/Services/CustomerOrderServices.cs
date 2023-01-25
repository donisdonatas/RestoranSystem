using RestoranSystem.Model;
using RestoranSystem.Struct;
using RestoranSystem.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RestoranSystem.Services
{
    public class CustomerOrderServices
    {
        private int _tableId = 0;
        public List<int> Order = new List<int>();
        public List<MenuLine> Menu = SQLiteServices.GetRestaurantMenu();

        public int TableId
        {
            get { return _tableId; }
            set { _tableId = value; }
        }

        public void InitializeOrder()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Užsakymų priėmimas");
            Console.WriteLine("Pasirinkite staliuką užsakymo priėmimui:");
            TableId = SelectReservedTables();
            if(TableId != 0)
            {
                GiveCustomersMenu();
                CompleteOrder();
                //PrintOrder();
            }
            //else
            //{
            //    Console.ForegroundColor = ConsoleColor.Magenta;
            //    Console.WriteLine("Nėra staliukų laukiančių užsakymo priėmimo.");

            //}
            SystemMenu Menu = new SystemMenu();
            Menu.BackToMainMenu();
        }

        protected int SelectReservedTables()
        {
            List<AvailableTable> ReservedTables = SQLiteServices.GetReservedTableList();
            if(ReservedTables.Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("-----------------");
                int KeyboardKey = 0;
                foreach (AvailableTable table in ReservedTables)
                {
                    ++KeyboardKey;
                    Console.WriteLine($"[{KeyboardKey}] Staliukas Nr. {table.TableID}. Laukia {table.OccupiedSeats} žmonės."); 
                }
                Console.WriteLine("-----------------");
                InputValidation.ValidateInput(KeyboardKey);
                return ReservedTables[KeyboardKey - 1].TableID;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Nėra staliukų laukiančių užsakymo priėmimo");
                return 0;
            }
        }

        protected void GiveCustomersMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MENU");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-----------------");
            int NumericKey = 0;
            string MealType = "";
            foreach (MenuLine menuLine in Menu)
            {
                if (MealType != menuLine.MealType)
                {
                    MealType = menuLine.MealType;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{menuLine.MealType}:");
                    ++NumericKey;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"[{NumericKey}] {menuLine.MealName} - {menuLine.MealPrice}");
                    
                }
                else
                {
                    ++NumericKey;
                    Console.WriteLine($"[{NumericKey}] {menuLine.MealName} - {menuLine.MealPrice}");
                }
            }
            Console.WriteLine("-----------------");
            SelectMenuItems();
        }

        protected void SelectMenuItems()
        {
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sudarykite užsakymą iš pateikto meniu:");
            Console.WriteLine("Užsakymą, galite vesti į vieną eitutę, reikšmes atskirant kableliu \",\":");
            string? InputLine;
            bool IsOrderComplete = false;
            List<string> _orderList = new List<string>();
            while (!IsOrderComplete)
            {
                Console.ForegroundColor = ConsoleColor.White;
                InputLine = Console.ReadLine();
                _orderList.Add(InputLine);
                object[] FullOrder = Converter.ConvertListStringToListInt(_orderList);
                ValidateOrder(FullOrder);
                IsOrderComplete = !AddAnotherLine();
            }
            
        }

        protected void ValidateOrder(object[] Lists)
        {
            List<int> GoodList = (List<int>)Lists[0];
            List<string> FailedList = (List<string>)Lists[1];
            Order.AddRange(GoodList);
            if (FailedList.Any())
            {
                foreach (string str in FailedList)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Blogai įvesta reikšmė: {str}");
                }
                //if(AddAnotherLine()) SelectMenuItems();
                //FailedList.Clear();
            }
        }

        protected bool AddAnotherLine()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ar norite papildyti užsakymą?");
            Console.WriteLine("[1] Taip");
            Console.WriteLine("[2] Ne");
            int NumericKey = InputValidation.ValidateInput(2);
            return Converter.ConvertToBool(NumericKey, 1);
        }

        //protected void CheckAnswer(int key)
        //{
        //    switch(key)
        //    {
        //        case 1: 
        //            SelectMenuItems();
        //            break;
        //        case 2:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        protected void CompleteOrder()
        {
            SQLiteServices.GenerateNewAccountingID();
            SQLiteServices.WriteOrderToDB(TableId, Order);
            string AcceptOrder = $"UPDATE Tables SET isOrderAccepted=1 WHERE TableID={TableId};";
            SQLiteServices.UpdateSQLTable(AcceptOrder);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Užsakymas priimtas.");
        }

        protected void PrintOrder()
        {
            foreach(int m in Order)
            {
                // Paliekam neužbaigtą
            }
        }
    }
}
